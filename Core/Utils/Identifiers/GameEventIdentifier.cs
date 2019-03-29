using AdventuresUnknownSDK.Core.Objects.Events;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownRuntime.Core.Utils.Identifiers
{
    [Serializable]
    public class GameEventIdentifier : ObjectIdentifier
    {
        #region Properties
        public new GameEvent Object => base.Object as GameEvent;
        #endregion

        #region Methods

        #endregion
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(GameEvent) };
        }
    }
}
