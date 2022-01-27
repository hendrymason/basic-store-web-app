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
    public class OrderDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public OrderDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderDetails.ToListAsync());
        }


        // GET: OrderDetails/Create
        public IActionResult Create(int OrderID)
        {
            //create a new instance of the orderDetail class
            OrderDetail od = new OrderDetail();

            //find the order that is associated with this ID
            Order dbOrder = _context.Orders.Find(OrderID);

            //set the order details order equal to the one just found
            od.Order = dbOrder;

            //populate the viewbag with all of the orders
            ViewBag.AllProducts = GetAllProducts();


            return View(od);
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind list carries editable properties 
        public async Task<IActionResult> Create([Bind("Order,OrderDetailID,QuantityofProduct")] OrderDetail orderDetail, int SelectedProduct)
        {
            if (ModelState.IsValid == false) //repopulate viewbag and send back orderDetail object
            {
                ViewBag.AllProducts = GetAllProducts();
                return View(orderDetail);
            }

            //find product in database associated with SelectedProduct parameter
            Product dbProduct = _context.Products.Find(SelectedProduct);

            //set orderDetail product property to equal that of the located Product^
            orderDetail.Product = dbProduct;

            //find the order associated with the order detail parameter
            Order dbOrder = _context.Orders.Find(orderDetail.Order.OrderID);

            //set the orderDetail's property equal to the order just found
            orderDetail.Order = dbOrder;

            //set the orderDetail's price equal to the selected product's price
            orderDetail.ProductPrice = dbProduct.Price;

            //calculate and set the extended price for the order detail
            orderDetail.ExtendedPrice = orderDetail.QuantityofProduct * orderDetail.ProductPrice;

            //add the order detail to the database
            _context.Add(orderDetail);
            await _context.SaveChangesAsync();

            //redirect user to the details view for the order
            return RedirectToAction("Details","Orders", new { id = orderDetail.Order.OrderID });
        }

        // GET: OrderDetails/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order order = _context.Orders
                            .Include(o => o.OrderDetails)
                            .ThenInclude(o => o.Product)
                            .Include(o => o.AppUser)
                            .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return View("Error", new String[] { "This order was not found in the database!" });
            }

            return View(order);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderDetailID,QuantityofProduct,ProductPrice,ExtendedPrice")] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailID)
            {
                return NotFound();
            }

            if(ModelState.IsValid == false)
            {
                return View(orderDetail);
            }

            OrderDetail dbOD;

            try
            {
                dbOD = _context.OrderDetails
                      .Include(od => od.Product)
                      .Include(od => od.Order)
                      .FirstOrDefault(od => od.OrderDetailID == orderDetail.OrderDetailID);

                //recalculate and set the order detail's price properties
                dbOD.QuantityofProduct = orderDetail.QuantityofProduct;
                dbOD.ProductPrice = dbOD.Product.Price;
                dbOD.ExtendedPrice = dbOD.QuantityofProduct * dbOD.ProductPrice;

                _context.Update(dbOD);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return View("Error", new String[] { "There was a problem editing this record", ex.Message });
            }

            //Redirect to the order when the user has finished editing
            return RedirectToAction("Details", "Orders", new { id = dbOD.Order.OrderID });
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(m => m.OrderDetailID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            OrderDetail orderDetail = await _context.OrderDetails
                                                   .Include(o => o.Order)
                                                   .FirstOrDefaultAsync(o => o.OrderDetailID == id);

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Orders", new { id = orderDetail.Order.OrderID });
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderDetailID == id);
        }

        private SelectList GetAllProducts()
        {
            List<Product> allProducts = _context.Products.ToList();

            SelectList slAllProducts = new SelectList(allProducts, nameof(Product.ProductID), nameof(Product.Name));

            return slAllProducts;
        }
    }
}
