using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items.Interfaces;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Items.Actions.Enabler
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Actions/Enabler/PowerLevelEnabler", fileName = "PowerLevelEnabler.asset")]
    public class PowerLevelEnabler : AbstractEnabler
    {
        [SerializeField] private int m_Level = 6;
        [SerializeField] private CheckType m_CheckType = CheckType.Equals;
        [SerializeField] private bool m_Inverted = false;

        #region Properties

        #endregion

        #region Methods

        #endregion
        public override bool IsEnabled(ItemStack itemStack)
        {
            bool isEnabled = true;
            int level = itemStack.PowerLevel;
            switch (m_CheckType)
            {
                case CheckType.Equals:
                    isEnabled = (level == m_Level);
                    break;
                case CheckType.Lower:
                    isEnabled = (level < m_Level);
                    break;
                case CheckType.LowerEquals:
                    isEnabled = (level <= m_Level);
                    break;
                case CheckType.Greater:
                    isEnabled = (level > m_Level);
                    break;
                case CheckType.GreaterEquals:
                    isEnabled = (level >= m_Level);
                    break;
            }
            if (m_Inverted)
            {
                isEnabled = !isEnabled;
            }
            return isEnabled;
        }
    }
}
