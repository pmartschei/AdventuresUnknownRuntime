using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.CalculationActions
{
    public abstract class CalculationAction : BaseAction
    {

        #region Properties
        public override ActionType ActionType { get => ActionTypeManager.Calculation; }
        #endregion

        #region Methods

        #endregion
    }
}
