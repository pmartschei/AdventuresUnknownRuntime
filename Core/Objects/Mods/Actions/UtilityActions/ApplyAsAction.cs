using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.UtilityActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Utility/ApplyAsAction", fileName = "ApplyAsAction.asset")]
    public class ApplyAsAction : MultipleBaseAction
    {
        [SerializeField] private ModTypeIdentifier m_Target = null;
        [SerializeField] private bool m_ShipAsTarget = false;
        #region Properties

        #endregion

        #region Methods

        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            Entity targetEntity = m_ShipAsTarget ? activeStats.EntityBehaviour.Entity : activeStats;
            if (targetEntity == null) targetEntity = activeStats;
            Stat target = targetEntity.GetStat(m_Target.Identifier);
            Stat source = activeStats.GetStat(ModType.Identifier);
            AuraContext auraContext = actionContext as AuraContext;

            object sourceObject = this;
            sourceObject = auraContext?.Source;

            target.RemoveStatModifiersBySource(sourceObject);
            target.AddStatModifier(new StatModifierByStat(source, CalculationType.Flat, sourceObject));
            target.AddStatModifier(new StatModifierByStat(source, CalculationType.Increased, sourceObject));
            target.AddStatModifier(new StatModifierByStat(source, CalculationType.More, sourceObject));
            target.AddStatModifier(new StatModifierByStat(source, CalculationType.FlatExtra, sourceObject));
        }
        #endregion
    }
}
