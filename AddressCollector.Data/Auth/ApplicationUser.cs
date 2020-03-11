using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AddressCollector.Data.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string Naam { get; set; }
        public string OndernemerId { get; set; }
        public virtual ApplicationUser Ondernemer { get; set; }
        [NotMapped]
        public string Rol { get; set; }
    }
}