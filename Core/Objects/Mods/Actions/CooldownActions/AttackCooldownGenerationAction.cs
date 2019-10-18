using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.CooldownActions
{
    public abstract class AttackCooldownGenerationAction : BaseAction
    {

        #region Properties
        public override ActionType ActionType { get => ActionTypeManager.AttackCooldownGeneration; }
        #endregion

        #region Methods

        #endregion
    }
}
