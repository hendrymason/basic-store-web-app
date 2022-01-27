using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Hendry_Mason_HW5.Models
{
    public class Supplier
    {
        [Key]
        [Display(Name = "Supplier ID:")]
        public Int32 SupplierID { get; set; }

        [Display(Name = "Supplier Name:")]
        public string Name { get; set; }

        [Display(Name = "Email Address:")]
        public string EmailAddress { get; set; }

        [Display(Name = "Phone Number:")]
        public string PhoneNumber { get; set; }

        public List<ProductSupplier> ProductSuppliers { get; set; }

        public Supplier()
        {
            if (ProductSuppliers == null)
            {
                ProductSuppliers = new List<ProductSupplier>();
            }
        }
    }
}
