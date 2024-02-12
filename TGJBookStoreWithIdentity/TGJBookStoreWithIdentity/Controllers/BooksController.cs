using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stripe;
using TGJBookStoreWithIdentity.Models;
using X.PagedList;

namespace TGJBookStoreWithIdentity.Controllers
{
    public class BooksController : Controller
    {
        private readonly TGJShopContext _context;

        public BooksController(TGJShopContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(int? pageNumber, string sortOrder, string searchString, string currentFilter)
        {
                     
            var books = from b in _context.Books select b;
            //Coloumn Sorting code - GM 17/11/22
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParam"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["NameSortParam"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["PriceSortParam"] = sortOrder == "price_desc" ? "price_asc" : "price_desc";
            ViewData["GenreSortParam"] = sortOrder == "genre_asc" ? "genre_desc" : "genre_asc";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            
            
            //Search for book title code - GM 17/11/22
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(a => a.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(a => a.Title); break;
                case "name_asc":
                    books = books.OrderBy(a => a.Author); break;
                case "name_desc":
                    books = books.OrderByDescending(a => a.Author); break;
                case "price_asc":
                    books = books.OrderBy(a => a.Price); break;
                case "price_desc":
                    books = books.OrderByDescending(a => a.Price); break;
                case "genre_asc":
                    books = books.OrderBy(a => a.Genre); break;
                case "genre_desc":
                    books = books.OrderByDescending(a => a.Genre); break;
                default:
                    books = books.OrderBy(a => a.Title); break;
            }

            int pageSize = 6;

            return View(await PaginatedList<Books>.CreateAsync(books.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        //Auto-suggest code - GM 17/11/22
        public string IndexAJAX(string searchString)
        {
            string sql = "SELECT * FROM Books WHERE Title LIKE @p0";    
            string wrapString = "%" + searchString + "%";
            List<Books> books = _context.Books.FromSqlRaw(sql, wrapString).ToList();
            string jason = JsonConvert.SerializeObject(books);
            return jason;
        }


        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin,Auditor,Developer")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,Slug,Author,Price,Quantity,Description,Genre")] Books books)
        {
            if (ModelState.IsValid)
            {
                _context.Add(books);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(books);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }
            return View(books);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,Slug,Author,Price,Quantity,Description,Genre")] Books books)
        {
            if (id != books.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(books);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksExists(books.BookId))
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
            return View(books);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'TGJShopContext.Books'  is null.");
            }
            var books = await _context.Books.FindAsync(id);
            if (books != null)
            {
                _context.Books.Remove(books);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BooksExists(int id)
        {
          return _context.Books.Any(e => e.BookId == id);
        }
    }
}
