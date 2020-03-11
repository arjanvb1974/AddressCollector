using System.Collections.Generic;
using System.Linq;
using AddressCollector.Data.Auth;
using AddressCollector.Data.Entities;
using AddressCollector.Data.Repositories.Interfaces;
using AddressCollector.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace AddressCollector.Controllers
{
    
    public class AddressController : Controller
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public AddressController(IAddressRepository addressRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        
        [Authorize(Roles="Administrator, Ondernemer, Klant")]
        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = new AddressListViewModel();

            var addresses = _addressRepository.AllAddresses(User).ToList();

            foreach (var address in addresses)
            {
                model.Addresses.Add(_mapper.Map<AddressViewModel>(address));
            }
            
            return View();
        }

        [Authorize(Roles="Administrator, Ondernemer, Klant")]
        public IActionResult AddNewAddress()
        {
            var model = new AddressViewModel();
            //set klant and ondernemerId
            var user = _userManager.FindByNameAsync(User.Identity.Name);
            if (user.Result != null)
            {
                model.KlantId = user.Result.Id;
                model.OnderNemerId = user.Result.OndernemerId;
            }

            return View(model);
        }

        [Authorize(Roles="Administrator, Ondernemer, Klant")]
        [HttpPost]
        public IActionResult AddNewAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var address = _mapper.Map<Address>(model);
            _addressRepository.Add(address);
            _addressRepository.Save();

            return RedirectToAction(model.Next ? "AddNewAddress" : "AddressManagement");
        }

        
        [Authorize(Roles="Administrator, Ondernemer, Klant")]
        public IActionResult AddressManagement()
        {
            var model = new AddressListViewModel();
            model.Addresses = new List<AddressViewModel>();

            var addresses = _addressRepository.AllAddresses(User).ToList();

            foreach (var address in addresses)
            {
                model.Addresses.Add(_mapper.Map<AddressViewModel>(address));
            }
            
            return View(model);
        }

        [Authorize(Roles="Administrator, Ondernemer, Klant")]
        public IActionResult DeleteAddress(int id)
        {
           //doe hier de delete
            return RedirectToAction("AddressManagement");
        }

        [Authorize(Roles="Administrator, Ondernemer, Klant")]
        public IActionResult EditAddress(int id)
        {
            var address = _addressRepository.GetAddressById(id);

            var model = _mapper.Map<AddressViewModel>(address);
            
            return View(model);
        }

        [Authorize(Roles="Administrator, Ondernemer, Klant")]
        [HttpPost]
        public IActionResult EditAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var address = _mapper.Map<Address>(model);
            _addressRepository.Update(address);
            _addressRepository.Save();

            return RedirectToAction(model.Next ? "AddNewAddress" : "AddressManagement");
        }

        public IActionResult GetOfficialAddress(string zipcode, string number)
        {
            var nr = 0;
            int.TryParse(number, out nr);
            var address = _addressRepository.GetAddress(zipcode, nr);
            return address == null ? Json(new { city = "", street = "" }) : Json(new { city = address.City, street = address.Street });
        }

        [Authorize(Roles="Administrator, Ondernemer, Klant")]
        [HttpPost]
        public IActionResult FirstAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
                
            return View(model);
        }

        [Authorize(Roles="Administrator, Ondernemer, Klant")]
        [HttpPost]
        public IActionResult PreviousAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
                
            return View(model);
        }

        [Authorize(Roles="Administrator, Ondernemer, Klant")]
        [HttpPost]
        public IActionResult NextAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
                
            return View(model);
        }

        [Authorize(Roles="Administrator, Ondernemer, Klant")]
        [HttpPost]
        public IActionResult LastAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
                
            return View(model);
        }

    }
}
