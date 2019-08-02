using AdventuresUnknownSDK.Core.Objects.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class TagIdentifier : ObjectIdentifier
    {
        public new Tag Object
        {
            get => base.Object as Tag;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(Tag) };
        }
    }
}
