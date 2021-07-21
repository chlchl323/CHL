using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OrderSystemManager.Models
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int? OrderId { get; set; }
        public int? FoodId { get; set; }
        public string FoodName { get; set; }
        public int? Quatity { get; set; }
        public decimal? UnitPrice { get; set; }
        public string PicUrl { get; set; }
        
        [JsonIgnore]
        public virtual Order Order { get; set; }
    }
}
