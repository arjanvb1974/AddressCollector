using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using AddressCollector.Data.Auth;
using AddressCollector.Data.DataContext;
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

namespace AddressCollector.Controllers
{
    
    public class EnvelopeController : Controller
    {
        private readonly IEnvelopeRepository _envelopeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public EnvelopeController(IEnvelopeRepository envelopeRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _envelopeRepository = envelopeRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        
        [Authorize(Roles="Administrator, Ondernemer")]
        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = new EnvelopeListViewModel();

            var envelopes = _envelopeRepository.AllEnvelopes(User).ToList();

            foreach (var envelope in envelopes)
            {
                model.Envelopes.Add(_mapper.Map<EnvelopeViewModel>(envelopes));
            }
            
            return View();
        }

        [Authorize(Roles="Administrator, Ondernemer")]
        public IActionResult AddNewEnvelope()
        {
            var model = new EnvelopeViewModel();
            //set ondernemerId
            var user = _userManager.FindByNameAsync(User.Identity.Name);
            if (user.Result != null)
            {
                model.OndernemerId = user.Result.Id;
            }
            
            return View(model);
        }

        [Authorize(Roles="Administrator, Ondernemer")]
        [HttpPost]
        public IActionResult AddNewEnvelope(EnvelopeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var envelope = _mapper.Map<Envelope>(model);
            _envelopeRepository.Add(envelope);
            _envelopeRepository.Save();

            return RedirectToAction("EnvelopeManagement");
        }

        
        [Authorize(Roles="Administrator, Ondernemer")]
        public IActionResult EnvelopeManagement()
        {
            var model = new EnvelopeListViewModel();
            model.Envelopes = new List<EnvelopeViewModel>();
            model.Envelopes = _mapper.Map<List<EnvelopeViewModel>>(_envelopeRepository.AllEnvelopes(User).ToList());

            if (User.IsInRole(Constants.AdminUserRole))
            {
                foreach (var envelope in model.Envelopes)
                {
                    envelope.Ondernemer = _userManager.Users.FirstOrDefault(x => x.Id == envelope.OndernemerId);
                }
            }

            return View(model);
        }
        
        [Authorize(Roles="Administrator, Ondernemer")]
        public IActionResult DeleteEnvelope(int id)
        {
           //doe hier de delete
           var envelope = _envelopeRepository.GetEnvelopeById(id);
           _envelopeRepository.Delete(envelope);
           _envelopeRepository.Save();

            return RedirectToAction("EnvelopeManagement");
        }

        [Authorize(Roles="Administrator, Ondernemer")]
        public IActionResult EditEnvelope(int id)
        {
            var envelope = _envelopeRepository.GetEnvelopeById(id);
            var model = _mapper.Map<EnvelopeViewModel>(envelope);
            
            return View(model);
        }

        [Authorize(Roles="Administrator, Ondernemer")]
        [HttpPost]
        public IActionResult EditEnvelope(EnvelopeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var envelope = _mapper.Map<Envelope>(model);
            _envelopeRepository.Update(envelope);
            _envelopeRepository.Save();

            return RedirectToAction("EnvelopeManagement");
        }

        

        

        
        

        

        

        
    }
}
