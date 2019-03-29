using AdventuresUnknownSDK.Core.Objects.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class EnemyTypeIdentifier : ObjectIdentifier
    {
        public new EnemyType Object
        {
            get => base.Object as EnemyType;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(EnemyType) };
        }
    }
}
