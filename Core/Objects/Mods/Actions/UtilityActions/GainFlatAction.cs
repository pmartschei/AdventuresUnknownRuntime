using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Utility/GainFlatAction", fileName= "GainFlatAction.asset")]
    public class GainFlatAction : MultipleBaseAction
    {
        [SerializeField] private ModTypeIdentifier m_DestinationMod;
        #region Properties

        #endregion

        #region Methods
        public override void Initialize(Entity activeStats)
        {
            Stat destination = activeStats.GetStat(m_DestinationMod.Identifier);
            destination.AddStatModifier(new StatModifier(0.0f, CalculationType.FlatExtra, this.Root));
        }

        public override void Notify(Entity activeStats,ActionContext context)
        {
            Stat destination = activeStats.GetStat(m_DestinationMod.Identifier);
            Stat origin = activeStats.GetStat(ModType.Identifier);
            StatModifier[] statModifiers = destination.GetStatModifiersBySource(this);
            if (statModifiers.Length >= 1)
            {
                statModifiers[0].Value = origin.Current;
            }
        }
        #endregion
    }
}
