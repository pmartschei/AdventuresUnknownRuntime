using AdventuresUnknownSDK.Core.Objects.Mods.ModBases;
using AdventuresUnknownSDK.Core.Objects.Tags;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static AdventuresUnknownSDK.Core.Objects.Inventories.ItemStack;

namespace AdventuresUnknownSDK.Core.Objects.Mods
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Mod", fileName = "Mod.asset")]
    public class Mod : CoreObject
    {
        [SerializeField] private BasicModBaseIdentifier m_ModBaseIdentifier = null;
        [SerializeField] private int m_Domain = 0;
        [SerializeField] private string[] m_ModGroups = null;
        [Range(1, 25)]
        [SerializeField] private int m_Tier = 1;
        [Range(0,100)]
        [SerializeField] private int m_RequiredLevel = 0;
        [SerializeField] private float m_MinValue = 1;
        [SerializeField] private float m_MaxValue = 1;
        [SerializeField] private float m_Round = 1;
        [SerializeField] private WeightedTagList m_WeightedTagList = new WeightedTagList();

        #region Properties
        public int Tier
        {
            get => m_Tier;
            set { m_Tier = value; ForceUpdate(); }
        }
        public int RequiredLevel { get => m_RequiredLevel; set => m_RequiredLevel = value; }
        public float MinValue { get => m_MinValue; set => m_MinValue = value; }
        public float MaxValue { get => m_MaxValue; set => m_MaxValue = value; }
        public float Round { get => m_Round; set => m_Round = value; }
        public BasicModBase ModBase {
            get
            {
                return m_ModBaseIdentifier.Object;
            }
        }

        public WeightedTagList WeightedTagList { get => m_WeightedTagList; set => m_WeightedTagList = value; }
        public int Domain { get => m_Domain; set => m_Domain = value; }
        public string[] ModGroups { get => m_ModGroups; set => m_ModGroups = value; }

        #endregion

        #region Methods
        public override bool ConsistencyCheck()
        {
            return m_ModBaseIdentifier.ConsistencyCheck();
        }
        public override bool IsIdentifierEditableInEditor()
        {
            return true;
        }
        protected override string IdentifierCalculation()
        {
            return base.IdentifierCalculation();
            //StringBuilder sb = new StringBuilder();
            //sb.Append(m_ModBaseIdentifier.Identifier);
            //sb.Append(".t");
            //sb.Append(Tier);
            //return sb.ToString().ToLowerInvariant();
        }
        public virtual void OnValidate()
        {
            ForceUpdateImmediately();
        }

        public virtual string ToText(string formatterIdentifier, float value)
        {
            return m_ModBaseIdentifier.Object.ToText(formatterIdentifier, value);
        }

        public virtual int GetSumOfTags(params string[] tags)
        {
            return m_WeightedTagList.GetWeight(tags);
        }

        public virtual ValueMod Roll()
        {
            ValueMod valueMod = new ValueMod();
            valueMod.Identifier = ModBase.Identifier;
            valueMod.Value = MinValue + UnityEngine.Random.Range(0.0f, MaxValue - MinValue);
            if (m_Round != 0.0f)
            {
                int count = (int)((m_MaxValue - m_MinValue) / m_Round);
                valueMod.Value = MinValue + UnityEngine.Random.Range(0,count+1) * m_Round;
            }
            valueMod.Mod = this;
            return valueMod;
        }
        #endregion
    }
}
