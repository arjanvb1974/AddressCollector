using System.Collections.Generic;
using AddressCollector.Models;

namespace AddressCollector.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Pie> PiesOfTheWeek { get; set; }
    }
}
