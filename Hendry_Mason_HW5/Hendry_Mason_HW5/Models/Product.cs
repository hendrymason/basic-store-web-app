using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Hendry_Mason_HW5.Models
{
    public enum ProductType
    {
        [Display(Name = "Candy")] Candy,
        [Display(Name = "Drink")] Drink,
        [Display(Name = "Popcorn")] Popcorn,
        [Display(Name = "Prepared Food")] PreparedFood,
        [Display(Name = "Other")] Other
    }

    public class Product
    {
        [Key]
        [Display(Name = "Product ID:")]
        public Int32 ProductID { get; set; }

        [Display(Name = "Product Name:")]
        [Required(ErrorMessage = "The Product Name is required.")]
        public string Name { get; set; }

        [Display(Name = "Description:")]
        public string Description { get; set; }

        [Display(Name = "Price:")]
        [Required(ErrorMessage = "The Price is required.")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal Price { get; set; }

        [Display(Name = "Product Type:")]
        public ProductType ProductType { get; set; }

        public List<ProductSupplier> ProductSuppliers { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        public Product()
        {
            if (ProductSuppliers == null)
            {
                ProductSuppliers = new List<ProductSupplier>();
            }

            if (OrderDetails == null)
            {
                OrderDetails = new List<OrderDetail>();
            }

        }
    }
}