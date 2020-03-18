using BeastHunterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeastHunterWebApps.Models
{
    public class EnemyItemsViewModel
    {
        public int EnemyId { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
