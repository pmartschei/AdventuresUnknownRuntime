using AdventuresUnknownSDK.Core.Utils.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects
{
    [Serializable]
    public sealed class SafeLoadCheck<T> : IAdventuresUnknownSerializeCallback, IAdventuresUnknownSerializer
    {
        private T m_T;
        private bool m_Invalid = false;
        private string m_Data = null;
        #region Properties
        public T Object
        {
            get { return m_T; }
            set { m_T = value; }
        }
        public string MissingTypeName
        {
            get;
            private set;
        }

        public string MissingAssembly
        {
            get;
            private set;
        }
        public bool Invalid { get => m_Invalid; }
        #endregion

        #region Methods

        public void InitializeObject()
        {
            m_T = default(T);
            m_Invalid = false;
            m_Data = null;
        }

        public bool OnAfterDeserialize()
        {
            m_Invalid = (m_T == null);
            return true;
        }

        public bool OnBeforeSerialize()
        {
            return true;
        }

        public void Serialize(JsonTextWriter jsonTextWriter)
        {
            AdventuresUnknownSerializeUtils.SerializeObject(jsonTextWriter, "m_Invalid", m_Invalid);
            if (m_T != null || m_Data == null)
            {
                AdventuresUnknownSerializeUtils.SerializeObject(jsonTextWriter, "m_T", m_T);
            }
            else
            {
                jsonTextWriter.WritePropertyName("m_T");
                AdventuresUnknownSerializeUtils.SerializeWriteRawObject(jsonTextWriter, m_Data);
            }
        }
        public void Deserialize(JToken jToken)
        {
            FieldInfo fieldInfo = GetType().GetField("m_Invalid", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            JToken invalidToken = jToken["m_Invalid"];
            object result = AdventuresUnknownSerializeUtils.DeserializeObject(invalidToken, fieldInfo, this);
            m_Invalid = (bool)result;
            fieldInfo = GetType().GetField("m_T", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            JToken objectToken = jToken["m_T"];
            string data = objectToken.ToString(Formatting.None);
            result = AdventuresUnknownSerializeUtils.DeserializeObject(objectToken, fieldInfo, this);
            if (result == null || !(result is T))
            {
                m_Data = data;
                JObject jObject = objectToken as JObject;
                if (jObject != null)
                {
                    JToken typeToken;
                    if (jObject.TryGetValue("type", out typeToken))
                    {
                        MissingTypeName = typeToken.ToString();
                    }
                    JToken originToken;
                    if (jObject.TryGetValue("origin", out originToken))
                    {
                        MissingAssembly = originToken.ToString();
                    }
                }
            }
            else
            {
                m_T = (T)result;
            }
        }
        #endregion
    }
}
