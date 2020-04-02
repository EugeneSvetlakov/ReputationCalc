using BeastHunterWebApps.Models;
using BeastHunterControllers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastHunterData;

namespace BeastHunterWebApps.ViewComponents
{
    public class EnemiesBlock : ViewComponent
    {
        #region PrivateData

        private readonly IEnemyServices _enemyServices;
        private readonly IItemServices _itemServices;

        #endregion


        #region ClassLifeCycles

        public EnemiesBlock(IEnemyServices enemyServices, IItemServices itemServices)
        {
            _enemyServices = enemyServices;
            _itemServices = itemServices;
        }

        #endregion


        #region Methods

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<EnemyViewModel> viewModels = new List<EnemyViewModel>();
            var enemies = _enemyServices.GetAll();
            var countEnemys = enemies.Select(e => e.EnemyItems.Count);


            if (enemies != null)
            {
                foreach (var enemy in enemies)
                {
                    viewModels.Add(new EnemyViewModel
                    {
                        Id = enemy.Id,
                        Name = enemy.Name,
                        EnemyItems = new Dictionary<Item, int>()
                    });

                    if(enemy.EnemyItems != null)
                    {
                        viewModels.First(m => m.Id == enemy.Id).EnemyItems = enemy.EnemyItems
                                .ToDictionary(
                                    key => _itemServices.GetById(key.ItemId),
                                    value => value.Chance);
                    }
                }
            }


            //EnemiesViewModel enemiesView = new EnemiesViewModel();

            //enemiesView.Enemies = _enemyServices.GetAll();

            return View(viewModels);
        }

        #endregion
    }
}
