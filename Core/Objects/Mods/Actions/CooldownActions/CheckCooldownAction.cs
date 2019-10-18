using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.CooldownActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/AttackCooldownApply/CheckCooldownAction", fileName = "CheckCooldownAction.asset")]
    public class CheckCooldownAction : AttackCooldownGenerationAction
    {
        [SerializeField] private ModTypeIdentifier m_Source = null;
        [SerializeField] private CheckType m_CheckType = CheckType.Lower;
        [SerializeField] private ModTypeIdentifier m_Value = null;


        #region Properties
        #endregion
        #region Methods


        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            CooldownContext cooldownContext = actionContext as CooldownContext;
            if (cooldownContext == null) return;
            if (!cooldownContext.CanUse) return;
            Stat source = cooldownContext.Entity.GetStat(m_Source.Identifier);
            Stat target = activeStats.GetStat(m_Value.Identifier);

            bool flag = true;
            switch (m_CheckType)
            {
                case CheckType.Lower:
                    flag = source.Current < target.Calculated;
                    break;
                case CheckType.LowerEquals:
                    flag = source.Current <= target.Calculated;
                    break;
                case CheckType.Greater:
                    flag = source.Current > target.Calculated;
                    break;
                case CheckType.GreaterEquals:
                    flag = source.Current >= target.Calculated;
                    break;
                case CheckType.Equals:
                    flag = source.Current == target.Calculated;
                    break;
            }
            if (!flag)
            {
                cooldownContext.CanUse = false;
            }
        }
        #endregion
    }
}
