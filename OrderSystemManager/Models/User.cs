using System;
using System.Collections.Generic;

#nullable disable

namespace OrderSystemManager.Models
{
    public partial class User
    {
        public User()
        {
            CartRecords = new HashSet<CartRecord>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? UserTypeId { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }
        public string RealName { get; set; }
        public string Phone { get; set; }

        public virtual UserType UserType { get; set; }
        public virtual ICollection<CartRecord> CartRecords { get; set; }
    }
}
