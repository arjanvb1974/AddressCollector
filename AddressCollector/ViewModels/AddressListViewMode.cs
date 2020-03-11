using System.Collections.Generic;
using AddressCollector.Models;

namespace AddressCollector.ViewModels
{
    public class AddressListViewModel
    {
        public AddressListViewModel()
        {
            Addresses = new List<AddressViewModel>();
        }
        public List<AddressViewModel> Addresses { get; set; }
    }
}
