using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.HitActions
{
    public abstract class HitCalculationAction : BaseAction
    {

        #region Properties
        public override ActionType ActionType { get => ActionTypeManager.HitCalculation; }
        #endregion

        #region Methods

        #endregion
    }
}
