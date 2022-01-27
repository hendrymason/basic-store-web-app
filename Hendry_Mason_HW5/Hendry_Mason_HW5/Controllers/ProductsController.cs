using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hendry_Mason_HW5.DAL;
using Hendry_Mason_HW5.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace Hendry_Mason_HW5.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(s => s.ProductSuppliers)
                .ThenInclude(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.AllSuppliers = GetAllSuppliers();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Name,Description,Price,ProductType")] Product product, int[] SelectedSuppliers)
        {
            if (ModelState.IsValid == false)
            {
                //re-populate the view bag with the suppliers
                ViewBag.AllSuppliers = GetAllSuppliers();
                //go back to the Create view to try again
                return View(product);
            }

            //if code gets to this point, we know the model is valid and
            //we can add the product to the database

            //add the product to the database and save changes
            _context.Add(product);
            await _context.SaveChangesAsync();

            foreach (int supplierID in SelectedSuppliers)
            {
                Supplier dbSupplier = _context.Suppliers.Find(supplierID);

                ProductSupplier ps = new ProductSupplier();

                ps.Product = product;

                ps.Supplier = dbSupplier;

                _context.ProductSuppliers.Add(ps);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", new string[] { "Please specify a product to edit" });
            }

            Product product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return View("Error", new string[] { "This Product was not found! " });
            }

            ViewBag.AllSuppliers = GetAllSuppliers(product.ProductID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductID,Name,Description,Price,ProductType")] Product product, int[] SelectedSuppliers)
        {
            if (id != product.ProductID)
            {
                return View("Error", new string[] { "Please try again!" });
            }

            if (ModelState.IsValid == false) //there is something wrong
            {
                ViewBag.AllSuppliers = GetAllSuppliers(product.ProductID);
                return View(product);
            }

            //if code gets this far, attempt to edit the product
            try
            {
                //Find the product to edit in the database and include relevant 
                //navigational properties
                Product dbProduct = _context.Products
                    .Include(ps => ps.ProductSuppliers)
                    .ThenInclude(ps => ps.Supplier)
                    .FirstOrDefault(p => p.ProductID == product.ProductID);


                //add the suppliers that aren't already there
                foreach (int supplierID in SelectedSuppliers)
                {
                    if (dbProduct.ProductSuppliers.Any(s => s.Supplier.SupplierID == supplierID) == false)
                    {
                        ProductSupplier ps = new ProductSupplier();
                        ps.Supplier = _context.Suppliers.Find(supplierID);
                        ps.Product = dbProduct;

                        _context.ProductSuppliers.Add(ps);
                        _context.SaveChanges();
                    }
                }

                //update the product's scalar properties
                dbProduct.Price = product.Price;
                dbProduct.Name = product.Name;
                dbProduct.Description = product.Description;

                //save changes
                _context.Products.Update(dbProduct);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return View("Error", new string[] { "There was an error editing this Product.", ex.Message });
            }

            //if code gets this far, everything is okay
            //send the user back to the page with all the products
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }

        //Generate a listbox of the Suppliers for the Product/Create Action Method
        private MultiSelectList GetAllSuppliers()
        {
            List<Supplier> allSuppliers = _context.Suppliers.ToList();

            Supplier SelectNone = new Supplier() { SupplierID = 0, Name = "All Suppliers" };
            allSuppliers.Add(SelectNone);

            MultiSelectList mslAllSuppliers = new MultiSelectList(allSuppliers.OrderBy(s => s.Name), "SupplierID", "Name");

            //return the MultiSelectList
            return mslAllSuppliers;
        }

        //Generate a listbox of the Suppliers for the Product/Create Action Method
        private MultiSelectList GetAllSuppliers(int productID)
        {
            //Get the list of suppliers from the database
            List<Supplier> allSuppliers = _context.Suppliers.ToList();

            List<ProductSupplier> productSuppliers = _context.ProductSuppliers
                                                 .Where(ps => ps.Product.ProductID == productID)
                                                 .ToList();

            List<Int32> selectedSupplierIDs = new List<Int32>();

            foreach (ProductSupplier ps in productSuppliers)
            {
                selectedSupplierIDs.Add(ps.Supplier.SupplierID);
            }

            MultiSelectList mslAllSuppliers = new MultiSelectList(allSuppliers.OrderBy(s => s.Name), "SupplierID", "Name", selectedSupplierIDs);

            //return the MultiSelectList
            return mslAllSuppliers;
        }
    }
}
