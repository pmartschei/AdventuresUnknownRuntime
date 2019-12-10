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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Calculation/PercentageValueCalculationAction", fileName= "PercentageValueCalculationAction.asset")]
    public class PercentageValueCalculationAction : CalculationAction
    {
        public enum PercentageType
        {
            Lower,
            Higher,
            LowerEquals,
            HigherEquals,
            Equals
        }
        [Range(0.0f,1.0f)]
        [SerializeField] private float m_Percent = 0.5f;
        [SerializeField] private PercentageType m_Type = PercentageType.Lower;
        [SerializeField] private ModTypeIdentifier m_ValueMod = null;
        [SerializeField] private ModTypeIdentifier m_DestinationMod = null;
       
        #region Methods

        public override void Notify(Entity activeStats,ActionContext context)
        {
            Stat stat = activeStats.GetStat(m_ValueMod.Identifier);
            Stat destination = activeStats.GetStat(m_DestinationMod.Identifier);
            destination.AddStatModifier(new StatModifierByStat(stat, CalculationType.Flat, this, CalculationType.Percentage, CheckPercentage));
        }

        private float CheckPercentage(float value)
        {
            switch (m_Type)
            {
                case PercentageType.Lower:
                    if (value < m_Percent) return 1.0f;
                    break;
                case PercentageType.Higher:
                    if (value > m_Percent) return 1.0f;
                    break;
                case PercentageType.LowerEquals:
                    if (value <= m_Percent) return 1.0f;
                    break;
                case PercentageType.HigherEquals:
                    if (value >= m_Percent) return 1.0f;
                    break;
                case PercentageType.Equals:
                    if (value == m_Percent) return 1.0f;
                    break;
            }
            return 0.0f;
        }
        #endregion
    }
}
