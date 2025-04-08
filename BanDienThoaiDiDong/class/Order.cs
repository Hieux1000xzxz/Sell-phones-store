using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDienThoaiDiDong
{

    public class Order
    {
        public int OrderID { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string PaymentMethod { get; set; }
        public int UserId { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}