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
                var item = new AdminMenuItem() {DisplayValue = "Beheer gebruikers", ActionValue = "UserManagement", ControllerValue = "Admin"};
                menuItems.Add(item);

                item = new AdminMenuItem() {DisplayValue = "Beheer enveloppen", ActionValue = "EnvelopeManagement", ControllerValue = "Envelope"};
                menuItems.Add(item);
            }
            
            if (User.IsInRole("Administrator"))
            {
                var item = new AdminMenuItem() {DisplayValue = "Beheer rollen", ActionValue = "RoleManagement", ControllerValue = "Admin"};
                menuItems.Add(item);
            }
            
            return View(menuItems);
        }
    }

    public class AdminMenuItem
    {
        public string DisplayValue { get; set; }
        public string ActionValue { get; set; }
        public string ControllerValue { get; set; } 
    }
}
