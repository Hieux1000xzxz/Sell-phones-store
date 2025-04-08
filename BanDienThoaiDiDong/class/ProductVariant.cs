using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDienThoaiDiDong
{

    public class ProductVariant
    {
        public int VarientID { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string ColorCode { get; set; }
        public bool IsSelected { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}