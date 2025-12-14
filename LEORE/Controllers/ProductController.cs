using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LEORE.Models;


namespace LEORE.Controllers
{
    public class ProductController : Controller
    {
        private readonly LEOREContext _context;

        public ProductController(LEOREContext context)
        {
            _context = context;
        }

        // ============================
        // GET: Product
        // List all products + filter by category
        // ============================
        public async Task<IActionResult> Index(int? categoryId)
        {
            var products = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (categoryId != null)
            {
                products = products.Where(p => p.CategoryID == categoryId);
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(await products.ToListAsync());
        }

        // ============================
        // GET: Product/Details/5
        // Product details page
        // ============================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductReviews) 
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        
        public async Task<IActionResult> AddToCart(int ProductID, int Quantity)
        {
            // افترض إن المستخدم مسجل دخوله
            var userId = 1; // لو للـ test، بعدين تحطي User.Identity

            // جلب الكارت الخاص بالمستخدم
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // تحقق هل المنتج موجود بالفعل في الكارت
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cart.CartId && ci.ProductID == ProductID);

            if (cartItem != null)
            {
                // لو موجود زود الكمية
                cartItem.Quantity += Quantity;
                _context.CartItems.Update(cartItem);
            }
            else
            {
                // لو مش موجود اضيف جديد
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductID = ProductID,
                    Quantity = Quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            // رجوع لصفحة المنتج أو الكارت
            return RedirectToAction("Index", "Cart");
        }


        // ============================
        // GET: Product/Create
        // Admin only (optional)
        // ============================
        public IActionResult Create()
        {
            ViewData["CategoryID"] =
                new SelectList(_context.Categories, "CategoryId", "Name");

            return View();
        }

        // ============================
        // POST: Product/Create
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryID"] =
                new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryID);

            return View(product);
        }

        // ============================
        // GET: Product/Edit/5
        // ============================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            ViewData["CategoryID"] =
                new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryID);

            return View(product);
        }

        // ============================
        // POST: Product/Edit/5
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryID"] =
                new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryID);

            return View(product);
        }

        // ============================
        // GET: Product/Delete/5
        // ============================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // ============================
        // POST: Product/Delete/5
        // ============================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ============================
        // Helper
        // ============================
        private bool ProductExists(int id)
        {
            return _context.Products.Any(p => p.ProductID == id);
        }
    }
}
















//namespace LEORE.Controllers
//{
//    public class ProductController : Controller
//    {
//        private readonly LEOREContext _context;

//        public ProductController(LEOREContext context)
//        {
//            _context = context;
//        }

//        // GET: Product
//        public async Task<IActionResult> Index()
//        {
//            var lEOREContext = _context.Products.Include(p => p.Category);
//            return View(await lEOREContext.ToListAsync());
//        }

//        // GET: Product/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var product = await _context.Products
//                .Include(p => p.Category)
//                .FirstOrDefaultAsync(m => m.ProductID == id);
//            if (product == null)
//            {
//                return NotFound();
//            }

//            return View(product);
//        }

//        // GET: Product/Create
//        public IActionResult Create()
//        {
//            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
//            return View();
//        }

//        // POST: Product/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("ProductID,CategoryID,Name,Description,Price,Color,Image,Stock,Size,CreatedAt")] Product product)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(product);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryID);
//            return View(product);
//        }

//        // GET: Product/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var product = await _context.Products.FindAsync(id);
//            if (product == null)
//            {
//                return NotFound();
//            }
//            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryID);
//            return View(product);
//        }

//        // POST: Product/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("ProductID,CategoryID,Name,Description,Price,Color,Image,Stock,Size,CreatedAt")] Product product)
//        {
//            if (id != product.ProductID)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(product);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!ProductExists(product.ProductID))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryID);
//            return View(product);
//        }

//        // GET: Product/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var product = await _context.Products
//                .Include(p => p.Category)
//                .FirstOrDefaultAsync(m => m.ProductID == id);
//            if (product == null)
//            {
//                return NotFound();
//            }

//            return View(product);
//        }

//        // POST: Product/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var product = await _context.Products.FindAsync(id);
//            if (product != null)
//            {
//                _context.Products.Remove(product);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool ProductExists(int id)
//        {
//            return _context.Products.Any(e => e.ProductID == id);
//        }
//    }
//}
