using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.ConditionActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/ConditionActions/CheckModAction", fileName = "CheckModAction.asset")]
    public class CheckModAction : ConditionAction
    {
        [SerializeField] private float m_Value = 0.0f;
        [SerializeField] private CheckType m_CheckType = CheckType.Lower;
        [SerializeField] private ModTypeIdentifier m_Source = null;
        #region Methods

        public override bool Notify(Entity activeStats, ActionContext actionContext)
        {
            Stat source = activeStats.GetStat(m_Source.Identifier);

            bool flag = true;
            switch (m_CheckType)
            {
                case CheckType.Lower:
                    flag = source.Current < m_Value;
                    break;
                case CheckType.LowerEquals:
                    flag = source.Current <= m_Value;
                    break;
                case CheckType.Greater:
                    flag = source.Current > m_Value;
                    break;
                case CheckType.GreaterEquals:
                    flag = source.Current >= m_Value;
                    break;
                case CheckType.Equals:
                    flag = source.Current == m_Value;
                    break;
            }
            return flag;
        }
        #endregion
    }
}
