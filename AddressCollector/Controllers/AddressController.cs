using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AddressCollector.Data.Auth;
using AddressCollector.Data.Entities;
using AddressCollector.Data.Repositories.Interfaces;
using AddressCollector.Models;
using AddressCollector.Shared;
using AddressCollector.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace AddressCollector.Controllers
{
    
    public class AddressController : Controller
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public AddressController(IAddressRepository addressRepository, ICountryRepository countryRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _addressRepository = addressRepository;
            _countryRepository = countryRepository;
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
                model.LandId = Constants.DefaultCountryNlId;
            }

            ViewBag.Landen = _countryRepository.Countries
                .OrderBy(c => c.CountryName)
                .ToList()
                .Select(c => new SelectListItem()
                {
                    Text = c.CountryName,
                    Value = c.Id.ToString()
                })
                .ToList();

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
            model.Addresses = _mapper.Map<List<AddressViewModel>>(_addressRepository.AllAddresses(User).ToList());

            if (!User.IsInRole("Klant"))
            {
                foreach (var address in model.Addresses)
                {
                    //address.Klant = _mapper.Map<ApplicationUser>(_countryRepository.GetById(address.LandId));
                }
            }

            foreach (var address in model.Addresses)
            {
                address.Land = _mapper.Map<CountryViewModel>(_countryRepository.GetById(address.LandId));
            }
            return View(model);
        }

        [Authorize(Roles="Administrator, Ondernemer, Klant")]
        public IActionResult AddressManagementPerKlant(string klantId, int envelopeId, bool fromPrint = false)
        {
            var model = new AddressListViewModel();
            model.FromPrint = fromPrint;
            model.EnvelopeId = envelopeId;

            model.Addresses = new List<AddressViewModel>();
            model.Addresses = _mapper.Map<List<AddressViewModel>>(_addressRepository.GetAddressesByKlantId(klantId).ToList());

            foreach (var address in model.Addresses)
            {
                address.Land = _mapper.Map<CountryViewModel>(_countryRepository.GetById(address.LandId));
            }
            return View("AddressManagement", model);
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
            
            ViewBag.Landen = _countryRepository.Countries
                .OrderBy(c => c.CountryName)
                .ToList()
                .Select(c => new SelectListItem()
                {
                    Text = c.CountryName,
                    Value = c.Id.ToString()
                })
                .ToList();

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

        
        public IActionResult PrintOrder()
        {
            try
            {
                // Create new PDF document
                PdfDocument document = new PdfDocument();
                var time = document.Info.CreationDate;
                document.Info.Title = "PDFsharp Clock Demo";
                document.Info.Author = "Arjan van Bavelgem";
                document.Info.Subject = "Server time: " + time.ToString("F", CultureInfo.InvariantCulture);

                for (int i = 0; i < 1; i++)
                {
                    // Create new page
                    PdfPage page = document.AddPage();
                    page.Size = PageSize.A5;
                    page.Orientation = PageOrientation.Landscape;
                    page.Width = XUnit.FromMillimeter(170);
                    page.Height = XUnit.FromMillimeter(170);
                
                    // Create graphics object and draw clock
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    gfx.RotateAtTransform(270,new XPoint(170,270));
                    // Create a font
                    XFont font = new XFont("Times New Roman", 16, XFontStyle.Regular);

                    //get address for test
                    var address = _addressRepository.GetAddressById(i + 6);
                    var naam = string.Concat(address.Voornaam, address.Tussenvoegsel != null ? " " + address.Tussenvoegsel : " ", address.Achternaan); 
                    // Draw the text
                    gfx.DrawString(string.Concat(address.Voornaam, address.Tussenvoegsel != null ? " " + address.Tussenvoegsel  + " " : " ", address.Achternaan), font, XBrushes.Black, new XRect(100, 40, page.Width, page.Height), XStringFormats.CenterLeft);
                    gfx.DrawString(string.Concat(address.Straat, " ", address.Huisnummer, " ", address.HuisnummerToevoeging), font, XBrushes.Black, new XRect(100, 60, page.Width, page.Height), XStringFormats.CenterLeft);
                    gfx.DrawString(string.Concat(address.Postcode, "  ", address.Plaats), font, XBrushes.Black, new XRect(100, 80, page.Width, page.Height), XStringFormats.CenterLeft);
                    if (address.LandId != Constants.DefaultCountryNlId)
                    {
                        gfx.DrawString(string.Concat(_countryRepository.GetById(address.LandId)), font, XBrushes.Black, new XRect(100, 100, page.Width, page.Height), XStringFormats.CenterLeft);
                    }

                }

                
                //voor vierkante enveloppen begin

                //// Create new page
                //PdfPage page = document.AddPage();
                //page.Size = PageSize.A5;
                //page.Width = XUnit.FromMillimeter(170);
                //page.Height = XUnit.FromMillimeter(170);
                
                //// Create graphics object and draw clock
                //XGraphics gfx = XGraphics.FromPdfPage(page);

                //// Create a font
                //XFont font = new XFont("Times New Roman", 16, XFontStyle.Regular);

                ////get address for test
                //var address = _addressRepository.GetAddressById(i + 6);
                //var naam = string.Concat(address.Voornaam, address.Tussenvoegsel != null ? " " + address.Tussenvoegsel : " ", address.Achternaan); 
                //// Draw the text
                //gfx.DrawString(string.Concat(address.Voornaam, address.Tussenvoegsel != null ? " " + address.Tussenvoegsel  + " " : " ", address.Achternaan), font, XBrushes.Black, new XRect(150, 0, page.Width, page.Height), XStringFormats.CenterLeft);
                //gfx.DrawString(string.Concat(address.Straat, " ", address.Huisnummer, " ", address.HuisnummerToevoeging), font, XBrushes.Black, new XRect(150, 20, page.Width, page.Height), XStringFormats.CenterLeft);
                //gfx.DrawString(string.Concat(address.Postcode, "  ", address.Plaats), font, XBrushes.Black, new XRect(150, 40, page.Width, page.Height), XStringFormats.CenterLeft);
                //if (address.LandId != Constants.DefaultCountryNlId)
                //{
                //    gfx.DrawString(string.Concat(_countryRepository.GetById(address.LandId)), font, XBrushes.Black, new XRect(150, 60, page.Width, page.Height), XStringFormats.CenterLeft);
                //}

                //voor vierkante enveloppen eind
            
                // Send PDF to browser
                MemoryStream stream = new MemoryStream();

                
                
                document.Save(stream, false);
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.Headers.Add("content-length", stream.Length.ToString());
                
                return File(stream, "application/pdf", "test.pdf");
                
                //byte[] image = Convert.FromBase64String(_base64Image2);
                //Response.Body.WriteAsync(stream.ToArray());
                ////Response.BinaryWrite(stream.ToArray());
                //Response.Body.Flush();
                //stream.Close();
                ////Response.End();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;
        }

        

        

        
    }
}
