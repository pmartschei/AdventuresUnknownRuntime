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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/HitGeneration/ApplyHitGenerationAction", fileName = "ApplyHitGenerationAction.asset")]
    public class ApplyHitGenerationAction : HitGenerationAction
    {
        [SerializeField] private ModTypeIdentifier m_Source = null;
        [SerializeField] private ModTypeIdentifier m_Target = null;
        [SerializeField] private HitType m_Type = HitType.DefensiveEntity;
        
        #region Methods

        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            HitContext hitContext = actionContext as HitContext;
            if (hitContext == null) return;
            Entity entity = hitContext.DefensiveEntity;
            if (m_Type == HitType.OffensiveEntity)
            {
                entity = hitContext.OffensiveEntity;
            }
            Stat target = entity.GetStat(m_Target.Identifier);
            Stat source = activeStats.GetStat(m_Source.Identifier);
            target.AddStatModifier(new StatModifier(source.Flat, CalculationType.Flat, this));
            target.AddStatModifier(new StatModifier(source.Increased - 1.0f, CalculationType.Increased, this));
            target.AddStatModifier(new StatModifier(source.More - 1.0f, CalculationType.More, this));
            target.AddStatModifier(new StatModifier(source.FlatExtra, CalculationType.FlatExtra, this));
        }

        #endregion
    }
}
