﻿using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Currencies;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Localization;
using AdventuresUnknownSDK.Core.Objects.Tags;
using AdventuresUnknownSDK.Core.UI.Items;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static AdventuresUnknownSDK.Core.Objects.Inventories.ItemStack;

namespace AdventuresUnknownSDK.Core.Objects.Items
{
    public abstract class Item : CoreObject
    {
        [SerializeField] private LocalizationString m_ItemName = null;
        [SerializeField] private LocalizationString m_Description = null;
        [SerializeField] private bool m_IsStackable = false;
        [SerializeField] private int m_MaxStack = 1;
        [SerializeField] private int m_Level = 0;
        [SerializeField] private CurrencyValue m_CurrencyValue;
        [SerializeField] private int m_DefaultAmount = 1;
        [SerializeField] private int m_DefaultPowerLevel = 0;
        [SerializeField] private ItemTypeIdentifier m_ItemTypeIdentifier = null;
        [SerializeField] private Sprite m_Sprite = null;
        [SerializeField] private AbstractItemStackDisplay m_ItemStackDisplay = null;
        [SerializeField] private TagList m_Tags = new TagList();
        [SerializeField] private string m_FormatterType = "";
        [SerializeField] private CraftingActionCatalogIdentifier m_CraftingActionCatalog = null;
        
        #region Properties
        public string ItemName { get => m_ItemName.LocalizedString; }
        public string Description { get => m_Description.LocalizedString; }
        public bool IsStackable { get => m_IsStackable; set => m_IsStackable = value; }
        public int MaxStack { get => m_MaxStack; set => m_MaxStack = value; }
        public int Level { get => m_Level; set => m_Level = value; }
        public Sprite Sprite { get => m_Sprite; set => m_Sprite = value; }
        public AbstractItemStackDisplay ItemStackDisplay { get => m_ItemStackDisplay; set => m_ItemStackDisplay = value; }
        public ItemType ItemType { get => m_ItemTypeIdentifier.Object; }
        public int DefaultAmount { get => m_DefaultAmount; set => m_DefaultAmount = value; }
        public int DefaultPowerLevel { get => m_DefaultPowerLevel; set => m_DefaultPowerLevel = value; }
        public CurrencyValue CurrencyValue { get => m_CurrencyValue; set => m_CurrencyValue = value; }
        public TagList Tags { get => m_Tags; set => m_Tags = value; }
        public CraftingActionCatalogIdentifier CraftingActionCatalog { get => m_CraftingActionCatalog; set => m_CraftingActionCatalog = value; }
        #endregion

        #region Methods

        public override bool ConsistencyCheck()
        {
            if (!base.ConsistencyCheck()) return false;
            if (!m_ItemTypeIdentifier.ConsistencyCheck()) return false;
            m_CraftingActionCatalog.ConsistencyCheck();
            if (!Tags.ConsistencyCheck()) return false;
            if (!m_CurrencyValue.Currency.ConsistencyCheck()) return false;

            return true;
        }

        public override void ForceUpdate()
        {
            base.ForceUpdate();
            m_ItemName.ForceUpdate();
            m_Description.ForceUpdate();
        }
        public virtual void PowerLevelItemStackChange(ItemStack itemStack)
        {
            return;
        }
        public virtual string[] GetImplicitTexts(ItemStack itemStack)
        {
            return GetValueModsTexts(itemStack.ImplicitMods, m_FormatterType);
        }
        public virtual string[] GetExplicitTexts(ItemStack itemStack)
        {
            return GetValueModsTexts(itemStack.ExplicitMods,m_FormatterType);
        }
        public virtual Stat[] GetStats(ItemStack itemStack)
        {
            return null;
        }

        public virtual ItemStack CreateItem(int amount)
        {
            ItemStack itemStack = new ItemStack(Identifier, amount);
            itemStack.PowerLevel = m_DefaultPowerLevel;
            return itemStack;
        }
        public virtual ItemStack CreateItem()
        {
            return CreateItem(m_DefaultAmount);
        }

        protected string[] GetValueModsTexts(ItemStack.ValueMod[] valueMods,string formatter)
        {
            List<string> implicitTexts = new List<string>();

            foreach (ItemStack.ValueMod valueMod in valueMods)
            {
                if (!valueMod.BasicModBase)
                {
                    implicitTexts.Add("Missing: " + valueMod.Identifier);
                    continue;
                }
                string text = valueMod.BasicModBase.ToText(formatter, valueMod.Value);
                if (!text.Equals(string.Empty))
                {
                    implicitTexts.Add(text);
                }
            }

            return implicitTexts.ToArray();
        }
        public virtual bool IsModifiable()
        {
            return true;
        }

        public virtual bool RegisterImplicitModifiers()
        {
            return true;
        }
        public virtual bool RegisterExplicitModifiers()
        {
            return true;
        }
        #endregion
    }
}
