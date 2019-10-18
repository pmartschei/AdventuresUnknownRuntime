using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.Weapons;
using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Pool;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AdventuresUnknownSDK.Core.Objects.Inventories.ItemStack;
using Attribute = AdventuresUnknownSDK.Core.Objects.Mods.Attribute;

namespace AdventuresUnknownSDK.Core.Objects.Items
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/ActiveGem", fileName = "ActiveGem.asset")]
    public class ActiveGem : Gem
    {
        [SerializeField] private Attribute[] m_Attributes = null;
        [SerializeField] private DisplayMods m_DisplayMods = null;
        [SerializeField] private GenericAttack m_GenericAttack = null;
        [SerializeField] private ActiveGemIdentifier[] m_SecondaryActiveGems = null;

        [NonSerialized]
        private List<Attribute> m_ConsistentAttributes = new List<Attribute>();
        [NonSerialized]
        private List<ActiveGem> m_SecondaryActiveList = new List<ActiveGem>();

        #region Properties

        public ModTypeIdentifier[] DisplayMods { get => m_DisplayMods.Mods; }
        public Attribute[] Attributes { get => m_ConsistentAttributes.ToArray(); }
        public bool AIRequiresTarget { get => m_GenericAttack.AIRequiresTarget; set => m_GenericAttack.AIRequiresTarget = value; }
        #endregion

        #region Methods
        public override bool ConsistencyCheck()
        {
            bool flag = base.ConsistencyCheck();
            if (m_GenericAttack == null)
            {
                GameConsole.LogWarningFormat("ActiveGem {0} has no attack prefab", this.Identifier);
                return false;
            }
            if (m_SecondaryActiveGems != null)
            {
                foreach (ActiveGemIdentifier secondaryActiveGem in m_SecondaryActiveGems)
                {
                    if (secondaryActiveGem.ConsistencyCheck())
                    {
                        m_SecondaryActiveList.Add(secondaryActiveGem.Object);
                    }
                    else
                    {
                        m_SecondaryActiveList.Add(null);
                    }
                }
            }
            m_ConsistentAttributes.Clear();
            if (m_Attributes != null)
            {
                foreach (Attribute attribute in m_Attributes)
                {
                    if (!attribute.ConsistencyCheck())
                    {
                        GameConsole.LogWarningFormat("Skipped inconsistent Attribute: {0}", attribute.ModBaseIdentifier);
                        continue;
                    }
                    m_ConsistentAttributes.Add(attribute);
                }
            }
            if (m_DisplayMods != null)
            {
                m_DisplayMods.ConsistencyCheck();
            }
            return flag;
        }

        public override void Initialize()
        {
            base.Initialize();
            for(int i = 0; i < m_SecondaryActiveList.Count; i++)
            {
                if (m_SecondaryActiveList[i] == null) continue;
                if (m_SecondaryActiveList[i].CheckRecursive(this))
                {
                    m_SecondaryActiveList[i] = null;
                    GameConsole.LogWarningFormat("ActiveGem {0} had recursive secondary active gems", this.Identifier);
                }
            }
            PoolManager.CreatePool(m_GenericAttack.GetComponent<PoolDescription>());
        }

        private bool CheckRecursive(ActiveGem activeGem)
        {
            Queue<ActiveGem> toCheck = new Queue<ActiveGem>();
            toCheck.Enqueue(this);
            while (toCheck.Count > 0)
            {
                ActiveGem selectedActiveGem = toCheck.Dequeue();
                if (selectedActiveGem == null) continue;
                if (selectedActiveGem == activeGem) return true;
                foreach(ActiveGem secondaryActiveGem in selectedActiveGem.m_SecondaryActiveList)
                {
                    toCheck.Enqueue(secondaryActiveGem);
                }
            }
            return false;
        }

        public virtual void Activate(EntityController entityController, Entity stats ,float level = 0.0f, params Muzzle[] muzzles)
        {
            List<Muzzle> validMuzzles = new List<Muzzle>();
            if (muzzles != null)
            {
                foreach (Muzzle muzzle in muzzles)
                {
                    if (!muzzle) continue;
                    validMuzzles.Add(muzzle);
                }
            }
            ActivationParameters activationParameters = new ActivationParameters();
            activationParameters.EntityController = entityController;
            activationParameters.ActiveGem = this;
            activationParameters.Stats = stats;
            activationParameters.FrozenStats = new Entity();
            activationParameters.FrozenStats.CopyFrom(stats);
            activationParameters.Level = level;
            activationParameters.Muzzles = validMuzzles.ToArray();
            activationParameters.SecondaryActiveGems = m_SecondaryActiveList.ToArray();
            entityController.StartCoroutine(m_GenericAttack.Activate(activationParameters));
        }
        private ValueMod[] GetImplicits(ItemStack itemStack)
        {
            List<ValueMod> valueMods = new List<ValueMod>();

            foreach (Attribute attribute in m_ConsistentAttributes)
            {
                ValueMod valueMod = new ValueMod();
                valueMod.Value = attribute.Value(itemStack.PowerLevel);
                valueMod.Identifier = attribute.ModBaseIdentifier;
                valueMods.Add(valueMod);
            }

            return valueMods.ToArray();
        }

        public override void PowerLevelItemStackChange(ItemStack itemStack)
        {
            base.PowerLevelItemStackChange(itemStack);
            itemStack.ImplicitMods = GetImplicits(itemStack);
        }

        public virtual float GetMaxSkillDistance(Entity stats)
        {
            return m_GenericAttack.GetMaxSkillDistance(stats);
        }

        public virtual float GetAIPriority(EntityController controller, Entity stats, EntityController target)
        {
            return m_GenericAttack.GetAIPriority(controller, stats, target);
        }

        #endregion
    }
}
