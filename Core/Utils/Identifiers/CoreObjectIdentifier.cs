using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class CoreObjectIdentifier : ObjectIdentifier
    {
        private Type[] m_TypesToCheck;

        public CoreObjectIdentifier(params Type[] types) 
        {
            m_TypesToCheck = types;
        }
        public override Type[] GetSupportedTypes()
        {
            return m_TypesToCheck;
        }
    }
}
