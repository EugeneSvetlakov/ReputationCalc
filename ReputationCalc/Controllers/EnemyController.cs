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

            Dictionary<Item, int> model = new Dictionary<Item, int>();

            foreach (var element in itemslist)
            {
                model.Add(element, 0);
            }

            return View(model); //need model contain list of available Items with
        }

        [HttpPost]
        public IActionResult AddItems(int enemy, int item, int chance)
        {
            //ToDo

            _enemyServices.AddItemToEnemy(
                enemy,
                _itemServices.GetById(item),
                chance
                );

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

            Dictionary<Item, int> model = new Dictionary<Item, int>();

            foreach (var element in itemslist)
            {
                model.Add(element, 0);
            }

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

        #endregion
    }
}