using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeastHunterData
{
    public class Enemy
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<EnemyItem> EnemyItems { get; set; }
        //public Dictionary<Item, int> EnemyItems { get; set; }

        #endregion
    }
}
