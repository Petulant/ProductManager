using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Models;
using ProductManager.Infrastructure.Data;

namespace ProductManager.Web.Controllers
{
    public class ProductController(ProductManagerContext context) : Controller
    {
        private readonly ProductManagerContext _context = context;

        // =======================================================================GET: Products=================================================
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5; // How many items per page

            var query = _context.Products.Include(p => p.Category).AsQueryable();

            var totalItems = await query.CountAsync();
            var products = await query
                .OrderByDescending(p => p.CreatedDate) // Show newest first
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Pass paging info to the View using ViewBag
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(products);
        }


        //========================================================== ADD Product=================================================================
        public IActionResult Create() // GET: This pulls the existing categories from your DB for the dropdown
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)  // POST This receives the data when the user clicks "Save"
        {
            ModelState.Remove("Category");
            if (ModelState.IsValid)
            {
                product.CreatedDate = DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            // If we reach here, something was wrong with the data, show the form again
            return View(product);
        }

        //================================================================ Edit Product============================================================================
        public async Task<IActionResult> Edit(long? id)   // GET
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            // Load categories for the dropdown so the user can change the category
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Product product)  //POST 
        {
            if (id != product.ProductId) return NotFound();

            // Ignore the full Category object validation like we did in Create
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Product updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.ProductId == product.ProductId))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, reload the dropdown
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }


        //====================================Delete Product================================================================================================
        public async Task<IActionResult> Delete(int? id) // GET : This shows the confirmation page
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]//  POST : This actually removes the record
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        //=================================================================================================================================================

    }
}