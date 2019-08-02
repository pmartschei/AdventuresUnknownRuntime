using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.AttackActions
{
    public abstract class AttackGenerationAction : BaseAction
    {

        #region Properties
        public override ActionType ActionType { get => ActionTypeManager.AttackGeneration; }
        #endregion

        #region Methods

        #endregion
    }
}
