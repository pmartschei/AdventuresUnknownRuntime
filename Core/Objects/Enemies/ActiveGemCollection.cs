using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Enemies
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Enemies/ActiveGemCollection", fileName = "ActiveGemCollection.asset")]
    public class ActiveGemCollection : CoreObject
    {
        [SerializeField] private List<ActiveGemIdentifier> m_ActiveGemsIdentifiers = new List<ActiveGemIdentifier>();

        [NonSerialized] private List<ActiveGem> m_ConsistentActiveGems = new List<ActiveGem>();

        #region Properties
        public List<ActiveGem> ActiveGems { get => m_ConsistentActiveGems; set => m_ConsistentActiveGems = value; }
        #endregion

        #region Methods
        public override bool ConsistencyCheck()
        {
            bool flag = base.ConsistencyCheck();

            m_ConsistentActiveGems.Clear();

            foreach(ActiveGemIdentifier activeGemIdentifier in m_ActiveGemsIdentifiers)
            {
                if (!activeGemIdentifier.ConsistencyCheck()) continue;
                m_ConsistentActiveGems.Add(activeGemIdentifier.Object);
            }

            if (m_ConsistentActiveGems.Count == 0) return false;

            return flag;
        }
        #endregion
    }
}
