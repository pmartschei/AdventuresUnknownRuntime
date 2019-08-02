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
        [SerializeField] private bool m_KeepPercentage = true;
        #region Properties

        #endregion

        #region Methods
        public override void Initialize(Entity activeStats)
        {
            activeStats.NotifyOnStatChange(activeStats.GetStat(ModType.Identifier), this.Root);
        }

        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            Stat target = activeStats.GetStat(m_Target.Identifier);
            Stat source = activeStats.GetStat(ModType.Identifier);

            float percentage = target.Percentage;
            target.RemoveStatModifiersBySource(this);
            target.AddStatModifier(new StatModifier(source.Flat, CalculationType.Flat, this));
            target.AddStatModifier(new StatModifier(source.Increased -1.0f, CalculationType.Increased, this));
            target.AddStatModifier(new StatModifier(source.More -1.0f, CalculationType.More, this));
            target.AddStatModifier(new StatModifier(source.FlatExtra, CalculationType.FlatExtra, this));
            if (m_KeepPercentage)
            {
                target.Current = target.Calculated * percentage;
            }
        }
        #endregion
    }
}
