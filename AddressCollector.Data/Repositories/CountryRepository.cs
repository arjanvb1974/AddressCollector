using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AddressCollector.Data.DataContext;
using AddressCollector.Data.Entities;
using AddressCollector.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AddressCollector.Data.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _data;

        public CountryRepository(IMemoryCache cache, AppDbContext data)
        {
            _cache = cache;
            _data = data;
        }
        

        public IEnumerable<Country> Countries
        {
            get
            {
                return _cache.GetOrCreate("data_Countries", x => _data.Country.OrderBy(c => c.CountryName).ToList());
            }
        }
        
        public Country GetByCode(string code)
        {
            return Countries.SingleOrDefault(x => x.Code != null && string.Equals(x.Code, code, StringComparison.CurrentCultureIgnoreCase));
        }

        public Country GetById(int id)
        {
            return Countries.SingleOrDefault(x => x.Id == id);
        }
    }
}
