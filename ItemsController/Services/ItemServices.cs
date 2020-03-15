using ItemsController.Interfaces;
using ItemsData;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ItemsController.Services
{
    public class ItemServices : IItemServices
    {
        #region PrivateData

        private List<Item> _items = new List<Item>
        {
            new Item{ Id = 1, Name = "Item_1" },
            new Item{ Id = 2, Name = "Item_2" },
            new Item{ Id = 3, Name = "Item_3" },
            new Item{ Id = 4, Name = "Item_4" },
            new Item{ Id = 5, Name = "Item_5" },
            new Item{ Id = 6, Name = "Item_6" },
            new Item{ Id = 7, Name = "Item_7" },
            new Item{ Id = 8, Name = "Item_8" },
            new Item{ Id = 9, Name = "Item_9" },
            new Item{ Id = 10, Name = "Item_10" },
            new Item{ Id = 11, Name = "Item_11" },
            new Item{ Id = 12, Name = "Item_12" },
            new Item{ Id = 13, Name = "Item_13" }
        };

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

        public ItemServices()
        {

        }

        public ItemServices(List<Item> items, List<Enemy> enemies)
        {
            _items = items ?? throw new ArgumentNullException(nameof(items));
            _enemies = enemies ?? throw new ArgumentNullException(nameof(enemies));
        }

        #endregion


        #region Methods

        public List<Item> GetAllItems()
        {
            return _items;
        }

        public Item GetItemById(int id)
        {
            return _items.FirstOrDefault(i => i.Id == id);
        }

        public void AddItem(Item item)
        {
            if (!_items.Contains(item))
            {
                _items.Add(item);
            }
        }

        public void UpdateItem(Item item)
        {
            if (_items.Exists(i => i.Id == item.Id))
            {
                _items.Find(i => i.Id == item.Id).Name = item.Name;
            }
        }

        public void DeleteItem(int id)
        {
            if(_items.Exists(i => i.Id == id))
            {
                _items.Remove(_items.Find(i => i.Id == id));
            }
        }

        public List<Enemy> GetAllEnemies()
        {
            return _enemies;
        }

        #endregion
    }
}
