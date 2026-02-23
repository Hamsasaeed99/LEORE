using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LEORE.Models;
using Microsoft.AspNetCore.Authorization;

namespace LEORE.Controllers
{
    public class CartController : Controller
    {
        private readonly LEOREContext _context;

        public CartController(LEOREContext context)
        {
            _context = context;
        }

        // GET: Cart (للأدمن فقط)

        public async Task<IActionResult> Index()
        {
            var carts = await _context.Carts
                .Include(c => c.User)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .ToListAsync();
            return View(carts);
        }

        // GET: MyCart (عرض كارت المستخدم المسجل)
        public async Task<IActionResult> MyCart()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["ErrorMessage"] = "Please login to view your cart";
                return RedirectToAction("Login", "Account");
            }

            // البحث عن كارت المستخدم أو إنشاء واحد جديد
            var cart = await GetOrCreateCartAsync(userId.Value);

            // حساب الإجمالي
            ViewBag.TotalPrice = CalculateCartTotal(cart);

            return View(cart);
        }

        // POST: Cart/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Json(new { success = false, message = "Please login to add items to cart" });
            }

            try
            {
                var cart = await GetOrCreateCartAsync(userId.Value);
                var product = await _context.Products.FindAsync(productId);

                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }

                // البحث عن المنتج في الكارت
                var existingItem = cart.CartItems
                    .FirstOrDefault(ci => ci.ProductID == productId);

                if (existingItem != null)
                {
                    // تحديث الكمية إذا المنتج موجود
                    existingItem.Quantity += quantity;
                }
                else
                {
                    // إضافة منتج جديد للكارت
                    var cartItem = new CartItem
                    {
                        ProductID = productId,
                        Quantity = quantity,
                        CartId = cart.CartId
                    };
                    _context.CartItems.Add(cartItem);
                }

                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Product added to cart",
                    cartCount = GetCartItemCount(userId.Value)
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // POST: Cart/UpdateQuantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Json(new { success = false, message = "Please login" });
            }

            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.CartItemsId == cartItemId && ci.Cart.UserId == userId);

            if (cartItem == null)
            {
                return Json(new { success = false, message = "Item not found" });
            }

            if (quantity <= 0)
            {
                // حذف العنصر إذا كانت الكمية صفر أو أقل
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
            }

            await _context.SaveChangesAsync();

            var cart = await GetOrCreateCartAsync(userId.Value);
            var total = CalculateCartTotal(cart);

            return Json(new
            {
                success = true,
                totalPrice = total,
                cartCount = GetCartItemCount(userId.Value)
            });
        }

        // POST: Cart/RemoveItem
        [HttpPost]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Json(new { success = false, message = "Please login" });
            }

            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.CartItemsId == cartItemId && ci.Cart.UserId == userId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            var cart = await GetOrCreateCartAsync(userId.Value);
            var total = CalculateCartTotal(cart);

            return Json(new
            {
                success = true,
                totalPrice = total,
                cartCount = GetCartItemCount(userId.Value)
            });
        }

        // POST: Cart/ClearCart
        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Json(new { success = false, message = "Please login" });
            }

            var cart = await GetOrCreateCartAsync(userId.Value);

            if (cart.CartItems.Any())
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }

            return Json(new
            {
                success = true,
                message = "Cart cleared",
                cartCount = 0
            });
        }

        public async Task<IActionResult> Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cart = await GetOrCreateCartAsync(userId.Value);

            if (!cart.CartItems.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty";
                return RedirectToAction("MyCart");
            }

            ViewBag.TotalPrice = CalculateCartTotal(cart);
            return View(cart);
        }

        // ========== دوال مساعدة ==========

        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                // إنشاء كارت جديد للمستخدم
                cart = new Cart
                {
                    UserId = userId
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        private decimal CalculateCartTotal(Cart cart)
        {
            return cart.CartItems.Sum(item => item.Quantity * (item.Product?.Price ?? 0));
        }

        private int GetCartItemCount(int userId)
        {
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId);

            return cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;
        }

        private async Task<Order> CreateOrderFromCart(int userId, Cart cart, string shippingAddress, string paymentMethod)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // إنشاء الطلب
                var order = new Order
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    OrderStatus = "Pending",
                    TotalAmount = CalculateCartTotal(cart),
                    PaymentMethod = paymentMethod,
                    ShippingAddress = shippingAddress


                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // الحصول على OrderId

                // إضافة العناصر من الكارت إلى الطلب
                foreach (var cartItem in cart.CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderID,
                        ProductID = cartItem.ProductID,
                        Quantity = cartItem.Quantity,
                        PriceAtPurchase = cartItem.Product?.Price ?? 0
                    };
                    _context.OrderItems.Add(orderItem);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(string ShippingAddress)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cart = await GetOrCreateCartAsync(userId.Value);

            if (!cart.CartItems.Any())
                return RedirectToAction("MyCart");

            // إنشاء الأوردر
            var order = await CreateOrderFromCart(
                userId.Value,
                cart,
                ShippingAddress,
                "Cash on Delivery"
            );

            // تفريغ الكارت
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            // فتح صفحة الانفويس
            return RedirectToAction("Invoice", "Order", new { id = order.OrderID });
        }


    }
}
