using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Serialization
{
    public interface IAdventuresUnknownSerializeCallback
    {
        #region Methods
        void InitializeObject();
        bool OnBeforeSerialize();
        bool OnAfterDeserialize();
        #endregion
    }
}
