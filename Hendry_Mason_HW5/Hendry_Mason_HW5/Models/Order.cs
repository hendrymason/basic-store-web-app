using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hendry_Mason_HW5.Models
{
    public class Order
    {
        public const Decimal TAX_RATE = 0.0825m;

        [Key]
        [Display(Name = "Order ID:")]
        public Int32 OrderID { get; set; }

        [Display(Name = "Order Number:")]
        public Int32 OrderNumber { get; set; }

        [Display(Name = "Order Date:")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Order Notes:")]
        public string OrderNotes { get; set; }

        [Display(Name = "Order Subtotal:")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal OrderSubtotal
        {
            get { return OrderDetails.Sum(od => od.ExtendedPrice); }
        }

        [Display(Name = "Sales Tax:")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal SalesTax
        {
            get { return OrderSubtotal * TAX_RATE; }
        }

        [Display(Name = "Order Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal OrderTotal
        {
            get { return OrderSubtotal + SalesTax; }
        }

        public AppUser AppUser { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            if (OrderDetails == null)
            {
                OrderDetails = new List<OrderDetail>();
            }
        }
    }
}
