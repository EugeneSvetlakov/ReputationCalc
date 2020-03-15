using ReputationData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace BeastHunterWebApps.Models
{
    public class ReputationViewModel
    {
        #region Properties

        public int MaxValue;

        public int MinValue;

        public int Threshold;


        [Range(-1000, 1000, ErrorMessage = "Can only be between -1000 .. 1000")]
        [Required]
        public int Peasants { get; set; }

        [Range(-1000, 1000, ErrorMessage = "Can only be between -1000 .. 1000")]
        [Required]
        public int Church { get; set; }

        [Range(-1000, 1000, ErrorMessage = "Can only be between -1000 .. 1000")]
        [Required]
        public int Bandits { get; set; }

        [Range(-1000, 1000, ErrorMessage = "Can only be between -1000 .. 1000")]
        [Required]
        public int Nobles { get; set; }

        public float PeasantsInPercent;

        public float ChurchInPercent;

        public float BanditsInPercent;

        public float NoblesInPercent;
        
        #endregion
    }
}
