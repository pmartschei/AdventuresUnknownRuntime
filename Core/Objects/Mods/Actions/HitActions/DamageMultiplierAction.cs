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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/HitApply/DamageMultiplierAction", fileName = "DamageMultiplierAction.asset")]
    public class DamageMultiplierAction : HitApplyAction
    {
        [SerializeField] private ModTypeIdentifier m_Source = null;
        [SerializeField] private ModTypeIdentifier m_Target = null;

        #region Properties

        #endregion

        #region Methods

        #endregion
        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            HitContext hitContext = actionContext as HitContext;
            if (hitContext == null) return;
            if (hitContext.IsProtected) return;
            Entity entity = hitContext.OffensiveEntity;
            Stat stat = entity.GetStat(m_Target.Identifier);
            stat.AddStatModifier(new StatModifier(entity.GetStat(m_Source.Identifier).Calculated, CalculationType.More, this));
        }
    }
}
