using ReputationController.Interfaces;
using ReputationData;
using System;

namespace ReputationController.Services
{
    public class ReputationServices : IReputationServices
    {
        private Reputation _reputation;
        private const int _MaxAmount = 100;
        private const int _MinAmount = 0;

        public ReputationServices(Reputation reputation)
        {
            _reputation = reputation;

            CorrectReputation();
        }

        public ReputationServices()
        {
            _reputation = new Reputation();

            _reputation.Peasants = GetRandomNumber(_MinAmount, _MaxAmount);
            _reputation.Church = GetRandomNumber(_MinAmount, _MaxAmount);
            _reputation.Bandits = GetRandomNumber(_MinAmount, _MaxAmount);
            _reputation.Nobles = GetRandomNumber(_MinAmount, _MaxAmount);

            CorrectReputation();
        }

        public void LoadReputation()
        {
            _reputation = new Reputation();

            _reputation.Peasants = GetRandomNumber(_MinAmount, _MaxAmount);
            _reputation.Church = GetRandomNumber(_MinAmount, _MaxAmount);
            _reputation.Bandits = GetRandomNumber(_MinAmount, _MaxAmount);
            _reputation.Nobles = GetRandomNumber(_MinAmount, _MaxAmount);

            CorrectReputation();
        }

        public Reputation GetReputation()
        {
            return _reputation;
        }

        public int GetPeasantsReputation()
        {
            return _reputation.Peasants;
        }

        public int GetChurchReputation()
        {
            return _reputation.Church;
        }

        public int GetBanditsReputation()
        {
            return _reputation.Bandits;
        }

        public int GetNoblesReputation()
        {
            return _reputation.Nobles;
        }

        public void AddReputation(Reputation additive)
        {
            AddPeasants(additive.Peasants);
            AddChurch(additive.Church);
            AddBandits(additive.Bandits);
            AddNobles(additive.Nobles);
        }

        public void AddPeasants(int amount)
        {
            _reputation.Peasants = CorrectAmount(_reputation.Peasants + amount);

            CorrectReputation();
        }

        public void AddChurch(int amount)
        {
            _reputation.Church = CorrectAmount(_reputation.Church + amount);

            CorrectReputation();
        }

        public void AddBandits(int amount)
        {
            _reputation.Bandits = CorrectAmount(_reputation.Bandits + amount);

            CorrectReputation();
        }

        public void AddNobles(int amount)
        {
            _reputation.Nobles = CorrectAmount(_reputation.Nobles + amount);

            CorrectReputation();
        }

        /// <summary>
        /// Return Amount in range [MinAmount, MaxAmount]
        /// </summary>
        /// <param name="amount">Correcting value</param>
        /// <returns></returns>
        public int CorrectAmount(int value)
        {
            int curvalue = value;

            if (value > _MaxAmount)
            {
                curvalue = _MaxAmount;
            }
            else if (value < _MinAmount)
            {
                curvalue = 0;
            }

            return curvalue;
        }

        // ??? 
        // При изменении репутации у одного из слоев населения, 
        // пропорционально изменять репутацию у остальнеых слоев населения
        // If you change the reputation of one of the strata of the population, 
        // proportionally change the reputation of the rest of the population
        public void CorrectReputation()
        {
            if(_reputation.Peasants > _MaxAmount / 2)
            {
                _reputation.Church = _reputation.Church % (_MaxAmount / 2);
                _reputation.Bandits = _reputation.Bandits % (_MaxAmount / 2);
                _reputation.Nobles = _reputation.Nobles % (_MaxAmount / 2);
            }

            if (_reputation.Church > _MaxAmount / 2)
            {
                _reputation.Peasants = _reputation.Peasants % (_MaxAmount / 2);
                _reputation.Bandits = _reputation.Bandits % (_MaxAmount / 2);
                _reputation.Nobles = _reputation.Nobles % (_MaxAmount / 2);
            }

            if (_reputation.Bandits > _MaxAmount / 2)
            {
                _reputation.Church = _reputation.Church % (_MaxAmount / 2);
                _reputation.Peasants = _reputation.Peasants % (_MaxAmount / 2);
                _reputation.Nobles = _reputation.Nobles % (_MaxAmount / 2);
            }

            if (_reputation.Nobles > _MaxAmount / 2)
            {
                _reputation.Church = _reputation.Church % (_MaxAmount / 2);
                _reputation.Bandits = _reputation.Bandits % (_MaxAmount / 2);
                _reputation.Peasants = _reputation.Peasants % (_MaxAmount / 2);
            }

        }

        private int GetRandomNumber(int min, int max)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(min, max);
        }
    }
}
