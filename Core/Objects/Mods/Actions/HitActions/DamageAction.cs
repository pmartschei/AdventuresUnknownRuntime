using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.HitActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/HitApply/DamageAction", fileName = "DamageAction.asset")]
    public class DamageAction : HitApplyAction
    {
        [SerializeField] private ModTypeIdentifier m_Source = null;
        [SerializeField] private ModTypeIdentifier m_Life = null;

        #region Methods

        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            HitContext hitContext = actionContext as HitContext;
            if (hitContext == null) return;
            if (hitContext.IsProtected) return;
            hitContext.Target.Entity.GetStat(m_Life.Identifier).Current -= activeStats.GetStat(m_Source.Identifier).Calculated;
            Debug.LogFormat("{0} took {1} from {2}", hitContext.Target, activeStats.GetStat(m_Source.Identifier).Calculated, m_Source.Identifier);
        }
        #endregion
    }
}
