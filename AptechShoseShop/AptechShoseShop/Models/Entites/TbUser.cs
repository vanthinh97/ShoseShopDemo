using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class TbUser
    {
        public TbUser()
        {
            Orders = new HashSet<Order>();
            Products = new HashSet<Product>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public bool? Gender { get; set; }
        public string NumberPhone { get; set; }

        ///[Index(IsUnique = true)]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Avatar { get; set; }

        public virtual StatusUser Status { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}