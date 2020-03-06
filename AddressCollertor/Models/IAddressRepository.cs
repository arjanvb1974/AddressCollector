using System.Collections.Generic;

namespace AddressCollector.Models
{
    public interface IAddressRepository
    {
        IEnumerable<Address> AllAddresses { get; }

        PostalCode GetAddress(string zipcode, int? number);
    }
}
