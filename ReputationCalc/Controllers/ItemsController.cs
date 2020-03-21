using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastHunterControllers.Interfaces;
using BeastHunterData;
using BeastHunterWebApps.Models;
using Microsoft.AspNetCore.Mvc;


namespace BeastHunterWebApps.Controllers
{
    public class ItemsController : Controller
    {
        #region PrivateData

        private IItemServices _itemServices;
        private IEnemyServices _enemyServices;

        #endregion


        #region ClassLifeCycles

        public ItemsController(IItemServices itemServices, IEnemyServices enemyServices)
        {
            _itemServices = itemServices;
            _enemyServices = enemyServices;
        }

        #endregion


        #region Methods

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            ItemViewModel model = new ItemViewModel { Id = -1, Name = "New Item Name" };

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(ItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newid = _itemServices.GetAll().OrderByDescending(e => e.Id).First().Id + 1;

            Item newitem = new Item
            {
                Id = newid,
                Name = model.Name
            };

            _itemServices.Add(newitem);

            return RedirectToAction("Index", "Items");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _itemServices.GetById(id);

            ItemViewModel viewModel = new ItemViewModel
            {
                Id = model.Id,
                Name = model.Name
            };

            if (model == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(ItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _itemServices.GetById(model.Id).Name = model.Name;

            return RedirectToAction("Index", "Items");
        }

        public IActionResult Delete(int id)
        {
            var deletingItem = _itemServices.GetById(id);
            var enemiesHavingDeletingItem = _enemyServices.GetAll().Where(e => e.EnemyItems.ContainsKey(deletingItem));

            if (enemiesHavingDeletingItem != null)
            {
                foreach (var enemy in enemiesHavingDeletingItem)
                {
                    _enemyServices.DeleteItemFromEnemy(enemy.Id, deletingItem);
                }
            }

            _itemServices.Delete(id);

            return RedirectToAction("Index", "Items");
        }

        #endregion
    }
}