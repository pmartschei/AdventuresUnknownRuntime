using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions
{
    [CreateAssetMenu(menuName ="AdventuresUnknown/Core/Mods/Actions/GainPercentualAction",fileName="GainPercentualAction.asset")]
    public class GainPercentualAction : CalculationAction
    {

        [SerializeField] private ModTypeIdentifier m_OriginMod;
        [SerializeField] private ModTypeIdentifier m_DestinationMod;
        #region Properties

        #endregion

        #region Methods

        #endregion
        public override void Calculate(IActiveStat iActiveStat, ModType modType)
        {
            Stat origin = iActiveStat.GetStat(m_OriginMod.Identifier);
            Stat destination = iActiveStat.GetStat(m_DestinationMod.Identifier);
            Stat value = iActiveStat.GetStat(modType.Identifier);
            destination.AddValue(origin.Calculated * value.Calculated, CalculationType.Flat);
        }
    }
}
