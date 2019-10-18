using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.Weapons;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Logic.ActiveGemContainers
{
    public class PlayerActiveGemContainer : GenericActiveGemContainer
    {
        [SerializeField] private InventoryIdentifier m_Inventory = null;

        private List<string[]> m_DisplayMods = new List<string[]>();

        #region Properties
        public Inventory Inventory { get => m_Inventory.Object; }
        #endregion

        #region Methods
        private void OnEnable()
        {
            EntityStats = PlayerManager.SpaceShip;
            if (!m_Inventory.ConsistencyCheck())
            {
                this.enabled = false;
                return;
            }
            m_Inventory.Object.OnSlotUpdateEvent.AddListener(OnInventoryUpdate);
            for (int i = 0; i < m_Inventory.Object.Size; i++)
                OnInventoryUpdate(i);
        }

        private void OnDisable()
        {
            if (!m_Inventory.Object) return;
            m_Inventory.Object.OnSlotUpdateEvent.RemoveListener(OnInventoryUpdate);
        }
        private void OnInventoryUpdate(int slot)
        {
            UpdateListSizes(ActiveStats, m_Inventory.Object.Size);
            UpdateListSizes(m_DisplayMods, m_Inventory.Object.Size);
            if (ActiveStats[slot] == null)
            {
                ActiveStats[slot] = new Entity();
                //hacky stuff
                EntityStats.Entity.Register(ActiveStats[slot]);
                if (slot == 0)
                {
                    for (int i = 1; i < m_Inventory.Object.Size; i++)
                        m_Inventory.Object.Register(ActiveStats[slot], i);
                }
                else
                {
                    m_Inventory.Object.Register(ActiveStats[slot], 0);
                }
                if (slot == 1)
                {
                    m_Inventory.Object.Register(ActiveStats[slot], 2);
                    m_Inventory.Object.Register(ActiveStats[slot], 5);
                }
                else if (slot == 2)
                {
                    m_Inventory.Object.Register(ActiveStats[slot], 1);
                    m_Inventory.Object.Register(ActiveStats[slot], 3);
                }
                else if (slot == 3)
                {
                    m_Inventory.Object.Register(ActiveStats[slot], 2);
                    m_Inventory.Object.Register(ActiveStats[slot], 4);
                }
                else if (slot == 4)
                {
                    m_Inventory.Object.Register(ActiveStats[slot], 3);
                    m_Inventory.Object.Register(ActiveStats[slot], 5);
                }
                else if (slot == 5)
                {
                    m_Inventory.Object.Register(ActiveStats[slot], 1);
                    m_Inventory.Object.Register(ActiveStats[slot], 4);
                }
            }
            ActiveGem activeGem = m_Inventory.Object.Items[slot].Item as ActiveGem;
            if (activeGem != null)
            {
                List<string> displayList = new List<string>();
                foreach(ModTypeIdentifier identifier in activeGem.DisplayMods)
                {
                    displayList.Add(identifier.Identifier);
                }
                m_DisplayMods[slot] = displayList.Distinct().ToArray();
            }
        }

        public Entity CalculateEntity(int index,bool notifyAttackApply)
        {
            Entity entity = CalculateEntity(index);
            if (notifyAttackApply)
            {
                entity.Notify(ActionTypeManager.AttackApply, new AttackContext(m_Inventory.Object.Items[index].Item as ActiveGem));
            }
            return entity;
        }
        public override Entity CalculateEntity(int index)
        {
            Entity entity = new Entity();
            entity.CopyFrom(ActiveStats[index]);
            ItemStack stack = m_Inventory.Object.Items[index];
            foreach (Objects.Mods.Attribute attribute in (stack.Item as ActiveGem).Attributes)
            {
                entity.GetStat(attribute.ModBase.ModTypeIdentifier).AddStatModifier(new StatModifier(attribute.Value(stack.PowerLevel), attribute.ModBase.CalculationType, this));
            }
            return entity;
        }

        public float GetCooldown(int index)
        {
            return CooldownManager.GetCooldown(m_Inventory.Object.Items[index]);
        }
        public string[] CalculateDisplayMods(int index)
        {
            return m_DisplayMods[index];
        }
        public override void Spawn(EntityController origin, int index,params Muzzle[] muzzles)
        {
            ItemStack stack = m_Inventory.Object.Items[index];
            if (stack.IsEmpty) return;
            if (CooldownManager.HasCooldown(stack)) return;
            if (CooldownManager.HasCooldown(origin.SpaceShip.Entity)) return;
            Entity statsWithout = new Entity();
            statsWithout.CopyFrom(ActiveStats[index]);
            Entity entity = new Entity();
            entity.CopyFrom(CalculateEntity(index,true));
            statsWithout.EntityBehaviour = origin.SpaceShip;
            entity.EntityBehaviour = origin.SpaceShip;
            CooldownContext cooldownContext = new CooldownContext(EntityStats.Entity);
            entity.Notify(ActionTypeManager.AttackCooldownGeneration, cooldownContext);
            if (cooldownContext.CanUse)
            {
                CooldownManager.AddCooldown(stack, entity.GetStat("core.modtypes.skills.cooldown").Calculated);
                CooldownManager.AddCooldown(this.EntityStats.Entity, entity.GetStat("core.modtypes.skills.casttime").Calculated);
                entity.Notify(ActionTypeManager.AttackCooldownApply, cooldownContext);
                ActiveGem activeGem = stack.Item as ActiveGem;
                activeGem.Activate(origin, statsWithout, stack.PowerLevel , muzzles);
            }
        }
        #endregion
    }
}
