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
        public override void Notify(Entity activeStats,ActionContext context)
        {
            Stat destination = activeStats.GetStat(m_DestinationMod.Identifier);
            Stat origin = activeStats.GetStat(m_OriginMod.Identifier);
            Stat stat = activeStats.GetStat(ModType.Identifier);

            destination.AddStatModifier(new StatModifierByStat(stat, CalculationType.Flat, this, CalculationType.Calculated, (value) => { return value * origin.Calculated; }));
        }
        #endregion
    }
}
