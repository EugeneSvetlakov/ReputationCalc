using BeastHunterControllers.Interfaces;
using BeastHunterData;
using SqliteStorage;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BeastHunterControllers.Services
{
    public class SqliteItemServices : IItemServices
    {
        #region PrivateData

        private readonly SqliteDbContext _context;

        #endregion


        #region Fields

        #endregion


        #region Properties

        #endregion


        #region ClassLifeCycles

        public SqliteItemServices()
        {

        }

        public SqliteItemServices(SqliteDbContext context)
        {
            _context = context;
        }

        #endregion


        #region Methods

        public List<Item> GetAll()
        {
            return _context.Items.ToList();
        }

        public Item GetById(int id)
        {
            return _context.Items.FirstOrDefault(i => i.Id == id);
        }

        public void Add(Item item)
        {
            if (!_context.Items.Contains(item))
            {
                _context.Items.Add(item);
                _context.SaveChanges();
            }
        }

        public void Update(Item item)
        {
            if (_context.Items.ToList().Exists(i => i.Id == item.Id))
            {
                _context.Items.First(i => i.Id == item.Id).Name = item.Name;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            if (_context.Items.ToList().Exists(i => i.Id == id))
            {
                _context.Items.Remove(_context.Items.First(i => i.Id == id));
                _context.SaveChanges();
            }
        }

        #endregion
    }
}
