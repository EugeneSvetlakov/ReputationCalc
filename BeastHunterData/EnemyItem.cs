using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeastHunterData
{
    public class EnemyItem
    {
        [Key]
        public int EnemyId { get; set; }
        // public virtual Enemy Enemy { get; set; }

        [Key]
        public int ItemId { get; set; }
        // public virtual Item Item { get; set; }

        [DefaultValue(0)]
        public int Chance { get; set; }
    }
}
