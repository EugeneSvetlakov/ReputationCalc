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
            EnemyViewModel model = new EnemyViewModel {
                Id = -1,
                Name = "New Enemy Name"
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(EnemyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //var newid = _enemyServices.GetAll().OrderByDescending(e => e.Id).First().Id + 1;

            Enemy newenemy = new Enemy
            {
                //Id = newid,
                Name = model.Name,
                EnemyItems = new List<EnemyItem>()
            };

            _enemyServices.Add(newenemy);

            return RedirectToAction("Index", "Enemy");
        }

        [HttpGet]
        public IActionResult AvailableItems(int enemy)
        {
            //ToDo
            IEnumerable<Item> itemslist;

            if (_enemyServices.GetById(enemy).EnemyItems == null)
            {
                itemslist = _itemServices.GetAll();
            }
            else
            {
                //var enemy_items = _enemyServices.GetById(enemy).EnemyItems.Select(i => i.ItemId);
                //var excluding_items = _itemServices.GetAll().Where(i => !enemy_items.Contains(i.Id));
                //var not_in_enemy = excluding_items.Select(i => i.Id);

                itemslist = _itemServices
                    .GetAll()
                    .Where(i => !_enemyServices
                        .GetById(enemy)
                        .EnemyItems.Select(e => e.ItemId).Contains(i.Id));
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

            _enemyServices.Update(new Enemy { Id = model.Id, Name = model.Name });
                //GetById(model.Id).Name = model.Name;

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
            var item = _enemyServices.GenerateItem(id);
            var enemy = _enemyServices.GetById(id);
            string itemName;

            switch (item)
            {
                case -1:
                    itemName = "[No Items Error: Enemy must have minimum 1 Item]";
                    break;
                case -2:
                    itemName = "[Null Chance Error: Enemy must have minimum 1 Item with chance > 0]";
                    break;
                default:
                    itemName = _itemServices.GetById(item).Name;
                    break;
            }

            EnemyItemWithChanceViewModel model = 
                new EnemyItemWithChanceViewModel
                {
                    EnemyId = id,
                    EnemyName = enemy.Name,
                    ItemId = item,
                    ItemName = itemName,
                    Chance = (item < 0)? 0 : enemy.EnemyItems.FirstOrDefault(i => i.ItemId == item).Chance
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

            //var test = _enemyServices.GetById(enemy);

            return RedirectToAction("AvailableItems", "Enemy", new { enemy = enemy });
        }

        [HttpGet]
        public IActionResult EditItemChance(int enemy, int item)
        {
            var enemyName = _enemyServices.GetById(enemy).Name;

            var enemyItem = _enemyServices
                .GetById(enemy)
                .EnemyItems
                .First(i => i.ItemId == item);

            EnemyItemWithChanceViewModel model = new EnemyItemWithChanceViewModel
            {
                EnemyId = enemy,
                EnemyName = enemyName,
                ItemId = item,
                ItemName = _itemServices.GetById(item).Name,
                Chance = enemyItem.Chance
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