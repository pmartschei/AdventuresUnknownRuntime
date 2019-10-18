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
            m_ActiveGemIdentifier.Object.Activate(activeStats.EntityBehaviour.EntityController, activeStats, m_Level);
        }
        public override void Initialize(ModType modType)
        {
            base.Initialize(modType);
            m_ActiveGemIdentifier.ConsistencyCheck();
        }
    }
}
