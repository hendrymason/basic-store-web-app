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
    public class SuppliersController : Controller
    {
        private readonly AppDbContext _context;

        public SuppliersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Suppliers.ToListAsync());
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error", new String[] { "Please specify a Supplier to view!" });
            }

            var supplier = await _context.Suppliers
                .Include(p => p.ProductSuppliers)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(s => s.SupplierID == id);

            if (supplier == null)
            {
                return View("Error", new String[] { "This Supplier was not found!" });
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierID,Name,EmailAddress,PhoneNumber")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", new String[] { "Please specify a Supplier to edit!" });
            }

            //find supplier in the database
            Supplier supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
               return View("Error", new String[] { "This Supplier does not exist in the database!" });
            }

            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierID,Name,EmailAddress,PhoneNumber")] Supplier supplier)
        {
            if (id != supplier.SupplierID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.SupplierID))
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
            return View(supplier);
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.SupplierID == id);
        }
    }
}
