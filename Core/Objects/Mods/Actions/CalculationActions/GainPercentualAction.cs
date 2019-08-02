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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Calculation/GainPercentualAction", fileName="GainPercentualAction.asset")]
    public class GainPercentualAction : CalculationAction
    {

        [SerializeField] private ModTypeIdentifier m_OriginMod;
        [SerializeField] private ModTypeIdentifier m_DestinationMod;
        
        #region Methods
        public override void Initialize(Entity activeStats)
        {
            Stat destination = activeStats.GetStat(m_DestinationMod.Identifier);
            Stat origin = activeStats.GetStat(m_OriginMod.Identifier);
            destination.AddStatModifier(new StatModifier(0.0f, CalculationType.FlatExtra, this.Root));
            activeStats.NotifyOnStatChange(origin, this);
        }

        public override void Notify(Entity activeStats,ActionContext context)
        {
            Stat destination = activeStats.GetStat(m_DestinationMod.Identifier);
            Stat origin = activeStats.GetStat(m_OriginMod.Identifier);
            Stat value = activeStats.GetStat(ModType.Identifier);
            StatModifier[] statModifiers = destination.GetStatModifiersBySource(this);
            if (statModifiers.Length >= 1)
            {
                statModifiers[0].Value = origin.Calculated * value.Calculated;
            }
        }
        #endregion
    }
}
