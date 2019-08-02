using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Items.Actions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Actions/CraftingActionCatalog", fileName = "CraftingActionCatalog.asset")]
    public class CraftingActionCatalog : CoreObject
    {
        [SerializeField] private CraftingAction[] m_CraftingActions = null;
        
        #region Properties
        public CraftingAction[] CraftingActions { get => m_CraftingActions; set => m_CraftingActions = value; }
        #endregion

        #region Methods
        public override void ForceUpdate()
        {
            base.ForceUpdate();
            foreach(CraftingAction ca in m_CraftingActions)
            {
                ca.ForceUpdate();
            }
        }
        #endregion
    }
}
