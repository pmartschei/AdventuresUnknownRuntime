using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.HitActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/HitCalculation/ProtectionCalculationAction", fileName = "ProtectionCalculationAction.asset")]
    public class ProtectionCalculationAction : HitCalculationAction
    {
        [SerializeField] private ProtectionCause m_ProtectionCause = null;
        [SerializeField] private ActionType m_NotifyTypeAttacker = null;
        [SerializeField] private ActionType m_NotifyTypeDefender = null;

        #region Properties

        #endregion

        #region Methods

        #endregion
        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            HitContext hitContext = actionContext as HitContext;
            if (hitContext == null) return;
            if (hitContext.IsProtected) return;
            if (m_ProtectionCause != null)
            {
                hitContext.IsProtected = true;
                hitContext.ProtectionCause = m_ProtectionCause;
            }
            if (m_NotifyTypeAttacker)
                hitContext.Source.Entity.Notify(m_NotifyTypeAttacker,actionContext);
            if (m_NotifyTypeDefender)
                hitContext.Target.Entity.Notify(m_NotifyTypeDefender,actionContext);
        }
    }
}
