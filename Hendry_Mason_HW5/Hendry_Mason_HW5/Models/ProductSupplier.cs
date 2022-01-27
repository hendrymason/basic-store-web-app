using System;

namespace Hendry_Mason_HW5.Models
{
    public class ProductSupplier
    {
        public Int32 ProductSupplierID { get; set; }

        public Product Product { get; set; }
        public Supplier Supplier { get; set; }
    }
}
