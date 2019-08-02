using AdventuresUnknownSDK.Core.Objects.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class EnemyIdentifier : ObjectIdentifier
    {
        public new Enemy Object
        {
            get => base.Object as Enemy;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(Enemy) };
        }
    }
}
