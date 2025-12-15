using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LEORE.Data;
using LEORE.Models;

namespace LEORE.Controllers
{
    public class WishListItemController : Controller
    {
        private readonly LEOREContext _context;

        public WishListItemController(LEOREContext context)
        {
            _context = context;
        }

        // GET: WishListItem
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.WishListItems.Include(w => w.Product).Include(w => w.Wishlist);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: WishListItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishListItem = await _context.WishListItems
                .Include(w => w.Product)
                .Include(w => w.Wishlist)
                .FirstOrDefaultAsync(m => m.WishlistItemsId == id);
            if (wishListItem == null)
            {
                return NotFound();
            }

            return View(wishListItem);
        }

        // GET: WishListItem/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Set<Product>(), "ProductID", "Name");
            ViewData["WishlistID"] = new SelectList(_context.WishLists, "WishlistID", "WishlistID");
            return View();
        }

        // POST: WishListItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WishlistItemsId,WishlistID,AddedAt,ProductID")] WishListItem wishListItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wishListItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Set<Product>(), "ProductID", "Name", wishListItem.ProductID);
            ViewData["WishlistID"] = new SelectList(_context.WishLists, "WishlistID", "WishlistID", wishListItem.WishlistID);
            return View(wishListItem);
        }

        // GET: WishListItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishListItem = await _context.WishListItems.FindAsync(id);
            if (wishListItem == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Set<Product>(), "ProductID", "Name", wishListItem.ProductID);
            ViewData["WishlistID"] = new SelectList(_context.WishLists, "WishlistID", "WishlistID", wishListItem.WishlistID);
            return View(wishListItem);
        }

        // POST: WishListItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WishlistItemsId,WishlistID,AddedAt,ProductID")] WishListItem wishListItem)
        {
            if (id != wishListItem.WishlistItemsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishListItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishListItemExists(wishListItem.WishlistItemsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Set<Product>(), "ProductID", "Name", wishListItem.ProductID);
            ViewData["WishlistID"] = new SelectList(_context.WishLists, "WishlistID", "WishlistID", wishListItem.WishlistID);
            return View(wishListItem);
        }

        // GET: WishListItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishListItem = await _context.WishListItems
                .Include(w => w.Product)
                .Include(w => w.Wishlist)
                .FirstOrDefaultAsync(m => m.WishlistItemsId == id);
            if (wishListItem == null)
            {
                return NotFound();
            }

            return View(wishListItem);
        }

        // POST: WishListItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wishListItem = await _context.WishListItems.FindAsync(id);
            if (wishListItem != null)
            {
                _context.WishListItems.Remove(wishListItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishListItemExists(int id)
        {
            return _context.WishListItems.Any(e => e.WishlistItemsId == id);
        }
    }
}
