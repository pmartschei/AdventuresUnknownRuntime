using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.AttackActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/AttackGeneration/ApplyToAttackAction", fileName = "ApplyToAttackAction.asset")]
    public class ApplyToAttackAction : AttackGenerationAction
    {
        [SerializeField] private ActiveGem m_ActiveGem = null;
        [SerializeField] private ModTypeIdentifier m_Destination = null;
        #region Methods
        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            AttackContext attackContext = actionContext as AttackContext;
            if (attackContext == null) return;
            if (attackContext.ActiveGem != m_ActiveGem) return;
            Stat destination = activeStats.GetStat(m_Destination.Identifier);
            destination += activeStats.GetStat(ModType.Identifier);
        }
        #endregion
    }
}
