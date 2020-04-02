using BeastHunterControllers.Interfaces;
using BeastHunterData;
using BeastHunterData.Enum;
using SqliteStorage;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BeastHunterControllers.Services
{
    public class SqliteReputationServices : IReputationServices
    {

        #region PrivateData

        private readonly SqliteDbContext _context;

        private const int _maxAmount = 1000;

        private const int _minAmount = -1000;

        private const int _thresholdValue = 500; // = (_MaxAmount - _MinAmount) * 0.75 + _MinAmount;

        #endregion


        #region Fields

        #endregion


        #region Properties

        #endregion


        #region ClassLifeCycles

        public SqliteReputationServices(SqliteDbContext context)
        {
            _context = context;

            if (!_context.Reputations.Any())
            {
                _context.Reputations.Add(new Reputation
                {
                    Peasants = 0,
                    Church = 0,
                    Bandits = 0,
                    Nobles = 0
                });

                _context.SaveChanges();
            }

            CorrectReputation();
        }

        public SqliteReputationServices()
        {
            
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
            return _context.Reputations.OrderBy(o => o.Id).FirstOrDefault();
        }

        public int GetPeasantsReputation()
        {
            return GetReputation().Peasants;
        }

        public int GetChurchReputation()
        {
            return GetReputation().Church;
        }

        public int GetBanditsReputation()
        {
            return GetReputation().Bandits;
        }

        public int GetNoblesReputation()
        {
            return GetReputation().Nobles;
        }

        public void AddReputation(Reputation additive)
        {
            UnderThreshold beforeSumm = GetMaxUnderThreshold();

            int[] thresholdArr = GetThresholCorrectingdArr(beforeSumm);

            AddPeasants(additive.Peasants, thresholdArr[0]);
            AddChurch(additive.Church, thresholdArr[1]);
            AddBandits(additive.Bandits, thresholdArr[2]);
            AddNobles(additive.Nobles, thresholdArr[3]);

            CorrectReputation();
        }

        private void AddPeasants(int amount, int threshold)
        {
            _context.Reputations.OrderBy(o => o.Id).FirstOrDefault().Peasants = CorrectAmount(GetPeasantsReputation() + amount, threshold);

            _context.SaveChanges();
        }

        private void AddChurch(int amount, int threshold)
        {
            _context.Reputations.OrderBy(o => o.Id).FirstOrDefault().Church = CorrectAmount(GetChurchReputation() + amount, threshold);

            _context.SaveChanges();
        }

        private void AddBandits(int amount, int threshold)
        {
            _context.Reputations.OrderBy(o => o.Id).FirstOrDefault().Bandits = CorrectAmount(GetBanditsReputation() + amount, threshold);

            _context.SaveChanges();
        }

        private void AddNobles(int amount, int threshold)
        {
            _context.Reputations.OrderBy(o => o.Id).FirstOrDefault().Nobles = CorrectAmount(GetNoblesReputation() + amount, threshold);

            _context.SaveChanges();
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

                _context.Reputations.OrderBy(o => o.Id).FirstOrDefault().Peasants = CorrectAmount(GetPeasantsReputation(), thresholdArr[0]);
                _context.Reputations.OrderBy(o => o.Id).FirstOrDefault().Church = CorrectAmount(GetChurchReputation(), thresholdArr[1]);
                _context.Reputations.OrderBy(o => o.Id).FirstOrDefault().Bandits = CorrectAmount(GetBanditsReputation(), thresholdArr[2]);
                _context.Reputations.OrderBy(o => o.Id).FirstOrDefault().Nobles = CorrectAmount(GetNoblesReputation(), thresholdArr[3]);

                _context.SaveChanges();
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
            var reputation = GetReputation();

            UnderThreshold result = UnderThreshold.NoUnder;

            Dictionary<UnderThreshold, int> underThresholdDictionary = new Dictionary<UnderThreshold, int>();

            if (reputation.Peasants > _thresholdValue)
            {
                underThresholdDictionary.Add(UnderThreshold.ItsPeasants, reputation.Peasants);
            }

            if (reputation.Church > _thresholdValue)
            {
                underThresholdDictionary.Add(UnderThreshold.ItsChurch, reputation.Church);
            }

            if (reputation.Bandits > _thresholdValue)
            {
                underThresholdDictionary.Add(UnderThreshold.ItsBandits, reputation.Bandits);
            }

            if (reputation.Nobles > _thresholdValue)
            {
                underThresholdDictionary.Add(UnderThreshold.ItsNobles, reputation.Nobles);
            }

            if (underThresholdDictionary.Count > 0)
            {
                result = underThresholdDictionary.OrderBy(c => c.Value).First().Key;
            }

            return result;
        }

        private int GetRandomNumber(int min, int max)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(min, max);
        }

        public void SetDefaultReputation()
        {
            var nowReputation = GetReputation();

            nowReputation.Peasants = 0;
            nowReputation.Church = 0;
            nowReputation.Bandits = 0;
            nowReputation.Nobles = 0;

            _context.SaveChanges();
        }

        #endregion
    }
}
