using System.Collections.Generic;
using AddressCollector.Data.Auth;
using AddressCollector.Data.Entities;

namespace AddressCollector.ViewModels
{
    public class PrintViewModel
    {
        public string KlantId { get; set; }
        
        public List<ApplicationUser> Klanten { get; set; }
        public int EnvelopeId { get; set; }
        
        public List<AddressViewModel> Adressen { get; set; }
        
    }
}
