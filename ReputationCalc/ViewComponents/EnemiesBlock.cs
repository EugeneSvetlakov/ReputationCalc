using BeastHunterWebApps.Models;
using ItemsController.Interfaces;
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

        private readonly IItemServices _itemServices;

        #endregion


        #region ClassLifeCycles

        public EnemiesBlock(IItemServices itemServices)
        {
            _itemServices = itemServices;
        }

        #endregion


        #region Methods

        public async Task<IViewComponentResult> InvokeAsync()
        {
            EnemiesViewModel enemiesView = new EnemiesViewModel();
            enemiesView.Enemies = _itemServices.GetAllEnemies();

            return View(enemiesView);
        }

        #endregion
    }
}
