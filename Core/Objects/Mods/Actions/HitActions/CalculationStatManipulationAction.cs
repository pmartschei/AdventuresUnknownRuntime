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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/HitCalculation/CalculationStatManipulationAction", fileName = "CalculationStatManipulationAction.asset")]
    public class CalculationStatManipulationAction : HitCalculationAction
    {
        [SerializeField] private CalculationType m_SourceCalculationType = CalculationType.Calculated;
        [SerializeField] private ModTypeIdentifier m_Target = null;
        [SerializeField] private CalculationType m_TargetCalculationType = CalculationType.Flat;
        [SerializeField] private OpType m_OpType = OpType.Source;
        [Tooltip("Value will only be used if the OpType is not OpType.Source")]
        [SerializeField] private float m_Value = 0.0f;

        public enum OpType
        {
            Source,
            ValueMinus,
            MinusValue,
            PlusValue,
            TimesValue,
        }
        #region Properties

        #endregion

        #region Methods
        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            HitContext hitContext = actionContext as HitContext;
            if (hitContext == null) return;
            if (hitContext.IsProtected) return;
            float stat = activeStats.GetStat(ModType.Identifier).GetValue(m_SourceCalculationType);
            switch (m_OpType)
            {
                case OpType.MinusValue:
                    stat = stat - m_Value;
                    break;
                case OpType.ValueMinus:
                    stat = m_Value - stat;
                    break;
                case OpType.PlusValue:
                    stat = stat + m_Value;
                    break;
                case OpType.TimesValue:
                    stat = stat * m_Value;
                    break;
            }
            hitContext.OffensiveEntity.GetStat(m_Target.Identifier).AddStatModifier(
                new StatModifier(stat, m_TargetCalculationType, this));
        }
        #endregion
    }
}
