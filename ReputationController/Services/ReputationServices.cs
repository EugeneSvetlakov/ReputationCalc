using ReputationController.Interfaces;
using ReputationData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReputationController.Services
{
    public class ReputationServices : IReputationServices
    {
        private enum UnderThreshold
        {
            NoUnder,
            ItsPeasants,
            ItsChurch,
            ItsBandits,
            ItsNobles
        }
        private Reputation _reputation;
        private const int _MaxAmount = 1000;
        private const int _MinAmount = -1000;
        private const int _ThresholdValue = 500; // = (_MaxAmount - _MinAmount) * 0.75 + _MinAmount;

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

        public int GetMaxAmount()
        {
            return _MaxAmount;
        }

        public int GetMinAmount()
        {
            return _MinAmount;
        }

        public int GetThresholdValue()
        {
            return _ThresholdValue;
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

            int[] ThresholdArr = GetThresholCorrectingdArr(beforeSumm);

            AddPeasants(additive.Peasants, ThresholdArr[0]);
            AddChurch(additive.Church, ThresholdArr[1]);
            AddBandits(additive.Bandits, ThresholdArr[2]);
            AddNobles(additive.Nobles, ThresholdArr[3]);

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
            else if (value < _MinAmount)
            {
                curvalue = 0;
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
                int[] ThresholdArr = GetThresholCorrectingdArr(maxUnderThreshold);
                _reputation.Peasants = CorrectAmount(_reputation.Peasants, ThresholdArr[0]);
                _reputation.Church = CorrectAmount(_reputation.Church, ThresholdArr[1]);
                _reputation.Bandits = CorrectAmount(_reputation.Bandits, ThresholdArr[2]);
                _reputation.Nobles = CorrectAmount(_reputation.Nobles, ThresholdArr[3]);
            }
        }

        private static int[] GetThresholCorrectingdArr(UnderThreshold underThreshold)
        {
            int[] ThresholdArr;
            if (underThreshold == UnderThreshold.NoUnder)
            {
                ThresholdArr = new int[4] { _MaxAmount, _MaxAmount, _MaxAmount, _MaxAmount };
            }
            else
            {
                ThresholdArr = new int[4] { _ThresholdValue, _ThresholdValue, _ThresholdValue, _ThresholdValue };

                if (underThreshold == UnderThreshold.ItsPeasants) ThresholdArr[0] = _MaxAmount;
                if (underThreshold == UnderThreshold.ItsChurch) ThresholdArr[1] = _MaxAmount;
                if (underThreshold == UnderThreshold.ItsBandits) ThresholdArr[2] = _MaxAmount;
                if (underThreshold == UnderThreshold.ItsNobles) ThresholdArr[3] = _MaxAmount;
            }

            return ThresholdArr;
        }

        private UnderThreshold GetMaxUnderThreshold()
        {
            UnderThreshold result = GetUnderThreshold();

            Dictionary<UnderThreshold, int> UnderThresholdDictionary = new Dictionary<UnderThreshold, int>();

            if (_reputation.Peasants > _ThresholdValue)
            {
                UnderThresholdDictionary.Add(UnderThreshold.ItsPeasants, _reputation.Peasants);
            }
            if (_reputation.Church > _ThresholdValue)
            {
                UnderThresholdDictionary.Add(UnderThreshold.ItsChurch, _reputation.Church);
            }
            if (_reputation.Bandits > _ThresholdValue)
            {
                UnderThresholdDictionary.Add(UnderThreshold.ItsBandits, _reputation.Bandits);
            }
            if (_reputation.Nobles > _ThresholdValue)
            {
                UnderThresholdDictionary.Add(UnderThreshold.ItsNobles, _reputation.Nobles);
            }

            if (UnderThresholdDictionary.Count > 0)
            {
                result = UnderThresholdDictionary.OrderBy(c => c.Value).First().Key;
            }

            return result;
        }

        private UnderThreshold GetUnderThreshold()
        {
            UnderThreshold result = UnderThreshold.NoUnder;

            if (_reputation.Peasants > _ThresholdValue)
            {
                result = UnderThreshold.ItsPeasants;
            }

            if (_reputation.Church > _ThresholdValue)
            {
                result = UnderThreshold.ItsChurch;
            }

            if (_reputation.Bandits > _ThresholdValue)
            {
                result = UnderThreshold.ItsBandits;
            }

            if (_reputation.Nobles > _ThresholdValue)
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

    }
}
