using ReputationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReputationCalc.Models
{
    public class ReputationViewModel : Reputation
    {
        public int MaxValue;

        public int MinValue;

        public int Threshold;

        public float PeasantsInPercent;

        public float ChurchInPercent;

        public float BanditsInPercent;

        public float NoblesInPercent;
    }


}
