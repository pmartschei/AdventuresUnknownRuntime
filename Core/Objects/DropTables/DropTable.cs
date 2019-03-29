using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using AdventuresUnknownSDK.Core.Log;

namespace AdventuresUnknownSDK.Core.Objects.DropTables
{
    [CreateAssetMenu(menuName ="AdventuresUnknown/Core/DropTables/DropTable",fileName ="DropTable.asset")]
    public class DropTable : CoreObject
    {
        [SerializeField] private bool m_DistinctRolls = false;
        [SerializeField] private List<DropChance> m_DropChances = new List<DropChance>();

        private List<DropChance> m_ConsistentDropChances = new List<DropChance>();

        #region Properties
        public bool DistinctRolls { get => m_DistinctRolls; set => m_DistinctRolls = value; }
        public List<DropChance> DropChances { get => m_DropChances; }

        #endregion

        #region Methods
        public override bool ConsistencyCheck()
        {
            m_ConsistentDropChances.Clear();
            foreach (DropChance dropChance in m_DropChances)
            {
                if (!dropChance.ConsistencyCheck())
                {
                    GameConsole.LogWarningFormat("Skipped inconsistent DropChance: {0} at {1}", dropChance.DropIdentifier);
                    continue;
                }
                if (CheckForRecursion(dropChance))
                {
                    GameConsole.LogWarningFormat("Skipped recursive DropChance: {0} at {1}", dropChance.DropIdentifier);
                    continue;
                }
                m_ConsistentDropChances.Add(dropChance);
            }
            return true;
        }

        private bool CheckForRecursion(DropChance dropChance)
        {
            DropTable dropTable = dropChance.GetDropTable();
            if (dropTable == null) return false;
            if (dropTable == this) return true;
            for (int i = 0; i < dropTable.m_DropChances.Count; i++)
            {
                if (CheckForRecursion(dropTable.m_DropChances[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual Item[] Roll(int count)
        {
            return null;
        }
        #endregion
    }
}
