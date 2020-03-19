using BeastHunterControllers.Interfaces;
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
            new Enemy{ Id = 1, Name = "Enemy_1", EnemyItems = new Dictionary<Item, int>() },
            new Enemy{ Id = 2, Name = "Enemy_2", EnemyItems = new Dictionary<Item, int>() },
            new Enemy{ Id = 3, Name = "Enemy_3", EnemyItems = new Dictionary<Item, int>() }
        };

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
                && !_enemies.Find(e => e.Id == id).EnemyItems.ContainsKey(item))
            {
                _enemies.Find(e => e.Id == id).EnemyItems.Add(item, chance);
            }
        }

        public void UpdateItemOnEnemy(int id, Item item, int chance)
        {
            if (_enemies.Exists(e => e.Id == id)
                && _enemies.Find(e => e.Id == id).EnemyItems.ContainsKey(item))
            {
                _enemies.Find(e => e.Id == id).EnemyItems[item] = chance;
            }
        }

        public void DeleteItemFromEnemy(int id, Item item)
        {
            if (_enemies.Exists(e => e.Id == id)
                && _enemies.Find(e => e.Id == id).EnemyItems.ContainsKey(item))
            {
                _enemies.Find(e => e.Id == id).EnemyItems.Remove(item);
            }
        }

        public Item GenerateItem(int id)
        {
            var result = new Item();
            var enemy = GetById(id);
            var ChanceRange = enemy.EnemyItems.Values.Sum();
            var ChancePoint = ChanceRange;
            Dictionary<Item, int> ChanceDictionary = new Dictionary<Item, int>();
            foreach (var item in enemy.EnemyItems)
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
                
                ChancePoint += ChanceDictionary[DicKey];
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
