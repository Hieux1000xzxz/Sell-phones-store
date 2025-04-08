using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDienThoaiDiDong
{

    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Variant { get; set; }
        public int Quantity { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public int? VariantId { get; set; }
    }
}
