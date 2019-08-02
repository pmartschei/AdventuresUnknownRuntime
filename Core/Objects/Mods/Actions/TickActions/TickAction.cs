using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.TickActions
{
    public abstract class TickAction : BaseAction
    {


        #region Properties
        public override ActionType ActionType { get => ActionTypeManager.Tick; }
        #endregion

        #region Methods

        #endregion
    }
}
