using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AddressCollector.ViewModels
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Vult u a.u.b. een rolnaam in")]
        [Display(Name = "Rolnaam")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }

    }
}