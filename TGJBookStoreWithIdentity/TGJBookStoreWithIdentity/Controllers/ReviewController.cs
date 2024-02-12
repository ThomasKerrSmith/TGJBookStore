using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TGJBookStoreWithIdentity.Models;
using X.PagedList;
//GM 20/11/22
namespace TGJBookStoreWithIdentity.Controllers
{
    public class ReviewController : Controller
    {
        private readonly TGJShopContext _context;

        public ReviewController(TGJShopContext context)
        {
            _context = context;
        }

        // GET: Review
        //will not allow sorting for some reason as Stripe Review clashes with lines 67 and 99 for some reason
        public IActionResult Index(string searchString, int? page)
        {
            //Paginated pages and Search
            var pageNumber = page ?? 1;
            var reviews = from r in _context.Reviews select r;
            if (!String.IsNullOrEmpty(searchString))
            {
                reviews = reviews.Where(s => s.BookTitle.Contains(searchString));
            }
            return View(reviews.ToPagedList(pageNumber, 4));
        }

        // GET: Review/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Review/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Review/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewId,BookTitle,Author,Reviewer,Rating,Description")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Review/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Review/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewId,BookTitle,Author,Reviewer,Rating,Description")] Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewId))
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
            return View(review);
        }

        // GET: Review/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Review/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reviews == null)
            {
                return Problem("Entity set 'TGJShopContext.Reviews'  is null.");
            }
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
          return _context.Reviews.Any(e => e.ReviewId == id);
        }
    }
}
