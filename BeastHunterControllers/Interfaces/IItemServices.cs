using BeastHunterData;
using System;
using System.Collections.Generic;
using System.Text;


namespace BeastHunterControllers.Interfaces
{
    public interface IItemServices
    {
        #region Methods

        List<Item> GetAll();

        Item GetById(int id);

        void Add(Item item);

        void Update(Item item);

        void Delete(int id);

        #endregion
    }
}
