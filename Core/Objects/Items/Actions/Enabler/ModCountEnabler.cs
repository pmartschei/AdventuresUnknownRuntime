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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Actions/Enabler/ModCountEnabler", fileName = "ModCountEnabler.asset")]
    public class ModCountEnabler : AbstractEnabler
    {
        [SerializeField] private ValueType m_ValueType = ValueType.Explicit;
        [SerializeField] private int m_Count = 6;
        [SerializeField] private CheckType m_CheckType = CheckType.Equals;
        [SerializeField] private bool m_Inverted = false;

        #region Properties

        #endregion

        #region Methods

        #endregion
        public override bool IsEnabled(ItemStack itemStack)
        {
            bool isEnabled = true;
            int modCount = itemStack.ExplicitMods.Length;
            if (m_ValueType == ValueType.Implicit)
            {
                modCount = itemStack.ImplicitMods.Length;
            }
            switch (m_CheckType)
            {
                case CheckType.Equals:
                    isEnabled = (modCount == m_Count);
                    break;
                case CheckType.Lower:
                    isEnabled = (modCount < m_Count);
                    break;
                case CheckType.LowerEquals:
                    isEnabled = (modCount <= m_Count);
                    break;
                case CheckType.Greater:
                    isEnabled = (modCount > m_Count);
                    break;
                case CheckType.GreaterEquals:
                    isEnabled = (modCount >= m_Count);
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
