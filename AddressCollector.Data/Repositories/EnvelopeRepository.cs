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
    public class EnvelopeRepository : IEnvelopeRepository
    {
        private readonly AppDbContext _appDbContext;
        public EnvelopeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Envelope> AllEnvelopes(ClaimsPrincipal user)
        {
            if (user.IsInRole(Constants.AdminUserRole))
            {
                return _appDbContext.Envelope;
            }
            else if (user.IsInRole(Constants.OndernemerUserRole))
            {
                return _appDbContext.Envelope.Where(x => x.OnderNemerId == user.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            else
            {
                return null;
            }
        }

        public Envelope GetEnvelopeById(int id)
        {
            return _appDbContext.Envelope.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Envelope> GetEnvelopesByOndernemerId(string ondernemerId)
        {
            return _appDbContext.Envelope.Where(x => x.OnderNemerId == ondernemerId);
        }


        public bool Delete(Envelope entity)
        {
            _appDbContext.Remove(entity);
            _appDbContext.SaveChanges();
            return true;
        }

        public bool Save()
        {
            return _appDbContext.SaveChanges() > 0;
        }

        public bool Update(Envelope entity)
        {
            _appDbContext.Update(entity);
            return _appDbContext.SaveChanges() > 0;
        }

        public EntityEntry<Envelope> Add(Envelope entity)
        {
            return _appDbContext.Envelope.Add(entity);
        }
    }
}
