using Microsoft.AspNetCore.Mvc;
using ReputationController.Interfaces;
using ReputationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReputationCalc.ViewComponents
{
    public class ReputationBlock : ViewComponent
    {
        private readonly IReputationServices _reputationService;

        public ReputationBlock(IReputationServices reputationService)
        {
            _reputationService = reputationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var reputation = GetReputation();
            return View(reputation);
        }

        private Reputation GetReputation()
        {
            return _reputationService.GetReputation();
        }
    }
}
