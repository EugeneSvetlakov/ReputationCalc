using Microsoft.AspNetCore.Mvc;
using BeastHunterWebApps.Models;
using BeastHunterControllers.Interfaces;
using BeastHunterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BeastHunterWebApps.ViewComponents
{
    public class ReputationBlock : ViewComponent
    {
        #region PrivateData

        private readonly IReputationServices _reputationService;

        #endregion


        #region ClassLifeCycles

        public ReputationBlock(IReputationServices reputationService)
        {
            _reputationService = reputationService;
        }

        #endregion


        #region Methods

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var reputationView = GetReputationViewModel(GetReputation());

            return View(reputationView);
        }

        private Reputation GetReputation()
        {
            return _reputationService.GetReputation();
        }

        private ReputationViewModel GetReputationViewModel(Reputation reputation)
        {
            var maxValue = _reputationService.GetMaxAmount();

            var minValue = _reputationService.GetMinAmount();

            var threshold = _reputationService.GetThresholdValue();

            var test = ((((float)reputation.Peasants - (float)minValue) /
                ((float)maxValue - (float)minValue)) * 100f);

            return new ReputationViewModel()
            {
                MaxValue = maxValue,
                MinValue = minValue,
                Threshold = threshold,
                Peasants = reputation.Peasants,
                Church = reputation.Church,
                Bandits = reputation.Bandits,
                Nobles = reputation.Nobles,
                PeasantsInPercent = (((float)reputation.Peasants - (float)minValue) / 
                    ((float)maxValue - (float)minValue)) * 100f,
                ChurchInPercent = (((float)reputation.Church - (float)minValue) / 
                    ((float)maxValue - (float)minValue)) * 100f,
                BanditsInPercent = (((float)reputation.Bandits - (float)minValue) / 
                    ((float)maxValue - (float)minValue)) * 100f,
                NoblesInPercent = (((float)reputation.Nobles - (float)minValue) / 
                    ((float)maxValue - (float)minValue)) * 100f
            };
        }

        #endregion
    }
}
