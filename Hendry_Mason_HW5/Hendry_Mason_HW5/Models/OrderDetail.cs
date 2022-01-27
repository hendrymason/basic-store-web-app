using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Hendry_Mason_HW5.Models
{
    public class OrderDetail
    {
        public Int32 OrderDetailID { get; set; }

        [Required(ErrorMessage = "You must provide a specific quantity to order.")]
        [Display(Name = "Quantity:")]
        [Range(1, 1000, ErrorMessage = "Product Quantity must be in between 1 and 1000.")]
        public Int32 QuantityofProduct { get; set; }

        [Display(Name = "Price:")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal ProductPrice { get; set; }

        [Display(Name = "Subtotal:")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal ExtendedPrice { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
