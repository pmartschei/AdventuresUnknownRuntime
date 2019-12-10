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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Utility/MultiAction", fileName = "MultiAction.asset")]
    public class MultiAction : MultipleBaseAction
    {
        [SerializeField] private BaseAction[] m_Actions = null;

        #region Properties

        #endregion

        #region Methods

        #endregion
        public override void Initialize(ModType modType)
        {
            base.Initialize(modType);
            foreach (BaseAction action in m_Actions)
            {
                if (!action) continue;
                action.Initialize(modType);
                action.Root = this;
            }
        }

        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            foreach(BaseAction action in m_Actions)
            {
                if (!action) continue;
                action.Notify(activeStats, actionContext);
            }
        }
    }
}
