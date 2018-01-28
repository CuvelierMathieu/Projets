using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_NORTHWND
{
    public partial class Order_Details
    {
        public Order_Details(int ProductID, short Quantity, decimal UnitPrice)
        {
            this.Discount = 0;
            this.ProductID = ProductID;
            this.Quantity = Quantity;
            this.UnitPrice = UnitPrice;
        }

        public Order_Details() { }
    }
}
