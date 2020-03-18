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
        public IActionResult AddItems(int enemy)
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

            return View(model); //need model contain list of available Items with
        }

        [HttpGet]
        public IActionResult AddItem(int enemy, int item)
        {
            //ToDo

            _enemyServices.AddItemToEnemy(
                enemy,
                _itemServices.GetById(item),
                0
                );

            var test = _enemyServices.GetById(enemy);

            return RedirectToAction("AddItems", "Enemy", new { enemy = enemy });
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

        #endregion
    }
}