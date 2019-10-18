using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.CooldownActions
{
    public abstract class AttackCooldownApplyAction : BaseAction
    {

        #region Properties
        public override ActionType ActionType { get => ActionTypeManager.AttackCooldownApply; }
        #endregion

        #region Methods

        #endregion
    }
}
