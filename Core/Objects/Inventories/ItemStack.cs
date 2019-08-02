using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Items.Actions;
using AdventuresUnknownSDK.Core.Objects.Mods.ModBases;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using AdventuresUnknownSDK.Core.Utils.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Inventories
{
    [Serializable]
    public class ItemStack : IActiveStat
    {
        [Serializable]
        public class ValueMod
        {
            [SerializeField] private float m_Value;
            [SerializeField] private BasicModBaseIdentifier m_BasicModBaseIdentifier = new BasicModBaseIdentifier();

            public float Value { get => m_Value; set => m_Value = value; }
            public BasicModBase BasicModBase { get => m_BasicModBaseIdentifier.Object; }
            public string Identifier { get => m_BasicModBaseIdentifier.Identifier; set => m_BasicModBaseIdentifier.Identifier = value; }
            internal BasicModBaseIdentifier BasicModBaseIdentifier { get => m_BasicModBaseIdentifier; }
        }
        public static ItemStack Empty = new ItemStack(null, 0);

        [SerializeField] private ItemIdentifier m_ItemIdentifier = new ItemIdentifier();
        [SerializeField] private int m_PowerLevel = 0;
        [SerializeField] private int m_Amount = 0;
        [SerializeField] private ValueMod[] m_ImplicitMods = new ValueMod[0];
        [SerializeField] private ValueMod[] m_ExplicitMods = new ValueMod[0];

        [NonSerialized] private string m_ItemStackName = "";
        [NonSerialized] private bool m_NameDataChanged = false;
        [NonSerialized] private string m_LastItemName = "";
        [NonSerialized] private bool m_StatsChanged = false;
        [NonSerialized] private Dictionary<string,Stat> m_HashedStats = new Dictionary<string, Stat>();

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
                }
            }
        }
        public int Amount
        {
            get => m_Amount;
            set
            {
                m_Amount = value;
                ForceUpdate();
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
        
        public bool IsEmpty { get => m_ItemIdentifier.Identifier.Equals(""); }
        public ValueMod[] ImplicitMods { get => m_ImplicitMods; set => m_ImplicitMods = value; }
        public ValueMod[] ExplicitMods { get => m_ExplicitMods; set => m_ExplicitMods = value; }

        public bool StatsChanged
        {
            get => m_StatsChanged;
            set => m_StatsChanged = value;
        }

        #endregion

        #region Methods
        public void Initialize(Entity activeStat)
        {
            if (IsEmpty) return;
            foreach(ValueMod valueMod in m_ImplicitMods)
            {
                activeStat.GetStat(valueMod.BasicModBase.ModTypeIdentifier).AddStatModifier(new StatModifier(valueMod.Value, valueMod.BasicModBase.CalculationType,this));
            }
        }

        private void UpdateIfChanged()
        {
            if (m_LastItemName == null || !m_LastItemName.Equals(Item.ItemName))
            {
                ForceUpdate();
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
            StatsChanged = true;
            m_NameDataChanged = true;
            m_ItemIdentifier.ForceUpdate();
            if (Item)
                Item.PowerLevelItemStackChange(this);
            if (m_ImplicitMods != null)
            {
                foreach (ValueMod valueMod in m_ImplicitMods)
                {
                    valueMod.BasicModBaseIdentifier.ForceUpdate();
                }
            }
            if (m_ExplicitMods != null)
            {
                foreach (ValueMod valueMod in m_ExplicitMods)
                {
                    valueMod.BasicModBaseIdentifier.ForceUpdate();
                }
            }
        }

        public CraftingAction[] GetCraftingActions()
        {
            if (IsEmpty) return null;
            return CraftingActionManager.GetDefaultCraftingActions(Item.ItemType.Identifier);
        }

        public ItemStack Copy()
        {
            ItemStack copyStack = new ItemStack(this.ItemIdentifier,this.Amount);

            copyStack.PowerLevel = this.PowerLevel;

            copyStack.ImplicitMods = this.ImplicitMods;
            copyStack.ExplicitMods = this.ExplicitMods;

            copyStack.StatsChanged = true;

            return copyStack;
        }
        #endregion
    }
}
