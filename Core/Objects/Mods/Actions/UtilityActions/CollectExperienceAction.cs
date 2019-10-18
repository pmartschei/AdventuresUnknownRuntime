using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.DropTables;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.UtilityActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Utility/CollectExperienceAction", fileName = "CollectExperienceAction.asset")]
    public class CollectExperienceAction : MultipleBaseAction
    {
        [SerializeField] private ModTypeIdentifier m_Source = null;
        [SerializeField] private ContextDataIdentifier m_ContextData = null;

        #region Properties

        #endregion

        #region Methods

        #endregion

        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            if (m_ContextData.Object)
            {
                float experience = activeStats.GetStat(m_Source.Identifier).Calculated;
                m_ContextData.Object.Experience += (int)experience;
                //TODO Notify Player Entity with ExperienceGain and collect additional modifiers
            }
        }
        public override void Initialize(ModType modType)
        {
            base.Initialize(modType);
            m_ContextData.ConsistencyCheck();
        }
    }
}
