using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Helperland_1.Models
{
    public partial class UserAddress
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }

        
        public string AddressLine1 { get; set; }
        
        public string AddressLine2 { get; set; }
        
        public string City { get; set; }
        
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public string Mobile { get; set; }
        public string Email { get; set; }

        public virtual User User { get; set; }
    }
}
