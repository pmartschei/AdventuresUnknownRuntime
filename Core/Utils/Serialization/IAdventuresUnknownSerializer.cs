using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Serialization
{
    public interface IAdventuresUnknownSerializer
    {
        #region Methods
        void Serialize(JsonTextWriter jsonTextWriter);//, FieldInfo[] fieldInfos, object[] values);
        void Deserialize(JToken jObject);//, FieldInfo[] fieldInfos);
        #endregion
    }
}
