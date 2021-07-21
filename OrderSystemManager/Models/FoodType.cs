using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OrderSystemManager.Models
{
    public partial class FoodType
    {
        public FoodType()
        {
            Foods = new HashSet<Food>();
        }

        public int FoodTypeId { get; set; }
        public string FoodTypeName { get; set; }

        [JsonIgnore]
        public virtual ICollection<Food> Foods { get; set; }
    }
}
