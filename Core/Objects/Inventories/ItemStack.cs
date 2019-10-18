using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Items.Actions;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Mods.ModBases;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using AdventuresUnknownSDK.Core.Utils.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Objects.Inventories
{
    [Serializable]
    public class ItemStack : IActiveStat
    {
        [Serializable]
        public class ValueMod : IAdventuresUnknownSerializeCallback
        {
            [SerializeField] private float m_Value;
            [SerializeField] private ModIdentifier m_ModIdentifier = new ModIdentifier();
            [SerializeField] private BasicModBaseIdentifier m_BasicModBaseIdentifier = new BasicModBaseIdentifier();

            public float Value { get => m_Value; set => m_Value = value; }
            public BasicModBase BasicModBase { get => m_BasicModBaseIdentifier.Object; }
            public string Identifier { get => m_BasicModBaseIdentifier.Identifier; set => m_BasicModBaseIdentifier.Identifier = value; }
            public Mod Mod { get => m_ModIdentifier.Object;
                set {
                    if (value == null)
                    {
                        m_ModIdentifier.Identifier = "";
                    }
                    else
                    {
                        m_ModIdentifier.Identifier = value.Identifier;
                    }
                }
            }

            internal ModIdentifier ModIdentifier { get => m_ModIdentifier; }
            internal BasicModBaseIdentifier BasicModBaseIdentifier { get => m_BasicModBaseIdentifier; }

            public void InitializeObject()
            {
                m_Value = 0.0f;
                m_ModIdentifier = new ModIdentifier();
                m_BasicModBaseIdentifier = new BasicModBaseIdentifier();
            }

            public bool OnAfterDeserialize()
            {
                return true;
            }

            public bool OnBeforeSerialize()
            {
                return true;
            }
        }
        public static ItemStack Empty = new ItemStack(null, 0);

        [SerializeField] private ItemIdentifier m_ItemIdentifier = new ItemIdentifier();
        [SerializeField] private int m_PowerLevel = 0;
        [SerializeField] private int m_Amount = 0;
        [SerializeField] private int m_ItemLevel = 0;
        [SerializeField] private ValueMod[] m_ImplicitMods = new ValueMod[0];
        [SerializeField] private ValueMod[] m_ExplicitMods = new ValueMod[0];

        [NonSerialized] private UnityEvent m_ItemChangeEvent = new UnityEvent();
        [NonSerialized] private string m_ItemStackName = "";
        [NonSerialized] private bool m_NameDataChanged = false;
        [NonSerialized] private string m_LastItemName = "";
        [NonSerialized] private Dictionary<string,Stat> m_HashedStats = new Dictionary<string, Stat>();

        [NonSerialized] private List<Entity> m_RegisteredEntities = new List<Entity>();

        public ItemStack(string identifier) : this(identifier,1)
        {

        }
        public ItemStack(string identifier, int amount)
        {
            if (identifier == null)
                identifier = "";
            m_ItemIdentifier.Identifier = identifier;
            if (Item)
            {
                Item.PowerLevelItemStackChange(this);
            }
            this.m_Amount = amount;
            m_NameDataChanged = true;
        }


        #region Properties

        public Item Item
        {
            get
            {
                return m_ItemIdentifier.Object;
            }
        }
        public string ItemIdentifier
        {
            get
            {
                return m_ItemIdentifier.Identifier;
            }
            set
            {
                if (!m_ItemIdentifier.Identifier.Equals(value))
                {
                    ForceUpdate();
                    m_ItemIdentifier.Identifier = value;
                    OnItemChange.Invoke();
                }
            }
        }
        public int Amount
        {
            get => m_Amount;
            set
            {
                if (m_Amount != value)
                {
                    m_Amount = value;
                    ForceUpdate();
                    OnItemChange.Invoke();
                }
            }
        }
        public int PowerLevel
        {
            get => m_PowerLevel;
            set
            {
                if (m_PowerLevel != value)
                {
                    m_PowerLevel = value;
                    if (Item)
                        Item.PowerLevelItemStackChange(this);
                    ForceUpdate();
                    OnItemChange.Invoke();
                }
            }
        }
        public string ItemStackName
        {
            get
            {
                UpdateIfChanged();
                return m_ItemStackName;
            }
        }
        
        public bool IsEmpty { get => m_ItemIdentifier.Identifier.Equals("") || m_ItemIdentifier.Object == null; }
        public ValueMod[] ImplicitMods { get => m_ImplicitMods;
             set
            {
                m_ImplicitMods = value;
                ChangeModifiersAll();
                OnItemChange.Invoke();
            }
        }
        public ValueMod[] ExplicitMods { get => m_ExplicitMods;
            set
            {
                m_ExplicitMods = value;
                ChangeModifiersAll();
                OnItemChange.Invoke();
            }
        }

        public UnityEvent OnItemChange {
            get
            {
                if (m_ItemChangeEvent == null)
                {
                    m_ItemChangeEvent = new UnityEvent();
                }
                return m_ItemChangeEvent;
            }
        }
        public List<Entity> RegisteredEntities
        {
            get
            {
                if (m_RegisteredEntities == null)
                {
                    m_RegisteredEntities = new List<Entity>();
                }
                return m_RegisteredEntities;
            }
        }
        public int ItemLevel { get => m_ItemLevel; set => m_ItemLevel = value; }

        #endregion

        #region Methods

        private void UpdateIfChanged()
        {
            if (m_LastItemName == null || !m_LastItemName.Equals(Item.ItemName))
            {
                //ForceUpdate();
                m_NameDataChanged = true;
                m_LastItemName = Item.ItemName;
            }
            if (m_NameDataChanged)
            {
                string newItemStackName = "No ItemStack Name";
                if (Item != null)
                {
                    StringBuilder sb = new StringBuilder();
                    if (m_Amount > 1)
                        sb.AppendFormat("{0}x ", m_Amount);
                    sb.Append(Item.ItemName);
                    if (m_PowerLevel > 0)
                        sb.AppendFormat(" +{0}", m_PowerLevel);
                    newItemStackName = sb.ToString();
                }
                m_ItemStackName = newItemStackName;
                m_NameDataChanged = false;
            }
        }

        public int AddAmount(int amount)
        {
            int possibleAmount = Item.MaxStack - Amount;
            int remainder = amount - possibleAmount;
            Amount += possibleAmount;
            return remainder;
        }

        public string[] GetImplicitTexts()
        {
            return Item.GetImplicitTexts(this);
        }
        public string[] GetExplicitTexts()
        {
            return Item.GetExplicitTexts(this);
        }

        public void ForceUpdate()
        {
            m_NameDataChanged = true;
            m_ItemIdentifier.ForceUpdate();
            if (!IsEmpty)
            {
                Item.PowerLevelItemStackChange(this);
            }
            if (m_ImplicitMods != null)
            {
                foreach (ValueMod valueMod in m_ImplicitMods)
                {
                    valueMod.ModIdentifier.ForceUpdate();
                    if (valueMod.Mod != null)
                    {
                        valueMod.Identifier = valueMod.Mod.ModBase.Identifier;
                    }
                    valueMod.BasicModBaseIdentifier.ForceUpdate();
                }
            }
            if (m_ExplicitMods != null)
            {
                foreach (ValueMod valueMod in m_ExplicitMods)
                {
                    valueMod.ModIdentifier.ForceUpdate();
                    if (valueMod.Mod != null)
                    {
                        valueMod.Identifier = valueMod.Mod.ModBase.Identifier;
                    }
                    valueMod.BasicModBaseIdentifier.ForceUpdate();
                }
            }
        }

        public CraftingAction[] GetCraftingActions()
        {
            if (IsEmpty) return null;
            if (Item.CraftingActionCatalog.Object != null) return Item.CraftingActionCatalog.Object.CraftingActions;
            return CraftingActionManager.GetDefaultCraftingActions(Item.ItemType.Identifier);
        }

        public ItemStack Copy()
        {
            ItemStack copyStack = new ItemStack(this.ItemIdentifier,this.Amount);

            copyStack.PowerLevel = this.PowerLevel;
            copyStack.ItemLevel = this.ItemLevel;

            copyStack.ImplicitMods = this.ImplicitMods;
            copyStack.ExplicitMods = this.ExplicitMods;

            return copyStack;
        }
        private void ChangeModifiersAll()
        {
            for(int i = 0; i < RegisteredEntities.Count; i++)
            {
                Entity entity = RegisteredEntities[i];
                if (entity == null)
                {
                    RegisteredEntities.RemoveAt(i);
                    i--;
                    continue;
                }
                AddModifiers(entity);
            }
        }

        private void AddModifiers(Entity entity)
        {
            if (entity == null) return;
            RemoveAllModifiers(entity);
            if (IsEmpty) return;
            if (Item.RegisterImplicitModifiers())
            {
                foreach (ValueMod valueMod in m_ImplicitMods)
                {
                    valueMod.BasicModBase.AddStatModifierTo(entity, valueMod.Value, this);
                }
            }
            if (Item.RegisterExplicitModifiers())
            {
                foreach (ValueMod valueMod in m_ExplicitMods)
                {
                    valueMod.BasicModBase.AddStatModifierTo(entity, valueMod.Value, this);
                }
            }
        }

        private void RemoveAllModifiers(Entity entity)
        {
            if (entity == null) return;
            entity.RemoveAllModifiersBySource(this);
        }

        public void Register(Entity entity)
        {
            if (entity == null) return;
            if (RegisteredEntities.Contains(entity)) return;
            RegisteredEntities.Add(entity);
            AddModifiers(entity);
        }

        public void Unregister(Entity entity)
        {
            if (entity == null) return;
            RegisteredEntities.Remove(entity);
            RemoveAllModifiers(entity);
        }
        #endregion
    }
}
