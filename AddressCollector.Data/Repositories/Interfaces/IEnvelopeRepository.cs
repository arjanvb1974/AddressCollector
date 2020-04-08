using System.Collections.Generic;
using System.Security.Claims;
using AddressCollector.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AddressCollector.Data.Repositories.Interfaces
{
    public interface IEnvelopeRepository
    {
        IEnumerable<Envelope> AllEnvelopes(ClaimsPrincipal user);
        
        bool Update(Envelope entity);

        bool Delete(Envelope entity);
        
        EntityEntry<Envelope> Add(Envelope entity);
        
        bool Save();

        Envelope GetEnvelopeById(int id);

        IEnumerable<Envelope> GetEnvelopesByOndernemerId(string klantid);
    }
}
