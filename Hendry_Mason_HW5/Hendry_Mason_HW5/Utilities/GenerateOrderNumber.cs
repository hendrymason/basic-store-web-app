using System;
using System.Linq;
using Hendry_Mason_HW5.DAL;

namespace Hendry_Mason_HW5.Utilities
{
    public static class GenerateOrderNumber
    {
        public static Int32 GetNextOrderNumber(AppDbContext _context)
        {
            //Set starting number for Order Numbers to begin at
            const Int32 START_NUMBER = 7001;

            Int32 intMaxOrderNumber; // the current maximum Order Number
            Int32 intNextOrderNumber; //the order number for the next order

            if (_context.Orders.Count() == 0) //there are no orders in the database yet
            {
                intMaxOrderNumber = START_NUMBER; //order numbers start at 7001
            }
            else
            {
                //this is the highest number in the database right now
                intMaxOrderNumber = _context.Orders.Max(c => c.OrderNumber); 
            }

            //You added courses before you realized that you needed this code
            //and now you have some course numbers less than 3000
            if (intMaxOrderNumber < START_NUMBER)
            {
                intMaxOrderNumber = START_NUMBER;
            }
            //add one to the current max to find the next one
            intNextOrderNumber = intMaxOrderNumber + 1;

            //return the value
            return intNextOrderNumber;
        }
    }
}
