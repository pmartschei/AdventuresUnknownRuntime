using AdventuresUnknownSDK.Core.Logic.ActiveGemContainers;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using System;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Datas
{
    [Serializable]
    public class HotkeyDisplayData
    {
        [SerializeField] private string m_Identifier = "";
        [SerializeField] private string m_ContainerName = "";
        [SerializeField] private int m_Slot = -1;

        [NonSerialized] private ActiveGem m_ActiveGem = null;
        [NonSerialized] private GenericActiveGemContainer m_Container = null;

        public string ContainerName { get => m_ContainerName; set => m_ContainerName = value; }
        public ActiveGem ActiveGem { get => m_ActiveGem; set => m_ActiveGem = value; }
        public int Slot { get => m_Slot; set => m_Slot = value; }
        public string Identifier { get => m_Identifier; set => m_Identifier = value; }
        public GenericActiveGemContainer Container { get => m_Container; set => m_Container = value; }
    }
}