using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDienThoaiDiDong

{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public string ImageUrl { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercent
        {
            get { return (SalePrice / OriginalPrice) * 100; }
        }
        public List<ProductImage> Images { get; set; }
        public List<ProductVariant> StorageOptions { get; set; }
        public List<ProductVariant> ColorOptions { get; set; }
        public List<ProductSpec> Specifications { get; set; }
    }

}