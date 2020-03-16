using BeastHunterWebApps.Models;
using BeastHunterControllers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BeastHunterWebApps.ViewComponents
{
    public class ItemsBlock : ViewComponent
    {
        #region PrivateData

        private readonly IItemServices _itemServices;

        #endregion


        #region ClassLifeCycles

        public ItemsBlock(IItemServices itemServices)
        {
            _itemServices = itemServices;
        }

        #endregion


        #region Methods

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ItemsViewModel itemsView = new ItemsViewModel();
            itemsView.Items = _itemServices.GetAll();

            return View(itemsView);
        }

        #endregion
    }
}
