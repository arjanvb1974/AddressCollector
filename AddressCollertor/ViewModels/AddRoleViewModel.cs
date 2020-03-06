using System.ComponentModel.DataAnnotations;

namespace AddressCollector.ViewModels
{
    public class AddRoleViewModel
    {
        [Required]
        [Display(Name = "Rolnaam")]
        public string RoleName { get; set; }
    }
}