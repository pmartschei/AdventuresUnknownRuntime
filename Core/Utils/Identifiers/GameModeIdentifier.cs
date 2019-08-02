using AdventuresUnknownSDK.Core.Objects.Events;
using AdventuresUnknownSDK.Core.Objects.GameModes;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownRuntime.Core.Utils.Identifiers
{
    [Serializable]
    public class GameModeIdentifier : ObjectIdentifier
    {
        #region Properties
        public new GameMode Object => base.Object as GameMode;
        #endregion

        #region Methods

        #endregion
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(GameMode) };
        }
    }
}
