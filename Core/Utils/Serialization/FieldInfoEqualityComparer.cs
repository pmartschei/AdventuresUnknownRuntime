using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Serialization
{
    public class FieldInfoEqualityComparer : IEqualityComparer<FieldInfo>
    {
        public bool Equals(FieldInfo x, FieldInfo y)
        {
            if (y == null && x == null) return true;
            if (y == null || x == null) return false;
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(FieldInfo obj)
        {
            return obj.FieldHandle.Value.ToInt32();
        }
    }
}
