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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/HitCalculation/MathHitCalculationAction", fileName = "MathHitCalculationAction.asset")]
    public class MathHitCalculationAction : HitCalculationAction
    {
        [SerializeField] private ModTypeIdentifier m_Target = null;
        [SerializeField] private MathType m_Type = MathType.Min;


        public enum MathType
        {
            Min,
            Max,
        }
        #region Properties

        #endregion

        #region Methods

        #endregion
        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            HitContext hitContext = actionContext as HitContext;
            if (hitContext == null) return;
            if (hitContext.IsProtected) return;
            float sourceCalculated = activeStats.GetStat(ModType.Identifier).Calculated;//0,8
            Stat targetStat = activeStats.GetStat(m_Target.Identifier);// 0,75
            switch (m_Type)
            {
                case MathType.Min:
                    if (sourceCalculated >= targetStat.Calculated) return;
                    break;
                case MathType.Max:
                    if (sourceCalculated <= targetStat.Calculated) return;
                    break;
            }
            sourceCalculated = sourceCalculated / targetStat.Calculated;
            targetStat.AddStatModifier(
                new StatModifier(sourceCalculated - 1.0f, CalculationType.More, this));
        }
    }
}
