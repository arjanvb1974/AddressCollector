using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AddressCollector.Models
{
    public class AddressRepository: IAddressRepository
    {
        private readonly AppDbContext _appDbContext;

        public AddressRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

       public IEnumerable<Address> AllAddresses => _appDbContext.Address;

       public PostalCode GetAddress(string zipcode, int? number)
       {
           return _appDbContext.Set<PostalCode>().FromSqlRaw("dbo.GetAddress @Zipcode = {0}, @Number = {1}", zipcode, number).ToList().FirstOrDefault();
       }
    }
}
