using System;
using System.Collections.Generic;
using System.Text;
using BeastHunterData;


namespace BeastHunterControllers.Interfaces
{
    public interface IReputationServices
    {
        #region Methods

        int GetMaxAmount();

        int GetMinAmount();

        int GetThresholdValue();

        Reputation GetReputation();

        int GetPeasantsReputation();

        int GetChurchReputation();

        int GetBanditsReputation();

        int GetNoblesReputation();

        void AddReputation(Reputation additive);
        
        #endregion
    }
}
