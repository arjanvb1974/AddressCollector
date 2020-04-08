using System.Collections.Generic;

namespace AddressCollector.ViewModels
{
    public class AddressListViewModel
    {
        public AddressListViewModel()
        {
            Addresses = new List<AddressViewModel>();
        }
        public List<AddressViewModel> Addresses { get; set; }
        public bool FromPrint { get; set; }
        public int EnvelopeId { get; set; }
    }
}
