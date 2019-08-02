using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Utils.Serialization
{
    public static class AdventuresUnknownSerializeUtils
    {

        public static FieldInfo[] GetSerializableFields(Type type)
        {
            if (type.IsInterface || type == typeof(object) || type == typeof(ScriptableObject)) return new FieldInfo[0];
            return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                      .Where(f => (f.Attributes & FieldAttributes.NotSerialized) != FieldAttributes.NotSerialized)
                      .Concat(GetSerializableFields(type.BaseType)).Distinct(new FieldInfoEqualityComparer()).ToArray();
        }

        public static void SerializeWriteRawObject(JsonTextWriter jsonTextWriter, string value)
        {
            JToken jToken = JToken.Parse(value);
            JObject jObject = jToken as JObject;
            JValue jValue = jToken as JValue;
            JArray jArray = jToken as JArray;
            if (jValue != null)
            {
                if (jValue.Value != null)
                {
                    jsonTextWriter.WriteValue(jValue.Value);
                }
                else
                {
                    jsonTextWriter.WriteNull();
                }
                return;
            }
            if (jArray != null)
            {
                jsonTextWriter.WriteStartArray();

                for (int i = 0; i < jArray.Count; i++)
                {
                    SerializeWriteRawObject(jsonTextWriter, jArray[i].ToString(Formatting.None));
                }

                jsonTextWriter.WriteEndArray();
                return;
            }
            if (jObject == null)
            {
                jsonTextWriter.WriteRawValue(value);
                return;
            }

            IList<string> keys = jObject.Properties().Select(p => p.Name).ToList();

            jsonTextWriter.WriteStartObject();
            foreach (string key in keys)
            {
                jsonTextWriter.WritePropertyName(key);
                SerializeWriteRawObject(jsonTextWriter,jObject[key].ToString(Formatting.None));
            }
            jsonTextWriter.WriteEndObject();
        }

        public static bool SerializeArrayNoPropertyName(JsonTextWriter jsonTextWriter, object value)
        {
            Type type = value.GetType();
            if (!type.IsArray) return false;
            jsonTextWriter.WriteStartArray();
            Array arr = value as Array;
            if (arr != null)
            {
                foreach (object o in arr)
                {
                    if (o == null)
                    {
                        jsonTextWriter.WriteNull();
                    }
                    else
                    {
                        if (SerializeArrayNoPropertyName(jsonTextWriter, o)) continue;
                        if (SerializePrimitiveNoPropertyName(jsonTextWriter, o)) continue;
                        SerializeFull(jsonTextWriter, o);
                    }
                }
            }
            jsonTextWriter.WriteEndArray();
            return true;
        }

        public static bool SerializePrimitiveNoPropertyName(JsonTextWriter jsonTextWriter, object value)
        {
            Type type = value.GetType();
            if (!type.IsPrimitive && type != typeof(string)) return false;
            try
            {
                jsonTextWriter.WriteValue(value);
            }
            catch //value is not supported (IntPtr)?!
            {
                jsonTextWriter.WriteNull();
            }
            return true;
        }

        public static bool SerializeObjectNoPropertyName(JsonTextWriter jsonTextWriter,object value)
        {
            if (SerializeArrayNoPropertyName(jsonTextWriter, value)) return true;
            if (SerializePrimitiveNoPropertyName(jsonTextWriter, value)) return true;
            if (value == null)
            {
                jsonTextWriter.WriteNull();
                return true;
            }
            SerializeFull(jsonTextWriter, value);
            return true;
        }

        public static bool SerializeArray(JsonTextWriter jsonTextWriter, string name, object value)
        {
            if (value == null) return false;
            Type type = value.GetType();
            if (!type.IsArray) return false;
            jsonTextWriter.WritePropertyName(name);
            SerializeArrayNoPropertyName(jsonTextWriter, value);
            return true;
        }
        public static object DeserializeArray(JToken jToken, FieldInfo fieldInfo, object objInstance)
        {
            if (!fieldInfo.FieldType.IsArray) return null;
            JArray arrayToken = jToken as JArray;
            Array array = Array.CreateInstance(fieldInfo.FieldType.GetElementType(), arrayToken.Count);
            for (int i = 0; i < arrayToken.Count; i++)
            {
                JObject arrayObject = arrayToken[i] as JObject;
                JValue arrayValue = arrayToken[i] as JValue;
                JArray arrayArray = arrayToken[i] as JArray;
                if (arrayArray != null)
                {
                    array.SetValue(DeserializeArray(arrayArray, fieldInfo, objInstance), i);
                    continue;
                }
                if (arrayValue != null && arrayValue.Value != null)
                {
                    array.SetValue(Convert.ChangeType(arrayValue.Value, arrayValue.Type.GetTypeCode()), i);
                    continue;
                }
                if (arrayObject != null)
                {
                    array.SetValue(DeserializeFull(arrayObject), i);
                    continue;
                }
            }
            fieldInfo.SetValue(objInstance, array);
            return array;
        }
        public static bool SerializePrimitive(JsonTextWriter jsonTextWriter, string name, object value)
        {
            if (value == null) return false;
            Type type = value.GetType();
            if (!type.IsPrimitive && type != typeof(string)) return false;

            jsonTextWriter.WritePropertyName(name);
            try
            {
                jsonTextWriter.WriteValue(value);
            }
            catch //value is not supported (IntPtr)?!
            {
                jsonTextWriter.WriteNull();
            }
            return true;
        }

        public static object DeserializePrimitive(JToken jToken, FieldInfo fieldInfo, object objInstance)
        {
            if (!fieldInfo.FieldType.IsPrimitive && fieldInfo.FieldType != typeof(string)) return null;
            JValue value = jToken as JValue;
            if (value != null && value.Value != null)
            {
                object result = Convert.ChangeType(value.Value, fieldInfo.FieldType);
                fieldInfo.SetValue(objInstance, result);
                return result;
            }
            //else its null/default
            return null;
        }

        public static bool SerializeObject(JsonTextWriter jsonTextWriter, string name, object value)
        {
            if (SerializeArray(jsonTextWriter, name, value)) return true;
            if (SerializePrimitive(jsonTextWriter, name, value)) return true;
            jsonTextWriter.WritePropertyName(name);
            if (value == null)
            {
                jsonTextWriter.WriteNull();
                return true;
            }
            SerializeFull(jsonTextWriter, value);
            return true;
        }

        public static object DeserializeObject(JToken jToken, FieldInfo fieldInfo, object objInstance)
        {
            object result = DeserializeArray(jToken, fieldInfo, objInstance);
            if (result != null) return result;
            result = DeserializePrimitive(jToken, fieldInfo, objInstance);
            if (result != null) return result;

            JObject memberObject = jToken as JObject;
            if (memberObject != null)
            {
                result = DeserializeFull(memberObject);
                fieldInfo.SetValue(objInstance, result);
                return result;
            }
            //else its null/default
            return null;
        }

        public static object DeserializeFull(JObject jObject)
        {
            object result = DeserializeStart(jObject);
            if (result != null)
            {
                Type type = result.GetType();
                JToken contentToken = jObject["content"];

                FieldInfo[] members = GetSerializableFields(type);

                IAdventuresUnknownSerializer auSerializer = result as IAdventuresUnknownSerializer;
                if (auSerializer != null)
                {
                    auSerializer.Deserialize(contentToken);//, members);
                }
                else
                {
                    foreach (FieldInfo member in members)
                    {
                        DeserializeObject(contentToken[member.Name], member, result);
                    }
                }
                IAdventuresUnknownSerializeCallback auSerializeCB = result as IAdventuresUnknownSerializeCallback;
                if (auSerializeCB != null) auSerializeCB.OnAfterDeserialize();
            }
            return result;
        }

        private static object DeserializeStart(JObject jObject)
        {
            JToken typeToken = jObject["type"];
            JToken originToken = jObject["origin"];
            string typeString = typeToken.Value<string>();
            string assemblyString = originToken.Value<string>();
            string combined = Assembly.CreateQualifiedName(assemblyString, typeString);
            Type type = Type.GetType(combined);
            if (type == null) return null;
            object objInstance = null;
            if (type.IsSubclassOf(typeof(ScriptableObject)))
            {
                objInstance = ScriptableObject.CreateInstance(type);
            }
            if (objInstance == null)
            {
                objInstance = FormatterServices.GetSafeUninitializedObject(type);
            }
            if (objInstance == null) return null;
            IAdventuresUnknownSerializeCallback auSerializeCB = objInstance as IAdventuresUnknownSerializeCallback;
            if (auSerializeCB != null) auSerializeCB.InitializeObject();
            return objInstance;
        }

        public static void SerializeFull(JsonTextWriter jsonTextWriter, object obj)
        {
            SerializeStart(jsonTextWriter, obj);
            Type type = obj.GetType();
            IAdventuresUnknownSerializer auSerializer = obj as IAdventuresUnknownSerializer;
            FieldInfo[] members = GetSerializableFields(type);
            object[] values = FormatterServices.GetObjectData(obj, members);
            if (auSerializer != null)
            {
                auSerializer.Serialize(jsonTextWriter);//, members, values);
            }
            else
            {
                for (int i = 0; i < values.Length; ++i)
                {
                    SerializeObject(jsonTextWriter, members[i].Name, values[i]);
                }
            }
            SerializeEnd(jsonTextWriter, obj);
        }
        public static void SerializeStart(JsonTextWriter jsonTextWriter, object obj)
        {
            IAdventuresUnknownSerializeCallback auSerializeCB = obj as IAdventuresUnknownSerializeCallback;
            if (auSerializeCB != null) auSerializeCB.OnBeforeSerialize();
            Type type = obj.GetType();

            jsonTextWriter.WriteStartObject();
            jsonTextWriter.WritePropertyName("type");
            jsonTextWriter.WriteValue(type.FullName);
            jsonTextWriter.WritePropertyName("origin");
            jsonTextWriter.WriteValue(Assembly.GetAssembly(type).GetName().Name);
            jsonTextWriter.WritePropertyName("content");
            jsonTextWriter.WriteStartObject();
        }
        public static void SerializeEnd(JsonTextWriter jsonTextWriter, object obj)
        {
            jsonTextWriter.WriteEndObject();
            jsonTextWriter.WriteEndObject();
        }
    }
}
