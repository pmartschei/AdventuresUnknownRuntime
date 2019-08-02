using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Datas;
using AdventuresUnknownSDK.Core.Objects.GameModes;
using AdventuresUnknownSDK.Core.Utils.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknown.Utils
{
    [Serializable]
    public class FileObject : IAdventuresUnknownSerializeCallback
    {
        private List<SafeLoadCheck<IPlayerData>> m_DataList = new List<SafeLoadCheck<IPlayerData>>();
        [NonSerialized]
        private List<SafeLoadCheck<IPlayerData>> m_CorruptedDataList = new List<SafeLoadCheck<IPlayerData>>();
        [NonSerialized]
        private Dictionary<string,IPlayerData> m_Data = new Dictionary<string, IPlayerData>();

        #region Properties

        #endregion

        #region Methods

        public void InitializeObject()
        {
            m_DataList = new List<SafeLoadCheck<IPlayerData>>();
            m_Data = new Dictionary<string, IPlayerData>();
            m_CorruptedDataList = new List<SafeLoadCheck<IPlayerData>>();
        }

        public void Load()
        {
            foreach(var data in m_Data.Values)
            {
                ContextData contextData = data as ContextData;
                if (contextData)
                {
                    PlayerManager.CurrentSaveFileName = contextData.SaveFileName;
                }
                data.Load();
            }
        }

        public void Delete()
        {
            ContextData cd = Get("core.datas.context") as ContextData;
            if (cd == null) return;
            GameMode gameMode = ObjectsManager.FindObjectByIdentifier<GameMode>(cd.GameMode);
            if (gameMode == null) return;
            PlayerManager.Delete(gameMode.FolderName + "/" + cd.SaveFileName);
        }
        public void Clear()
        {
            m_Data.Clear();
        }
        public IPlayerData Get(string key)
        {
            if (!m_Data.ContainsKey(key)) return null;
            return m_Data[key];
        }

        public bool OnAfterDeserialize()
        {
            if (m_Data == null)
                m_Data = new Dictionary<string, IPlayerData>();
            m_CorruptedDataList.Clear();
            m_Data.Clear();
            foreach (SafeLoadCheck<IPlayerData> dataItem in m_DataList)
            {
                if (dataItem.Invalid)
                {
                    m_CorruptedDataList.Add(dataItem);
                    continue;
                }
                IPlayerData playerData = ObjectsManager.FindObjectByIdentifier<IPlayerData>(dataItem.Object.Identifier);
                if (playerData == null)
                {
                    m_CorruptedDataList.Add(dataItem);
                    continue;
                }
                m_Data.Add(dataItem.Object.Identifier, dataItem.Object);
            }
            return true;
        }

        public bool OnBeforeSerialize()
        {
            m_DataList.Clear();
            foreach (string key in m_Data.Keys)
            {
                IPlayerData playerData = m_Data[key];
                SafeLoadCheck<IPlayerData> safeLoadCheck = new SafeLoadCheck<IPlayerData>();
                safeLoadCheck.Object = playerData;
                m_DataList.Add(safeLoadCheck);
            }
            foreach (SafeLoadCheck<IPlayerData> dataItem in m_CorruptedDataList)
            {
                m_DataList.Add(dataItem);
            }
            return true;
        }

        public void Put(string key,IPlayerData playerData)
        {
            if (m_Data.ContainsKey(key))
            {
                m_Data[key] = playerData;
            }
            else
            {
                m_Data.Add(key, playerData);
            }
        }
        
        #endregion
    }
}
