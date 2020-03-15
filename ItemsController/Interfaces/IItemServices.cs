using ItemsData;
using System;
using System.Collections.Generic;
using System.Text;


namespace ItemsController.Interfaces
{
    public interface IItemServices
    {
        #region Methods

        List<Item> GetAllItems();

        Item GetItemById(int id);

        void AddItem(Item item);

        void UpdateItem(Item item);

        void DeleteItem(int id);

        List<Enemy> GetAllEnemies();

        #endregion
    }
}
