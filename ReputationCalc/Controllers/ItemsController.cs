using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastHunterControllers.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace BeastHunterWebApps.Controllers
{
    public class ItemsController : Controller
    {
        #region PrivateData

        private IItemServices _itemServices;

        #endregion


        #region ClassLifeCycles

        public ItemsController(IItemServices itemServices)
        {
            _itemServices = itemServices;
        }

        #endregion


        #region Methods

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}