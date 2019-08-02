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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Utility/SetEntityDeadAction", fileName = "SetEntityDeadAction.asset")]
    public class SetEntityDeadAction : MultipleBaseAction
    {
        [SerializeField] private bool m_Value = false;

        #region Properties

        #endregion

        #region Methods

        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            activeStats.IsDead = m_Value;
        }

        #endregion
    }
}
