using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDienThoaiDiDong
{

    public class OrderItem
    {
        public int OrderID { get; set; }
        public int OrderDetailID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int? VariantID { get; set; }
    }
}