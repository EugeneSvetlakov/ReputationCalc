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
    public class EnemyController : Controller
    {
        #region PrivateData

        private IEnemyServices _enemyServices;
        private IItemServices _itemServices;

        #endregion


        #region ClassLifeCycles

        public EnemyController(IEnemyServices enemyServices, IItemServices itemServices)
        {
            _enemyServices = enemyServices;
            _itemServices = itemServices;
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
            EnemyViewModel model = new EnemyViewModel { Id = -1, Name = "New Enemy Name" };

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(EnemyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newid = _enemyServices.GetAll().OrderByDescending(e => e.Id).First().Id + 1;

            Enemy newenemy = new Enemy
            {
                Id = newid,
                Name = model.Name,
                EnemyItems = new Dictionary<Item, int>()
            };

            _enemyServices.Add(newenemy);

            return RedirectToAction("Index", "Enemy");
        }

        [HttpGet]
        public IActionResult AvailableItems(int enemy)
        {
            //ToDo
            IEnumerable<Item> itemslist;

            if (_enemyServices.GetById(enemy) == null)
            {
                itemslist = _itemServices.GetAll();
            }
            else
            {
                itemslist = _itemServices
                    .GetAll()
                    .Where(i => !_enemyServices
                        .GetById(enemy)
                        .EnemyItems.Any(ie => ie.Key.Id == i.Id));
            }

            EnemyItemsViewModel model = new EnemyItemsViewModel
            {
                EnemyId = enemy,
                Items = itemslist
            };

            return View(model);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _enemyServices.GetById(id);

            EnemyViewModel viewModel = new EnemyViewModel
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
        public IActionResult Edit(EnemyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _enemyServices.GetById(model.Id).Name = model.Name;

            return RedirectToAction("Index", "Enemy");
        }

        public IActionResult Delete(int id)
        {
            _enemyServices.Delete(id);

            return RedirectToAction("Index", "Enemy");
        }

        [HttpGet]
        public IActionResult GenerateItem(int id)
        {
            Item item = _enemyServices.GenerateItem(id);
            var enemy = _enemyServices.GetById(id);

            EnemyItemWithChanceViewModel model = 
                new EnemyItemWithChanceViewModel
                {
                    EnemyId = id,
                    EnemyName = enemy.Name,
                    ItemId = item.Id,
                    ItemName = item.Name,
                    Chance = enemy.EnemyItems.First(i => i.Key == item).Value
                };

            return View(model);
        }

        [HttpGet]
        public IActionResult AddItem(int enemy, int item)
        {
            _enemyServices.AddItemToEnemy(
                enemy,
                _itemServices.GetById(item),
                0
                );

            var test = _enemyServices.GetById(enemy);

            return RedirectToAction("AvailableItems", "Enemy", new { enemy = enemy });
        }

        [HttpGet]
        public IActionResult EditItemChance(int enemy, int item)
        {
            var enemyName = _enemyServices.GetById(enemy).Name;

            var enemyItem = _enemyServices
                .GetById(enemy)
                .EnemyItems
                .First(i => i.Key == _itemServices.GetById(item));

            EnemyItemWithChanceViewModel model = new EnemyItemWithChanceViewModel
            {
                EnemyId = enemy,
                EnemyName = enemyName,
                ItemId = item,
                ItemName = enemyItem.Key.Name,
                Chance = enemyItem.Value
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditItemChance(EnemyItemWithChanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _enemyServices
                .UpdateItemOnEnemy(model.EnemyId,
                _itemServices.GetById(model.ItemId),
                model.Chance);

            return RedirectToAction("Index", "Enemy");
        }

        [HttpGet]
        public IActionResult DeleteItem(int enemy, int item)
        {
            var itemToDelete = _itemServices.GetById(item);

            if (itemToDelete != null)
            {
                _enemyServices.DeleteItemFromEnemy(enemy, itemToDelete);
            }

            return RedirectToAction("Index", "Enemy");
        }

        #endregion
    }
}