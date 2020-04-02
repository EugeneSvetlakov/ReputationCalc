using BeastHunterControllers.Interfaces;
using BeastHunterData;
using Microsoft.EntityFrameworkCore;
using SqliteStorage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace BeastHunterControllers.Services
{
    public class SqliteEnemyServices : IEnemyServices
    {
        #region PrivateData

        private readonly SqliteDbContext _context;

        #endregion


        #region Fields

        #endregion


        #region Properties

        #endregion


        #region ClassLifeCycles

        public SqliteEnemyServices()
        {

        }

        public SqliteEnemyServices(SqliteDbContext context)
        {
            _context = context;
        }

        #endregion


        #region Methods

        public List<Enemy> GetAll()
        {
            return _context.Enemies
                .Include(i => i.EnemyItems)
                .ToList();
        }

        public Enemy GetById(int id)
        {
            return _context.Enemies
                .Include(i => i.EnemyItems)
                .FirstOrDefault(i => i.Id == id);
        }

        public void Add(Enemy item)
        {
            if (!_context.Enemies.Contains(item))
            {
                _context.Enemies.Add(item);
                _context.SaveChanges();
            }
        }

        public void Update(Enemy item)
        {
            if (_context.Enemies.Where(i => i.Id == item.Id).Any())
            {
                _context.Enemies.First(i => i.Id == item.Id).Name = item.Name;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            if (_context.Enemies.Where(i => i.Id == id).Any())
            {
                _context.Enemies.Remove(_context.Enemies.First(i => i.Id == id));
                _context.SaveChanges();
            }
        }

        public void AddItemToEnemy(int id, Item item, int chance)
        {
            bool enemyExist = _context.Enemies.Where(e => e.Id == id).Any();

            if (enemyExist)
            {
                bool enemyHasItem = (_context.EnemyItems.Where(e => e.EnemyId == id && e.ItemId == item.Id).Any());

                if (!enemyHasItem)
                {
                    _context.EnemyItems.Add(new EnemyItem { EnemyId = id, ItemId = item.Id, Chance = chance });
                    _context.SaveChanges();
                }
            }
        }

        public void UpdateItemOnEnemy(int id, Item item, int chance)
        {
            bool enemyExist = _context.Enemies.Where(e => e.Id == id).Any();

            if (enemyExist)
            {
                bool enemyHasItem = (_context.EnemyItems.Where(e => e.EnemyId == id && e.ItemId == item.Id).Any());

                if (enemyHasItem)
                {
                    _context.EnemyItems
                        .First(e => e.EnemyId == id && e.ItemId == item.Id)
                        .Chance = chance;

                    _context.SaveChanges();
                }
            }
        }

        public void DeleteItemFromEnemy(int id, Item item)
        {
            bool enemyExist = _context.Enemies.Where(e => e.Id == id).Any();

            if (enemyExist)
            {
                bool enemyHasItem = (_context.EnemyItems.Where(e => e.EnemyId == id && e.ItemId == item.Id).Any());

                if (enemyHasItem)
                {
                    _context.EnemyItems.Remove(_context.EnemyItems.First(e => e.ItemId == item.Id && e.EnemyId == id));
                    _context.SaveChanges();
                }

            }
        }

        /// <summary>
        /// Generating droped from Enemy id of Item
        /// </summary>
        /// <param name="id">Enemy.Id</param>
        /// <returns>
        /// Item.Id, -1 if No Items on Enemy, -2 if all Items Chance = 0
        /// </returns>
        public int GenerateItem(int id)
        {
            var result = new int();
            var enemy = GetById(id);
            if (enemy.EnemyItems.Count == 0)
            {
                return -1;
            }

            var ChanceRange = enemy.EnemyItems.Select(e => e.Chance).Sum();
            if (ChanceRange <= 0)
            {
                return -2;
            }

            var ChancePoint = ChanceRange;
            Dictionary<int, int> ChanceDictionary = new Dictionary<int, int>();
            foreach (var item in enemy.EnemyItems.ToDictionary(key => key.ItemId, valey => valey.Chance))
            {
                ChanceDictionary.Add(item.Key, item.Value);
            };
            var length = ChanceDictionary.Count;

            for (int i = length - 1; i >= 0; i--)
            {
                var beforeChange = ChanceDictionary[ChanceDictionary.ElementAt(i).Key];
                ChanceDictionary[ChanceDictionary.ElementAt(i).Key] = ChancePoint;
                ChancePoint -= beforeChange;
            }

            var rndnumber = GetRandomNumber(0, ChanceRange);

            for (int i = 0; i < length; i++)
            {
                var DicKey = ChanceDictionary.ElementAt(i).Key;
                if (rndnumber >= ChancePoint
                    && rndnumber < ChanceDictionary[DicKey])
                {
                    result = DicKey;
                }

                ChancePoint = ChanceDictionary[DicKey];
            }

            return result;
        }

        private int GetRandomNumber(int min, int max)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(min, max);
        }

        #endregion
    }
}
