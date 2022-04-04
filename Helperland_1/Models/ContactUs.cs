using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Helperland_1.Models
{
    public partial class ContactUs
    {
        public int ContactUsId { get; set; }
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Subject { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Message { get; set; }
        public string UploadFileName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string FileName { get; set; }
    }
}
