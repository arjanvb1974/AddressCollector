using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using AddressCollector.Data.Auth;
using AddressCollector.Data.DataContext;
using AddressCollector.Data.Entities;
using AddressCollector.Data.Repositories.Interfaces;
using AddressCollector.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AddressCollector.Data.Repositories
{
    public class AddressRepository: IAddressRepository
    {
        private readonly AppDbContext _appDbContext;
        public AddressRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Address> AllAddresses(ClaimsPrincipal user)
        {

            if (user.IsInRole(Constants.AdminUserRole))
            {
                return _appDbContext.Address;
            }
            else if (user.IsInRole(Constants.OndernemerUserRole))
            {
               return _appDbContext.Address.Where(x => x.OnderNemerId == user.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            else
            {
                return _appDbContext.Address.Where(x => x.KlantId == user.FindFirstValue(ClaimTypes.NameIdentifier) );
            }
        }

        public Address GetAddressById(int id)
        {
            return _appDbContext.Address.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Address> GetAddressesByKlantId(string klantid)
        {
            return _appDbContext.Address.Where(x => x.KlantId == klantid);
        }

       public PostalCode GetAddress(string zipcode, int? number)
       {
           return _appDbContext.Set<PostalCode>().FromSqlRaw("dbo.GetAddress @Zipcode = {0}, @Number = {1}", zipcode, number).ToList().FirstOrDefault();
       }

       public EntityEntry<Address> Add(Address entity)
       {
           return _appDbContext.Address.Add(entity);
       }

       public bool Update(Address entity)
       {
           _appDbContext.Update(entity);
           return _appDbContext.SaveChanges() > 0;
       }

       public bool Delete(Address entity)
       {
           //entity.IsDeleted = true;

           //todo: complement
           //remove username and email address, because these are unique
           //foreach (var user in entity.Users)
           //{
           //    user.EmailAddress = "";
           //    user.UserName = "";
           //}

           _appDbContext.SaveChanges();
           //todo: deletion introduces a bug where parent relation is reset.
           //_visData.Company.Remove(entity);
           return true;
       }

       public bool Save()
       {
           return _appDbContext.SaveChanges() > 0;
       }

        //public bool Update(Address entity)
        //{
        //    return Save();
        //}
    }
}
