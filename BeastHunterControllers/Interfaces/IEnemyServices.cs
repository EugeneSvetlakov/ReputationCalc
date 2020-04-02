using BeastHunterData;
using System;
using System.Collections.Generic;
using System.Text;


namespace BeastHunterControllers.Interfaces
{
    public interface IEnemyServices
    {
        #region Methods

        List<Enemy> GetAll();

        Enemy GetById(int id);

        void Add(Enemy item);

        void Update(Enemy item);

        void Delete(int id);

        void AddItemToEnemy(int id, Item item, int chance);

        void UpdateItemOnEnemy(int id, Item item, int chance);

        void DeleteItemFromEnemy(int id, Item item);

        int GenerateItem(int id);

        #endregion
    }
}
