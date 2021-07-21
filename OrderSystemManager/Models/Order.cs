using System;
using System.Collections.Generic;

#nullable disable

namespace OrderSystemManager.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public string RealName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public decimal? Totle { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? OrderState { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
