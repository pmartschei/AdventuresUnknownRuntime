using AdventuresUnknownSDK.Core.Objects.Mods.Actions;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class ProtectionCauseManager : SingletonBehaviour<ProtectionCauseManager>
    {

        #region Properties

        public static ProtectionCause Block                { get => Instance.BlockImpl; set => Instance.BlockImpl = value; }

        protected abstract ProtectionCause BlockImpl { get; set; }
        #endregion

        #region Methods

        #endregion
    }
}
