using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastHunterControllers.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace BeastHunterWebApps.Controllers
{
    public class EnemyController : Controller
    {
        #region PrivateData

        private IEnemyServices _enemyServices;

        #endregion


        #region ClassLifeCycles

        public EnemyController(IEnemyServices enemyServices)
        {
            _enemyServices = enemyServices;
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