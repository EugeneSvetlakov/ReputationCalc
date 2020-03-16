using BeastHunterControllers.Interfaces;
using BeastHunterData;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BeastHunterControllers.Services
{
    public class ReputationServices : IReputationServices
    {
        #region PrivateData

        private enum UnderThreshold
        {
                NoUnder = 0,
            ItsPeasants = 1,
              ItsChurch = 2,
             ItsBandits = 3,
              ItsNobles = 4
        }

        private Reputation _reputation;

        private const int _maxAmount = 1000;

        private const int _minAmount = -1000;

        private const int _thresholdValue = 500; // = (_MaxAmount - _MinAmount) * 0.75 + _MinAmount;

        #endregion


        #region Fields

        #endregion


        #region Properties

        #endregion


        #region ClassLifeCycles

        public ReputationServices(Reputation reputation)
        {
            _reputation = reputation;

            CorrectReputation();
        }

        public ReputationServices()
        {
            _reputation = new Reputation();
            _reputation.Peasants = 0;
            _reputation.Church = 0;
            _reputation.Bandits = 0;
            _reputation.Nobles = 0;

            CorrectReputation();
        }
        
        #endregion


        #region Methods

        public int GetMaxAmount()
        {
            return _maxAmount;
        }

        public int GetMinAmount()
        {
            return _minAmount;
        }

        public int GetThresholdValue()
        {
            return _thresholdValue;
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
            UnderThreshold beforeSumm = GetUnderThreshold();

            int[] thresholdArr = GetThresholCorrectingdArr(beforeSumm);

            AddPeasants(additive.Peasants, thresholdArr[0]);
            AddChurch(additive.Church, thresholdArr[1]);
            AddBandits(additive.Bandits, thresholdArr[2]);
            AddNobles(additive.Nobles, thresholdArr[3]);

            CorrectReputation();
        }

        private void AddPeasants(int amount, int threshold)
        {
            _reputation.Peasants = CorrectAmount(_reputation.Peasants + amount, threshold);
        }

        private void AddChurch(int amount, int threshold)
        {
            _reputation.Church = CorrectAmount(_reputation.Church + amount, threshold);
        }

        private void AddBandits(int amount, int threshold)
        {
            _reputation.Bandits = CorrectAmount(_reputation.Bandits + amount, threshold);
        }

        private void AddNobles(int amount, int threshold)
        {
            _reputation.Nobles = CorrectAmount(_reputation.Nobles + amount, threshold);
        }

        /// <summary>
        /// Summ and Return Amount in range [MinAmount, Threshold]
        /// </summary>
        /// <param name="amount">Correcting value</param>
        /// <param name="threshold">Threshold value</param>
        /// <returns></returns>
        private int CorrectAmount(int value, int threshold)
        {
            int curvalue = value;

            if (value > threshold)
            {
                curvalue = threshold;
            }
            else if (value < _minAmount)
            {
                curvalue = _minAmount;
            }

            return curvalue;
        }

        /// <summary>
        /// Only one can be > _ThresholdValue
        /// </summary>
        private void CorrectReputation()
        {
            UnderThreshold maxUnderThreshold = GetMaxUnderThreshold();

            if (maxUnderThreshold != UnderThreshold.NoUnder)
            {
                int[] thresholdArr = GetThresholCorrectingdArr(maxUnderThreshold);

                _reputation.Peasants = CorrectAmount(_reputation.Peasants, thresholdArr[0]);
                _reputation.Church = CorrectAmount(_reputation.Church, thresholdArr[1]);
                _reputation.Bandits = CorrectAmount(_reputation.Bandits, thresholdArr[2]);
                _reputation.Nobles = CorrectAmount(_reputation.Nobles, thresholdArr[3]);
            }
        }

        private static int[] GetThresholCorrectingdArr(UnderThreshold underThreshold)
        {
            int[] thresholdArr;

            if (underThreshold == UnderThreshold.NoUnder)
            {
                thresholdArr = new int[4] { _maxAmount, _maxAmount, _maxAmount, _maxAmount };
            }
            else
            {
                thresholdArr = new int[4] { _thresholdValue, _thresholdValue, _thresholdValue, _thresholdValue };

                if (underThreshold == UnderThreshold.ItsPeasants) thresholdArr[0] = _maxAmount;
                if (underThreshold == UnderThreshold.ItsChurch) thresholdArr[1] = _maxAmount;
                if (underThreshold == UnderThreshold.ItsBandits) thresholdArr[2] = _maxAmount;
                if (underThreshold == UnderThreshold.ItsNobles) thresholdArr[3] = _maxAmount;
            }

            return thresholdArr;
        }

        private UnderThreshold GetMaxUnderThreshold()
        {
            UnderThreshold result = GetUnderThreshold();

            Dictionary<UnderThreshold, int> underThresholdDictionary = new Dictionary<UnderThreshold, int>();

            if (_reputation.Peasants > _thresholdValue)
            {
                underThresholdDictionary.Add(UnderThreshold.ItsPeasants, _reputation.Peasants);
            }

            if (_reputation.Church > _thresholdValue)
            {
                underThresholdDictionary.Add(UnderThreshold.ItsChurch, _reputation.Church);
            }

            if (_reputation.Bandits > _thresholdValue)
            {
                underThresholdDictionary.Add(UnderThreshold.ItsBandits, _reputation.Bandits);
            }

            if (_reputation.Nobles > _thresholdValue)
            {
                underThresholdDictionary.Add(UnderThreshold.ItsNobles, _reputation.Nobles);
            }

            if (underThresholdDictionary.Count > 0)
            {
                result = underThresholdDictionary.OrderBy(c => c.Value).First().Key;
            }

            return result;
        }

        private UnderThreshold GetUnderThreshold()
        {
            UnderThreshold result = UnderThreshold.NoUnder;

            if (_reputation.Peasants > _thresholdValue)
            {
                result = UnderThreshold.ItsPeasants;
            }

            if (_reputation.Church > _thresholdValue)
            {
                result = UnderThreshold.ItsChurch;
            }

            if (_reputation.Bandits > _thresholdValue)
            {
                result = UnderThreshold.ItsBandits;
            }

            if (_reputation.Nobles > _thresholdValue)
            {
                result = UnderThreshold.ItsNobles;
            }

            return result;
        }

        private int GetRandomNumber(int min, int max)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(min, max);
        }

        #endregion
    }
}
