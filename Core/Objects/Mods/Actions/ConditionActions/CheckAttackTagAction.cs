using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.ConditionActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/ConditionActions/CheckAttackTagAction", fileName = "CheckAttackTagAction.asset")]
    public class CheckAttackTagAction : ConditionAction
    {
        [SerializeField] private TagIdentifier[] m_SupportedAttackTags = null;
        #region Methods

        public override bool Notify(Entity activeStats, ActionContext actionContext)
        {
            AttackContext attackContext = actionContext as AttackContext;
            if (attackContext == null || attackContext.ActiveGem == null) return false;

            string[] attackTags = attackContext.ActiveGem.Tags.ToArray();
            foreach(TagIdentifier tagIdentifier in m_SupportedAttackTags)
            {
                if (attackTags.Contains(tagIdentifier.Identifier)) return true;
            }
            
            return false;
        }
        #endregion
    }
}
