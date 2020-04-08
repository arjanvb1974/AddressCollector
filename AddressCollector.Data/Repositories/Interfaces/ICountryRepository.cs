using System;
using System.Collections.Generic;
using System.Text;
using AddressCollector.Data.Entities;

namespace AddressCollector.Data.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        IEnumerable<Country> Countries { get; }

        Country GetByCode(string code);

        Country GetById(int id);
    }
}
