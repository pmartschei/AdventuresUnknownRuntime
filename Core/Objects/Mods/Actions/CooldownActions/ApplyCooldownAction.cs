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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/AttackCooldownApply/ApplyCooldownAction", fileName = "ApplyCooldownAction.asset")]
    public class ApplyCooldownAction : BaseAction
    {
        [SerializeField] private ModTypeIdentifier m_Source = null;
        [SerializeField] private ModTypeIdentifier m_Value = null;

        #region Properties
        public override ActionType ActionType { get => ActionTypeManager.AttackCooldownApply; }
        #endregion
        #region Methods

        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            CooldownContext cooldownContext = actionContext as CooldownContext;
            if (cooldownContext == null) return;
            Stat source = cooldownContext.Entity.GetStat(m_Source.Identifier);
            Stat target = activeStats.GetStat(m_Value.Identifier);

            source.Current -= target.Calculated;
        }
        #endregion
    }
}
