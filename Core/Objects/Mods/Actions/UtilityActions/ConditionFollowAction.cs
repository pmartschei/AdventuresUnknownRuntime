using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.UtilityActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Utility/ConditionFollowAction", fileName = "ConditionFollowAction.asset")]
    public class ConditionFollowAction : MultipleBaseAction
    {
        [SerializeField] private ConditionAction m_ConditionAction = null;
        [SerializeField] private bool m_Inverted = false;
        [SerializeField] private BaseAction m_FollowAction = null;

        #region Properties

        #endregion

        #region Methods
        public override void Initialize(ModType modType)
        {
            base.Initialize(modType);
            if (m_FollowAction)
            {
                m_FollowAction.Initialize(modType);
                m_FollowAction.Root = this;
            }
            if (m_ConditionAction)
            m_ConditionAction.Initialize(modType);
        }

        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            if (!m_FollowAction)
            {
                return;
            }
            if (m_ConditionAction)
            {
                bool condition = m_ConditionAction.Notify(activeStats, actionContext);
                if (m_Inverted)
                {
                    condition = !condition;
                }
                if (!condition) return;
            }
            m_FollowAction.Notify(activeStats, actionContext);
        }
        #endregion
    }
}
