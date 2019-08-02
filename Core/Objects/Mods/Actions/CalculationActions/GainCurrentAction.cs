using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.CalculationActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Calculation/GainCurrentAction", fileName = "GainCurrentAction.asset")]
    public class GainCurrentAction : CalculationAction
    {
        [SerializeField] private ModTypeIdentifier m_DestinationMod;
        [SerializeField] private ModTypeIdentifier m_DependingMod;
        
        #region Methods
        public override void Initialize(Entity activeStats)
        {
            Stat destination = activeStats.GetStat(m_DestinationMod.Identifier);
            Stat depending = activeStats.GetStat(m_DependingMod.Identifier);
            destination.AddStatModifier(new StatModifier(0.0f, CalculationType.Flat, this.Root));
            activeStats.NotifyOnStatChange(depending, this);
            //ApplyTimedStat(activeStats);
        }

        private void JustRandomMethod(Entity activeStats)
        {
            Stat destination = activeStats.GetStat(m_DestinationMod.Identifier);
            StatModifier[] statModifiers = destination.GetStatModifiersBySource(this);
            if (statModifiers.Length <= 1) return;
            destination.RemoveStatModifier(statModifiers[1]);
            activeStats.AddTimer(new TimerObject(this, 2.0f, ApplyTimedStat));
        }

        private void ApplyTimedStat(Entity activeStats)
        {
            Stat destination = activeStats.GetStat(m_DestinationMod.Identifier);
            destination.AddStatModifier(new StatModifier(20.0f, CalculationType.Flat, this));
            activeStats.AddTimer(new TimerObject(this, 2.0f, JustRandomMethod));
        }

        public override void Notify(Entity activeStats,ActionContext context)
        {
            Stat destination = activeStats.GetStat(m_DestinationMod.Identifier);
            Stat depending = activeStats.GetStat(m_DependingMod.Identifier);
            StatModifier[] statModifiers = destination.GetStatModifiersBySource(this);
            if (statModifiers.Length >= 1)
            {
                statModifiers[0].Value = depending.Current / depending.Calculated * 20.0f;
            }
        }
        #endregion
    }
}
