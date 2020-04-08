using System.Collections.Generic;
using System.Security.Claims;
using AddressCollector.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AddressCollector.Data.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        IEnumerable<Address> AllAddresses(ClaimsPrincipal user);

        PostalCode GetAddress(string zipcode, int? number);

        bool Update(Address entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityEntry<Address> Add(Address entity);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Save();

        Address GetAddressById(int id);

        IEnumerable<Address> GetAddressesByKlantId(string klantid);
    }
}
