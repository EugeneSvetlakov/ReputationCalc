using BeastHunterControllers.Interfaces;
using BeastHunterData;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BeastHunterControllers.Services
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

        #endregion


        #region Fields

        #endregion


        #region Properties

        #endregion


        #region ClassLifeCycles

        public ItemServices()
        {

        }

        public ItemServices(List<Item> items)
        {
            _items = items ?? throw new ArgumentNullException(nameof(items));
        }

        #endregion


        #region Methods

        public List<Item> GetAll()
        {
            return _items;
        }

        public Item GetById(int id)
        {
            return _items.FirstOrDefault(i => i.Id == id);
        }

        public void Add(Item item)
        {
            if (!_items.Contains(item))
            {
                _items.Add(item);
            }
        }

        public void Update(Item item)
        {
            if (_items.Exists(i => i.Id == item.Id))
            {
                _items.Find(i => i.Id == item.Id).Name = item.Name;
            }
        }

        public void Delete(int id)
        {
            if (_items.Exists(i => i.Id == id))
            {
                _items.Remove(_items.Find(i => i.Id == id));
            }
        }

        #endregion
    }
}
