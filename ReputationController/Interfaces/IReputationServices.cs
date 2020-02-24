using System;
using System.Collections.Generic;
using System.Text;
using ReputationData;

namespace ReputationController.Interfaces
{
    public interface IReputationServices
    {
        void LoadReputation();

        Reputation GetReputation();

        int GetPeasantsReputation();

        int GetChurchReputation();

        int GetBanditsReputation();

        int GetNoblesReputation();

        void AddReputation(Reputation additive);

        void AddPeasants(int amount);

        void AddChurch(int amount);

        void AddBandits(int amount);

        void AddNobles(int amount);

        int CorrectAmount(int value);

        void CorrectReputation();
    }
}
