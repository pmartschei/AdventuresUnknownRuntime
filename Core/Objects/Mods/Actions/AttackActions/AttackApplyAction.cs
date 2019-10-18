using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.AttackActions
{
    public abstract class AttackApplyAction : BaseAction
    {

        #region Properties
        public override ActionType ActionType { get => ActionTypeManager.AttackApply; }
        #endregion

        #region Methods

        #endregion
    }
}
