using System.Collections.Generic;

namespace ItemsData
{
    public class Enemy
    {
        #region Properties

        public int Id;

        public string Name;

        public Dictionary<Item, int> EnemyItems;

        #endregion
    }
}
