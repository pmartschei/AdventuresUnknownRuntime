using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.DropTables
{
    [Serializable]
    public class DropRate
    {

        [SerializeField] private DropTable m_DropTable = null;
        [Range(0.0f,1.0f)]
        [SerializeField] private float m_Rate = 0.0f;
        [Range(0, 10)]
        [SerializeField] private int m_Count = 0;

        #region Properties

        #endregion

        #region Methods
        public virtual ItemStack[] Roll(int maxLevel,float rateMultiplier = 1.0f)
        {
            List<ItemStack> rolledItemStacks = new List<ItemStack>();
            List<Item> rolledItems = new List<Item>();
            for(int i = 0; i < m_Count; i++)
            {
                float roll = UnityEngine.Random.Range(0.0f, 1.0f);
                if (roll <= m_Rate * rateMultiplier)
                {
                    ItemStack itemStack = m_DropTable.Roll(maxLevel, rolledItems);
                    if (itemStack != null)
                    {
                        rolledItemStacks.Add(itemStack);
                        rolledItems.Add(itemStack.Item);
                    }
                }
            }
            return rolledItemStacks.ToArray();
        }
        #endregion
    }
}
