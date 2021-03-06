﻿using BeastHunterControllers.Interfaces;
using BeastHunterData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace BeastHunterControllers.Services
{
    public class EnemyServices : IEnemyServices
    {
        #region PrivateData

        private List<Enemy> _enemies = new List<Enemy>
        {
            new Enemy{ Id = 1, Name = "Enemy_1", EnemyItems = new List<EnemyItem>() },
            new Enemy{ Id = 2, Name = "Enemy_2", EnemyItems = new List<EnemyItem>() },
            new Enemy{ Id = 3, Name = "Enemy_3", EnemyItems = new List<EnemyItem>() }
        };

        private List<EnemyItem> _enemyItems = new List<EnemyItem>();

        #endregion


        #region Fields

        #endregion


        #region Properties

        #endregion


        #region ClassLifeCycles

        public EnemyServices()
        {

        }

        public EnemyServices(List<Enemy> enemies)
        {
            _enemies = enemies ?? throw new ArgumentNullException(nameof(enemies));
        }

        #endregion


        #region Methods

        public List<Enemy> GetAll()
        {
            return _enemies;
        }

        public Enemy GetById(int id)
        {
            return _enemies.FirstOrDefault(i => i.Id == id);
        }

        public void Add(Enemy item)
        {
            if (!_enemies.Contains(item))
            {
                _enemies.Add(item);
            }
        }

        public void Update(Enemy item)
        {
            if (_enemies.Exists(i => i.Id == item.Id))
            {
                _enemies.Find(i => i.Id == item.Id).Name = item.Name;
            }
        }

        public void Delete(int id)
        {
            if(_enemies.Exists(i => i.Id == id))
            {
                _enemies.Remove(_enemies.Find(i => i.Id == id));
            }
        }

        public void AddItemToEnemy(int id, Item item, int chance)
        {
            if(_enemies.Exists(e => e.Id == id) 
                && !_enemies.Find(e => e.Id == id).EnemyItems.Where(e => e.ItemId == item.Id && e.EnemyId == id).Any())
            {
                _enemyItems.Add(new EnemyItem { EnemyId = id, ItemId = item.Id, Chance = chance });

                _enemies.Find(e => e.Id == id).EnemyItems = _enemyItems.Where(e => e.EnemyId == id).ToList();
            }
        }

        public void UpdateItemOnEnemy(int id, Item item, int chance)
        {
            if (_enemies.Exists(e => e.Id == id)
                && _enemies.Find(e => e.Id == id)
                .EnemyItems.Where(e => e.ItemId == item.Id 
                                        && e.EnemyId == id).Any())
            {
                _enemyItems.Find(e => e.EnemyId == id && e.ItemId == item.Id).Chance = chance;

                _enemies.Find(e => e.Id == id).EnemyItems = _enemyItems.Where(e => e.EnemyId == id).ToList();

            }
        }

        public void DeleteItemFromEnemy(int id, Item item)
        {
            if (_enemies.Exists(e => e.Id == id)
                && _enemies.Find(e => e.Id == id).EnemyItems.Where(e => e.ItemId == item.Id
                                        && e.EnemyId == id).Any())
            {
                _enemyItems.Remove(_enemyItems.Find(e => e.ItemId == item.Id && e.EnemyId == id));

                _enemies.Find(e => e.Id == id).EnemyItems = _enemyItems.Where(e => e.EnemyId == id).ToList();

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
                //new Item
                //{
                //    Id = -1,
                //    Name = "[No Items Error: Enemy must have minimum 1 Item]"
                //};
            }

            var ChanceRange = enemy.EnemyItems.Select(e => e.Chance).Sum();
            if(ChanceRange <= 0)
            {
                return -2;
                //new Item
                //{
                //    Id = -2,
                //    Name = "[Null Chance Error: Enemy must have minimum 1 Item with chance > 0]"
                //};
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
                    result = DicKey; //new Item {Id = DicKey, Name = $@"item id: {DicKey} with chance: {ChanceDictionary[DicKey]}" }; //DicKey;
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
