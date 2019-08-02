using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.UtilityActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Utility/NotifyAction", fileName = "NotifyAction.asset")]
    public class NotifyAction : MultipleBaseAction
    {

        [SerializeField] private ActionType m_NotifyType = null;


        #region Properties

        #endregion

        #region Methods

        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            activeStats.Notify(m_NotifyType);
        }
        #endregion
    }
}
