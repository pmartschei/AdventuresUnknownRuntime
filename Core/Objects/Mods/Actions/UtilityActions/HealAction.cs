using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.UtilityActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Utility/HealAction", fileName = "HealAction.asset")]
    public class HealAction : MultipleBaseAction
    {
        [SerializeField] private ModTypeIdentifier m_Source = null;
        [SerializeField] private ModTypeIdentifier m_Target = null;
        #region Properties

        #endregion

        #region Methods

        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            Entity entity = activeStats.EntityBehaviour.Entity;
            if (entity == null)
            {
                entity = activeStats;
            }
            Stat target = entity.GetStat(m_Target.Identifier);
            Stat source = activeStats.GetStat(m_Source.Identifier);
            target.Current += source.Calculated;
        }
        #endregion
    }
}
