using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.UtilityActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Utility/ApplyEffectAction", fileName = "ApplyEffectAction.asset")]
    public class ApplyEffectAction : MultipleBaseAction
    {
        [SerializeField] private EffectIdentifier m_Effect = null;
        [SerializeField] private ModTypeIdentifier m_Duration = null;
        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            Entity entity = activeStats.EntityBehaviour.Entity;
            if (entity == null)
            {
                entity = activeStats;
            }
            entity.ApplyEffect(m_Effect.Identifier, activeStats.GetStat(m_Duration.Identifier).Calculated,activeStats.GetStat(ModType.Identifier).Calculated);
        }
    }
}
