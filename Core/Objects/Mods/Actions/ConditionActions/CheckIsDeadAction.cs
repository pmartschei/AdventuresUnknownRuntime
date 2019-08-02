using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.ConditionActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/ConditionActions/CheckIsDeadAction", fileName = "CheckIsDeadAction.asset")]
    public class CheckIsDeadAction : ConditionAction
    {
        [SerializeField] private bool m_Value = false;

        #region Properties

        #endregion

        #region Methods

        public override bool Notify(Entity activeStats, ActionContext actionContext)
        {
            return (activeStats.IsDead == m_Value);
        }
        #endregion
    }
}
