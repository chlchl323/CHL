using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace OrderSystemManager.Models
{
    public partial class UserType
    {
        public UserType()
        {
            Users = new HashSet<User>();
        }

        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}
