using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI.Items
{
    public class UIDropItemStackFilter : MonoBehaviour
    {
        [SerializeField] private ItemTypeIdentifier[] m_ItemTypes = null;
        [SerializeField] private bool m_LevelRestriction = false;
        [SerializeField] private ContextDataIdentifier m_ContextData = null;


        private bool m_ValidContextData = false;
        void Start()
        {
            if (m_ContextData.ConsistencyCheck()){
                m_ValidContextData = true;
            }
            else
            {
                GameConsole.LogWarningFormat("No Valid ContextData in {0}", this);
            }
        }
        public bool Filter(ItemStack itemStack)
        {
            if (itemStack.IsEmpty) return true;
            if (m_ValidContextData && m_LevelRestriction)
            {
                if (itemStack.ItemLevel > m_ContextData.Object.Level) return false;
            }

            foreach(ItemTypeIdentifier itemType in m_ItemTypes)
            {
                if (itemType == null) continue;
                if (itemStack.Item.ItemType.Identifier.Equals(itemType.Identifier))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
