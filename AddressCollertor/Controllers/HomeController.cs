﻿using AddressCollector.Models;
using AddressCollector.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AddressCollector.Controllers
{
    [Authorize(Roles="Administrator, Ondernemer, Klant")]
    public class HomeController : Controller
    {
        //private readonly IPieRepository _pieRepository;

        public HomeController(IPieRepository pieRepository)
        {
            //_pieRepository = pieRepository;
        }

        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel();
            
            return View(homeViewModel);
        }

        
    }
}