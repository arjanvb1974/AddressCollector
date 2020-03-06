using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AddressCollector.Components
{
    public class AddressMenu : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var menuItems = new List<AddressMenuItem> { new AddressMenuItem()
                {
                    DisplayValue = "Voer nieuwe adressen op",
                    ActionValue = "AddNewAddress"

                },
                new AddressMenuItem()
                {
                    DisplayValue = "Beheer adressen",
                    ActionValue = "AddressManagement"
                }};

            return View(menuItems);
        }
    }

    public class AddressMenuItem
    {
        public string DisplayValue { get; set; }
        public string ActionValue { get; set; }
    }
}
