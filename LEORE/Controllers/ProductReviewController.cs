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
    public class ProductReviewController : Controller
    {
        private readonly LEOREContext _context;

        public ProductReviewController(LEOREContext context)
        {
            _context = context;
        }

        // GET: ProductReview
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductReviews.Include(p => p.Product).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductReview/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productReview = await _context.ProductReviews
                .Include(p => p.Product)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (productReview == null)
            {
                return NotFound();
            }

            return View(productReview);
        }

        // GET: ProductReview/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Set<Product>(), "ProductID", "Name");
            ViewData["UserID"] = new SelectList(_context.Set<User>(), "UserId", "Email");
            return View();
        }

        // POST: ProductReview/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewID,ProductID,UserID,Comment,Rating,CreatedAt")] ProductReview productReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Set<Product>(), "ProductID", "Name", productReview.ProductID);
            ViewData["UserID"] = new SelectList(_context.Set<User>(), "UserId", "Email", productReview.UserID);
            return View(productReview);
        }

        // GET: ProductReview/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productReview = await _context.ProductReviews.FindAsync(id);
            if (productReview == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Set<Product>(), "ProductID", "Name", productReview.ProductID);
            ViewData["UserID"] = new SelectList(_context.Set<User>(), "UserId", "Email", productReview.UserID);
            return View(productReview);
        }

        // POST: ProductReview/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewID,ProductID,UserID,Comment,Rating,CreatedAt")] ProductReview productReview)
        {
            if (id != productReview.ReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductReviewExists(productReview.ReviewID))
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
            ViewData["ProductID"] = new SelectList(_context.Set<Product>(), "ProductID", "Name", productReview.ProductID);
            ViewData["UserID"] = new SelectList(_context.Set<User>(), "UserId", "Email", productReview.UserID);
            return View(productReview);
        }

        // GET: ProductReview/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productReview = await _context.ProductReviews
                .Include(p => p.Product)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (productReview == null)
            {
                return NotFound();
            }

            return View(productReview);
        }

        // POST: ProductReview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productReview = await _context.ProductReviews.FindAsync(id);
            if (productReview != null)
            {
                _context.ProductReviews.Remove(productReview);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductReviewExists(int id)
        {
            return _context.ProductReviews.Any(e => e.ReviewID == id);
        }
    }
}
