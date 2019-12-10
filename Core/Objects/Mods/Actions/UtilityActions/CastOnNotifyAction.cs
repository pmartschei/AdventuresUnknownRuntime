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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Utility/CastOnNotifyAction", fileName = "CastOnNotifyAction.asset")]
    public class CastOnNotifyAction : MultipleBaseAction
    {
        [SerializeField] private ActiveGemIdentifier m_ActiveGemIdentifier = null;
        [SerializeField] private int m_Level = 0;
        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            if (!m_ActiveGemIdentifier.Object) return;
            if (activeStats.GetStat(ModType.Identifier).Calculated == 0.0f) return;
            float level = m_Level;
            if (level < 0)
            {
                level = (int)(activeStats.GetStat("core.modtypes.utility.level").Calculated / 5);
            }
            Entity stats = new Entity();
            stats.CopyFrom(activeStats);
            foreach (Objects.Mods.Attribute attribute in m_ActiveGemIdentifier.Object.Attributes)
            {
                stats.GetStat(attribute.ModBase.ModTypeIdentifier).AddStatModifier(new StatModifier(attribute.GetValue(level), attribute.ModBase.CalculationType, m_ActiveGemIdentifier.Object));
            }
            m_ActiveGemIdentifier.Object.Activate(activeStats.EntityBehaviour.EntityController, stats, level);
        }
        public override void Initialize(ModType modType)
        {
            base.Initialize(modType);
            m_ActiveGemIdentifier.ConsistencyCheck();
        }
    }
}
