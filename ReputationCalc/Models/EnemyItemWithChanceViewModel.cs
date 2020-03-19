using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeastHunterWebApps.Models
{
    public class EnemyItemWithChanceViewModel
    {
        public int EnemyId { get; set; }

        public string EnemyName { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public int Chance { get; set; }
    }
}
