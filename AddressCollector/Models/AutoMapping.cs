using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressCollector.Data.Entities;
using AddressCollector.ViewModels;
using AutoMapper;

namespace AddressCollector.Models
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //db --> web
            CreateMap<Address, AddressViewModel>();
            CreateMap<Country, CountryViewModel>();
            CreateMap<Envelope, EnvelopeViewModel>();

            //web --> db
            CreateMap<AddressViewModel, Address>();
            CreateMap<CountryViewModel, Country>();
            CreateMap<EnvelopeViewModel, Envelope>();
        }
    }
}
