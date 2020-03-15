using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using BeastHunterWebApps.Models;
using ReputationController.Interfaces;
using ReputationData;


namespace BeastHunterWebApps.Controllers
{
    public class HomeController : Controller
    {
        #region PrivateData

        private IReputationServices _reputationService;

        #endregion


        #region ClassLifeCycles

        public HomeController(IReputationServices reputationServices)
        {
            _reputationService = reputationServices;
        }

        #endregion


        #region Methods

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Index(ReputationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Reputation reputation = new Reputation
            {
                Peasants = model.Peasants,
                Church = model.Church,
                Bandits = model.Bandits,
                Nobles = model.Nobles
            };

            _reputationService.AddReputation(reputation);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion
    }
}
