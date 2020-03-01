using System;
using System.Collections.Generic;
using System.Text;
using ReputationData;

namespace ReputationController.Interfaces
{
    public interface IReputationServices
    {
        int GetMaxAmount();

        int GetMinAmount();

        int GetThresholdValue();

        Reputation GetReputation();

        int GetPeasantsReputation();

        int GetChurchReputation();

        int GetBanditsReputation();

        int GetNoblesReputation();

        void AddReputation(Reputation additive);
    }
}
