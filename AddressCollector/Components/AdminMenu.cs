using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AddressCollector.Components
{
    public class AdminMenu : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var menuItems = new List<AdminMenuItem>();

            if (User.IsInRole("Administrator") || User.IsInRole("Ondernemer"))
            {
                var item = new AdminMenuItem() {DisplayValue = "Beheer gebruikers", ActionValue = "UserManagement"};
                menuItems.Add(item);
            }
            
            if (User.IsInRole("Administrator"))
            {
                var item = new AdminMenuItem() {DisplayValue = "Beheer rollen", ActionValue = "RoleManagement"};
                menuItems.Add(item);
            }
            
            return View(menuItems);
        }
    }

    public class AdminMenuItem
    {
        public string DisplayValue { get; set; }
        public string ActionValue { get; set; }
    }
}
