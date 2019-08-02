using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.ConditionActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/ConditionActions/CheckDescriptionAction", fileName = "CheckDescriptionAction.asset")]
    public class CheckDescriptionAction : ConditionAction
    {
        [SerializeField] private EntityType m_EntityType = EntityType.All;
        [SerializeField] private bool m_AffectMinions = true;
        [SerializeField] private bool m_AffectNonMinions = true;
        [SerializeField] private bool m_AffectPlayer = true;
        [SerializeField] private bool m_AffectEnemies = true;

        #region Properties

        #endregion

        #region Methods
        public override bool Notify(Entity activeStats, ActionContext actionContext)
        {
            EntityDescription desc = activeStats.Description;

            if (desc.IsPlayer && !m_AffectPlayer) return false;
            if (!desc.IsPlayer && !m_AffectEnemies) return false;
            if (desc.IsMinion && !m_AffectMinions) return false;
            if (!desc.IsMinion && !m_AffectNonMinions) return false;
            if (m_EntityType != EntityType.All && desc.EntityType != m_EntityType) return false;

            return true;
        }
        #endregion
    }
}
