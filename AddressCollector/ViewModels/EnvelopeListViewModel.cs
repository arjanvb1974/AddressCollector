using System.Collections.Generic;

namespace AddressCollector.ViewModels
{
    public class EnvelopeListViewModel
    {
        public EnvelopeListViewModel()
        {
            Envelopes = new List<EnvelopeViewModel>();
        }
        public List<EnvelopeViewModel> Envelopes { get; set; }
        
    }
}
