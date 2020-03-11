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
            CreateMap<Address, AddressViewModel>();


            CreateMap<AddressViewModel, Address>();
        }
    }
}
