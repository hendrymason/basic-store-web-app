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
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public IActionResult Index()
        {
            List<Order> Orders = new List<Order>();

            if (User.IsInRole("Admin"))
            {
                Orders = _context.Orders.Include(o => o.OrderDetails).ToList();
            }

            else //user is a customer
            {
                Orders = _context.Orders.Where(o => o.AppUser.UserName == User.Identity.Name).Include(ord => ord.OrderDetails).ToList();
            }

            return View(Orders);
        }

        // GET: Orders/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order order = _context.Orders
                .Include(od => od.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(od => od.AppUser)
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            //make sure a customer isn't trying to look at someone else's order
            if (User.IsInRole("Admin") == false && order.AppUser.UserName != User.Identity.Name)
            {
                return View("Error", new string[] { "You are not authorized to view the details of this order!" });
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "Customer")]
        public IActionResult Create() 
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> Create([Bind("OrderID,OrderNumber,OrderDate,OrderNotes")] Order order)
        {
            //Set the Value for the OrderNumber
            order.OrderNumber = Utilities.GenerateOrderNumber.GetNextOrderNumber(_context);

            //Set Order Date
            order.OrderDate = DateTime.Now;

            //Associate the Order with the logged-in customer
            order.AppUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid == false)
            {
                return View(order);
            }
            _context.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Create", "OrderDetails", new { OrderID = order.OrderID });
        }

        // GET: Orders/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order order = _context.Orders
                .Include(ord => ord.OrderDetails)
                .ThenInclude(ord => ord.Product)
                .Include(ord => ord.AppUser)
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            //make sure a customer isn't trying to look at someone else's order
            if (User.IsInRole("Admin") == false && order.AppUser.UserName != User.Identity.Name)
            {
                return View("Error", new string[] { "You are not authorized to edit this order!" });
            }

            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,OrderNotes")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
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
            return View(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
