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
        [SerializeField] private ModTypeIdentifier m_ValueMod;
        [SerializeField] private ModTypeIdentifier m_DestinationMod;
       
        #region Methods
        public override void Initialize(Entity activeStats)
        {
            Stat value = activeStats.GetStat(m_ValueMod.Identifier);
            value.StatChanged = true;
            activeStats.NotifyOnStatChange(value, this);
            Stat destination = activeStats.GetStat(m_DestinationMod.Identifier);
            destination.AddStatModifier(new StatModifier(0.0f, CalculationType.Flat, this.Root));
        }

        public override void Notify(Entity activeStats,ActionContext context)
        {
            Stat value = activeStats.GetStat(m_ValueMod.Identifier);
            Stat destination = activeStats.GetStat(m_DestinationMod.Identifier);
            StatModifier statModifier = destination.GetStatModifiersBySource(this)[0];
            bool flag = false;
            switch (m_Type)
            {
                case PercentageType.Lower:
                    flag = value.Percentage < m_Percent;
                    break;
                case PercentageType.Higher:
                    flag = value.Percentage > m_Percent;
                    break;
                case PercentageType.LowerEquals:
                    flag = value.Percentage <= m_Percent;
                    break;
                case PercentageType.HigherEquals:
                    flag = value.Percentage >= m_Percent;
                    break;
                case PercentageType.Equals:
                    flag = value.Percentage == m_Percent;
                    break;
            }
            if (flag)
            {
                statModifier.Value = 1.0f;
            }
            else
            {
                statModifier.Value = 0.0f;
            }
        }
        #endregion
    }
}
