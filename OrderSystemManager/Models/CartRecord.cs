using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OrderSystemManager.Models
{
    public partial class CartRecord
    {
        public int CartId { get; set; }
        public int? UserId { get; set; }
        public int? FoodId { get; set; }
        public int? Count { get; set; }

        [JsonIgnore]
        public virtual Food Food { get; set; }
        public virtual User User { get; set; }
    }
}
