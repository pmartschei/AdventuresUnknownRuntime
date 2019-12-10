using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Utils.UnityEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Objects.Datas
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Datas/HotkeyData", fileName = "HotkeyData.asset")]
    public class HotkeyData : IPlayerData
    {
        [NonSerialized] private Dictionary<string, HotkeyDisplayData> m_DisplayDatas = new Dictionary<string, HotkeyDisplayData>();

        [SerializeField] [HideInInspector] private HotkeyDisplayData[] m_DisplayDataList = null;

        #region Properties

        #endregion

        #region Methods
        public override void Load()
        {
            HotkeyData hkd = FindScriptableObject<HotkeyData>();
            if (!hkd) return;
            hkd.m_DisplayDatas.Clear();

            if (m_DisplayDataList == null) return;
            foreach(HotkeyDisplayData hkDisplayData in m_DisplayDataList)
            {
                hkd.m_DisplayDatas.Add(hkDisplayData.Identifier, hkDisplayData);
            }
        }

        public override void Reset()
        {
            m_DisplayDatas = new Dictionary<string, HotkeyDisplayData>();
        }

        public override bool OnBeforeSerialize()
        {
            m_DisplayDataList = m_DisplayDatas.Values.ToArray();
            return true;
        }
        public HotkeyDisplayData GetHotkeyDisplayData(string identifier)
        {
            HotkeyDisplayData data;
            if (m_DisplayDatas.TryGetValue(identifier, out data))
            {
                return data;
            }
            return null;
        }
        public void PutHotkeyDisplayData(string identifier,HotkeyDisplayData data)
        {
            if (m_DisplayDatas.ContainsKey(identifier))
            {
                m_DisplayDatas[identifier] = data;
            }
            else
            {
                m_DisplayDatas.Add(identifier, data);
            }
        }

        #endregion

    }
}
