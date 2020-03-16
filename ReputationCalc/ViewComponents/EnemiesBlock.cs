﻿using BeastHunterWebApps.Models;
using BeastHunterControllers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BeastHunterWebApps.ViewComponents
{
    public class EnemiesBlock : ViewComponent
    {
        #region PrivateData

        private readonly IEnemyServices _enemyServices;

        #endregion


        #region ClassLifeCycles

        public EnemiesBlock(IEnemyServices enemyServices)
        {
            _enemyServices = enemyServices;
        }

        #endregion


        #region Methods

        public async Task<IViewComponentResult> InvokeAsync()
        {
            EnemiesViewModel enemiesView = new EnemiesViewModel();
            enemiesView.Enemies = _enemyServices.GetAll();

            return View(enemiesView);
        }

        #endregion
    }
}
