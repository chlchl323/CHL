using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
#nullable disable

namespace LJXC.Models
{
    public partial class Food
    {
        public Food()
        {
            CartRecords = new HashSet<CartRecord>();
        }

        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int? FoodTypeId { get; set; }
        public decimal? Price { get; set; }
        public string PicUrl { get; set; }
        public int? SalesVolume { get; set; }
        public DateTime? AddDate { get; set; }
        public string Desc { get; set; }
        
        public virtual FoodType FoodType { get; set; }
        [JsonIgnore]
        public virtual ICollection<CartRecord> CartRecords { get; set; }
    }
}
