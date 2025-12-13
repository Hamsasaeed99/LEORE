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
    public class WishListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WishList
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.WishList.Include(w => w.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: WishList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishList
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.WishlistID == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
        }

        // GET: WishList/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: WishList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WishlistID,UserID")] WishList wishList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wishList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.User, "UserId", "Email", wishList.UserID);
            return View(wishList);
        }

        // GET: WishList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishList.FindAsync(id);
            if (wishList == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.User, "UserId", "Email", wishList.UserID);
            return View(wishList);
        }

        // POST: WishList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WishlistID,UserID")] WishList wishList)
        {
            if (id != wishList.WishlistID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishListExists(wishList.WishlistID))
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
            ViewData["UserID"] = new SelectList(_context.User, "UserId", "Email", wishList.UserID);
            return View(wishList);
        }

        // GET: WishList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishList
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.WishlistID == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
        }

        // POST: WishList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wishList = await _context.WishList.FindAsync(id);
            if (wishList != null)
            {
                _context.WishList.Remove(wishList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishListExists(int id)
        {
            return _context.WishList.Any(e => e.WishlistID == id);
        }
    }
}
