using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using AddressCollector.Data.Auth;
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

namespace AddressCollector.Controllers
{
    
    public class PrintController : Controller
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEnvelopeRepository _envelopeRepository;
        private PdfDocument _document;
        
        public PrintController(IAddressRepository addressRepository, 
                               ICountryRepository countryRepository,
                               IEnvelopeRepository envelopeRepository,
                               IMapper mapper, 
                               UserManager<ApplicationUser> userManager)
        {
            _addressRepository = addressRepository;
            _countryRepository = countryRepository;
            _envelopeRepository = envelopeRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        
        [Authorize(Roles="Administrator, Ondernemer")]
        // GET: /<controller>/
        public IActionResult Index(string klantId)
        {
            var model = new PrintViewModel();
            model.Adressen = new List<AddressViewModel>();

            model.Klanten = User.IsInRole("Administrator") ? _userManager.Users.ToList() : _userManager.Users.Where(x => x.OndernemerId == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();

            ViewBag.Envelopes = _envelopeRepository.GetEnvelopesByOndernemerId(User.FindFirstValue(ClaimTypes.NameIdentifier))
                .OrderBy(c => c.Naam)
                .ToList()
                .Select(c => new SelectListItem()
                {
                    Text = c.Naam,
                    Value = c.Id.ToString()
                })
                .ToList();

            if (!string.IsNullOrEmpty(klantId))
            {
                model.Adressen = _mapper.Map<List<AddressViewModel>>(_addressRepository.GetAddressesByKlantId(klantId));
                foreach (var address in model.Adressen)
                {
                    address.Land = _mapper.Map<CountryViewModel>(_countryRepository.GetById(address.LandId));
                }
                return View(model);
            }
            return View(model);
        }

        [Authorize(Roles="Administrator, Ondernemer")]
        public IActionResult PrintAddresses(string klantId, int envelopeId)
        {
            var envelope = _mapper.Map<EnvelopeViewModel>(_envelopeRepository.GetEnvelopeById(envelopeId));
            var addresses = _mapper.Map<List<AddressViewModel>>(_addressRepository.GetAddressesByKlantId(klantId));

            if (addresses.Any())
            {
                // Create new PDF document
                _document = CreatePdf();

                foreach (var address in addresses)
                {
                    // Create new page
                    var page = CreatePdfPage(envelope.Breedte, envelope.Lengte); //document.AddPage();
                    // Create address
                    CreateAddress(page, address, envelope);
                }

                //(var i = 0; i < addresses.Count; i++)
                //{
                //    // Create new page
                //    var page = CreatePdfPage(document); //document.AddPage();
                //    //page.Size = PageSize.A5;
                //    //page.Orientation = PageOrientation.Landscape;
                //    //page.Width = XUnit.FromMillimeter(170);
                //    //page.Height = XUnit.FromMillimeter(170);
            
                //    // Create address

                //    XGraphics gfx = XGraphics.FromPdfPage(page);
                //    gfx.RotateAtTransform(270,new XPoint(170,270));
                //    // Create a font
                //    XFont font = new XFont("Times New Roman", 16, XFontStyle.Regular);

                //    //get address for test
                //    var address = _addressRepository.GetAddressById(i + 6);
                //    var naam = string.Concat(address.Voornaam, address.Tussenvoegsel != null ? " " + address.Tussenvoegsel : " ", address.Achternaan); 
                //    // Draw the text
                //    gfx.DrawString(string.Concat(address.Voornaam, address.Tussenvoegsel != null ? " " + address.Tussenvoegsel  + " " : " ", address.Achternaan), font, XBrushes.Black, new XRect(100, 40, page.Width, page.Height), XStringFormats.CenterLeft);
                //    gfx.DrawString(string.Concat(address.Straat, " ", address.Huisnummer, " ", address.HuisnummerToevoeging), font, XBrushes.Black, new XRect(100, 60, page.Width, page.Height), XStringFormats.CenterLeft);
                //    gfx.DrawString(string.Concat(address.Postcode, "  ", address.Plaats), font, XBrushes.Black, new XRect(100, 80, page.Width, page.Height), XStringFormats.CenterLeft);
                //    if (address.LandId != Constants.DefaultCountryNlId)
                //    {
                //        gfx.DrawString(string.Concat(_countryRepository.GetById(address.LandId)), font, XBrushes.Black, new XRect(100, 100, page.Width, page.Height), XStringFormats.CenterLeft);
                //    }

                //}

                // Send PDF to browser
                var stream = new MemoryStream();
                
                _document.Save(stream, false);
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.Headers.Add("content-length", stream.Length.ToString());
                
                return File(stream, "application/pdf", "adressen.pdf");
            }

            return RedirectToAction("Index");
        }

        public IActionResult PrintAddress(int addressId, int envelopeId)
        {
            var envelope = _mapper.Map<EnvelopeViewModel>(_envelopeRepository.GetEnvelopeById(envelopeId));
            //get address
            var address = _mapper.Map<AddressViewModel>(_addressRepository.GetAddressById(addressId));

            // Create new PDF document
            _document = CreatePdf();
            // Create new page
            var page = CreatePdfPage(envelope.Breedte, envelope.Lengte); //document.AddPage();
            // Create address
            CreateAddress(page, address, envelope);
            // Send PDF to browser
            var stream = new MemoryStream();
                
            _document.Save(stream, false);
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-length", stream.Length.ToString());
                
            return File(stream, "application/pdf", "adressen.pdf");
        }


        //public IActionResult PrintOrder()
        //{
        //    try
        //    {
                

        //        for (int i = 0; i < 1; i++)
        //        {
        //            // Create new page
        //            PdfPage page = document.AddPage();
        //            page.Size = PageSize.A5;
        //            page.Orientation = PageOrientation.Landscape;
        //            page.Width = XUnit.FromMillimeter(170);
        //            page.Height = XUnit.FromMillimeter(170);
                
        //            // Create graphics object and draw clock
        //            XGraphics gfx = XGraphics.FromPdfPage(page);
        //            gfx.RotateAtTransform(270,new XPoint(170,270));
        //            // Create a font
        //            XFont font = new XFont("Times New Roman", 16, XFontStyle.Regular);

        //            //get address for test
        //            var address = _addressRepository.GetAddressById(i + 6);
        //            var naam = string.Concat(address.Voornaam, address.Tussenvoegsel != null ? " " + address.Tussenvoegsel : " ", address.Achternaan); 
        //            // Draw the text
        //            gfx.DrawString(string.Concat(address.Voornaam, address.Tussenvoegsel != null ? " " + address.Tussenvoegsel  + " " : " ", address.Achternaan), font, XBrushes.Black, new XRect(100, 40, page.Width, page.Height), XStringFormats.CenterLeft);
        //            gfx.DrawString(string.Concat(address.Straat, " ", address.Huisnummer, " ", address.HuisnummerToevoeging), font, XBrushes.Black, new XRect(100, 60, page.Width, page.Height), XStringFormats.CenterLeft);
        //            gfx.DrawString(string.Concat(address.Postcode, "  ", address.Plaats), font, XBrushes.Black, new XRect(100, 80, page.Width, page.Height), XStringFormats.CenterLeft);
        //            if (address.LandId != Constants.DefaultCountryNlId)
        //            {
        //                gfx.DrawString(string.Concat(_countryRepository.GetById(address.LandId)), font, XBrushes.Black, new XRect(100, 100, page.Width, page.Height), XStringFormats.CenterLeft);
        //            }

        //        }

                
        //        //voor vierkante enveloppen begin

        //        //// Create new page
        //        //PdfPage page = document.AddPage();
        //        //page.Size = PageSize.A5;
        //        //page.Width = XUnit.FromMillimeter(170);
        //        //page.Height = XUnit.FromMillimeter(170);
                
        //        //// Create graphics object and draw clock
        //        //XGraphics gfx = XGraphics.FromPdfPage(page);

        //        //// Create a font
        //        //XFont font = new XFont("Times New Roman", 16, XFontStyle.Regular);

        //        ////get address for test
        //        //var address = _addressRepository.GetAddressById(i + 6);
        //        //var naam = string.Concat(address.Voornaam, address.Tussenvoegsel != null ? " " + address.Tussenvoegsel : " ", address.Achternaan); 
        //        //// Draw the text
        //        //gfx.DrawString(string.Concat(address.Voornaam, address.Tussenvoegsel != null ? " " + address.Tussenvoegsel  + " " : " ", address.Achternaan), font, XBrushes.Black, new XRect(150, 0, page.Width, page.Height), XStringFormats.CenterLeft);
        //        //gfx.DrawString(string.Concat(address.Straat, " ", address.Huisnummer, " ", address.HuisnummerToevoeging), font, XBrushes.Black, new XRect(150, 20, page.Width, page.Height), XStringFormats.CenterLeft);
        //        //gfx.DrawString(string.Concat(address.Postcode, "  ", address.Plaats), font, XBrushes.Black, new XRect(150, 40, page.Width, page.Height), XStringFormats.CenterLeft);
        //        //if (address.LandId != Constants.DefaultCountryNlId)
        //        //{
        //        //    gfx.DrawString(string.Concat(_countryRepository.GetById(address.LandId)), font, XBrushes.Black, new XRect(150, 60, page.Width, page.Height), XStringFormats.CenterLeft);
        //        //}

        //        //voor vierkante enveloppen eind
            
        //        // Send PDF to browser
        //        MemoryStream stream = new MemoryStream();

                
                
        //        document.Save(stream, false);
        //        Response.Clear();
        //        Response.ContentType = "application/pdf";
        //        Response.Headers.Add("content-length", stream.Length.ToString());
                
        //        return File(stream, "application/pdf", "test.pdf");
                
        //        //byte[] image = Convert.FromBase64String(_base64Image2);
        //        //Response.Body.WriteAsync(stream.ToArray());
        //        ////Response.BinaryWrite(stream.ToArray());
        //        //Response.Body.Flush();
        //        //stream.Close();
        //        ////Response.End();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }

        //    return null;
        //}

        private PdfDocument CreatePdf()
        {
            // Create new PDF document
            var document = new PdfDocument();
            var time = document.Info.CreationDate;
            document.Info.Title = "Adressen";
            document.Info.Author = User.Identity.Name;
            document.Info.Subject = "Server time: " + time.ToString("F", CultureInfo.InvariantCulture);

            return document;
        }

        private PdfPage CreatePdfPage(int pageWidth, int pageHeigth)
        {
            var page = _document.AddPage();
            page.Size = PageSize.A5;
            //page.Orientation = PageOrientation.Landscape;
            page.Width = XUnit.FromMillimeter(pageWidth);
            page.Height = XUnit.FromMillimeter(pageHeigth);

            return page;
        }

        private void CreateAddress(PdfPage page, AddressViewModel address, EnvelopeViewModel envelope = null)
        {
            XGraphics gfx = XGraphics.FromPdfPage(page);

            if (page.Height < page.Width)
            {
                //gfx.RotateAtTransform(270,new XPoint(envelope.Breedte,envelope.Lengte));
            }
            //gfx.RotateAtTransform(270,new XPoint(170,270));
            //gfx.RotateAtTransform(270,new XPoint(page.Width,page.Height));
            // Create a font
            XFont font = new XFont("Times New Roman", 16, XFontStyle.Regular);

            // Draw the text
            //gfx.DrawString(string.Concat(address.Voornaam, address.Tussenvoegsel != null ? " " + address.Tussenvoegsel  + " " : " ", address.Achternaan), font, XBrushes.Black, new XRect(75, 140, page.Width, page.Height), XStringFormats.CenterLeft);
            //gfx.DrawString(string.Concat(address.Straat, " ", address.Huisnummer, " ", address.HuisnummerToevoeging), font, XBrushes.Black, new XRect(75, 160, page.Width, page.Height), XStringFormats.CenterLeft);
            //gfx.DrawString(string.Concat(address.Postcode, "  ", address.Plaats), font, XBrushes.Black, new XRect(75, 180, page.Width, page.Height), XStringFormats.CenterLeft);
            //if (address.LandId != Constants.DefaultCountryNlId)
            //{
            //    gfx.DrawString(string.Concat(_countryRepository.GetById(address.LandId)), font, XBrushes.Black, new XRect(75, 200, page.Width, page.Height), XStringFormats.CenterLeft);
            //}
            //Draw the text
            gfx.DrawString(string.Concat(address.Voornaam, address.Tussenvoegsel != null ? " " + address.Tussenvoegsel + " " : " ", address.Achternaan), font, XBrushes.Black, new XRect(150 + (envelope.OffsetTop ?? 0), 100 + (envelope.OffsetLinks ?? 0), page.Width, page.Height), XStringFormats.CenterLeft);
            gfx.DrawString(string.Concat(address.Straat, " ", address.Huisnummer, " ", address.HuisnummerToevoeging), font, XBrushes.Black, new XRect(150 + (envelope.OffsetTop ?? 0), 120 + (envelope.OffsetLinks ?? 0), page.Width, page.Height), XStringFormats.CenterLeft);
            gfx.DrawString(string.Concat(address.Postcode, "  ", address.Plaats), font, XBrushes.Black, new XRect(150 + (envelope.OffsetTop ?? 0), 140 + (envelope.OffsetLinks ?? 0), page.Width, page.Height), XStringFormats.CenterLeft);
            if (address.LandId != Constants.DefaultCountryNlId)
            {
                gfx.DrawString(string.Concat(_countryRepository.GetById(address.LandId)), font, XBrushes.Black, new XRect(150 + (envelope.OffsetTop ?? 0), 160 + (envelope.OffsetLinks ?? 0), page.Width, page.Height), XStringFormats.CenterLeft);
            }
        }



    }
}
