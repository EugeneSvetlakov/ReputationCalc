using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemsController.Interfaces;
using Microsoft.AspNetCore.Mvc;



namespace BeastHunterWebApps.Controllers
{
    public class EnemyItemsController : Controller
    {
        #region PrivateData

        private IItemServices _itemServices;

        #endregion


        #region ClassLifeCycles

        public EnemyItemsController(IItemServices itemServices)
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