using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.DropTables;
using AdventuresUnknownSDK.Core.Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class DropTableIdentifier : ObjectIdentifier
    {
        public new DropTable Object
        {
            get => base.Object as DropTable;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(DropTable) };
        }
    }
}
