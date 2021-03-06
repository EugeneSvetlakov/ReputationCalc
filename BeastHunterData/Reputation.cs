﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeastHunterData
{
    public class Reputation
    {
        #region Properties

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public int Peasants { get; set; }

        [Required]
        public int Church { get; set; }

        [Required]
        public int Bandits { get; set; }

        [Required]
        public int Nobles { get; set; }

        #endregion
    }
}
