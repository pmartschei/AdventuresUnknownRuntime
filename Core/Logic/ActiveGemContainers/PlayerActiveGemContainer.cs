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

        private List<Entity> m_ActiveStatsApply = new List<Entity>();

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
            UpdateListSizes(m_ActiveStatsApply, m_Inventory.Object.Size);
            UpdateSlot(slot);
        }

        private void UpdateSlot(int slot)
        {
            if (ActiveStats[slot] == null)
            {
                ActiveStats[slot] = new Entity();
                m_ActiveStatsApply[slot] = new Entity();
                ActiveStats[slot].EntityBehaviour = EntityStats;
                m_ActiveStatsApply[slot].EntityBehaviour = EntityStats;
                //hacky stuff
                EntityStats.Entity.Register(ActiveStats[slot]);
                EntityStats.Entity.Register(m_ActiveStatsApply[slot]);
                if (slot == 0)
                {
                    for (int i = 1; i < m_Inventory.Object.Size; i++)
                    {
                        m_Inventory.Object.Register(ActiveStats[slot], i);
                        m_Inventory.Object.Register(m_ActiveStatsApply[slot], i);
                    }
                }
                else
                {
                    m_Inventory.Object.Register(ActiveStats[slot], 0);
                    m_Inventory.Object.Register(m_ActiveStatsApply[slot], 0);
                }
                if (slot == 1)
                {
                    m_Inventory.Object.Register(ActiveStats[slot], 2);
                    m_Inventory.Object.Register(ActiveStats[slot], 5);
                    m_Inventory.Object.Register(m_ActiveStatsApply[slot], 2);
                    m_Inventory.Object.Register(m_ActiveStatsApply[slot], 5);
                }
                else if (slot == 2)
                {
                    m_Inventory.Object.Register(ActiveStats[slot], 1);
                    m_Inventory.Object.Register(ActiveStats[slot], 3);
                    m_Inventory.Object.Register(m_ActiveStatsApply[slot], 1);
                    m_Inventory.Object.Register(m_ActiveStatsApply[slot], 3);
                }
                else if (slot == 3)
                {
                    m_Inventory.Object.Register(ActiveStats[slot], 2);
                    m_Inventory.Object.Register(ActiveStats[slot], 4);
                    m_Inventory.Object.Register(m_ActiveStatsApply[slot], 2);
                    m_Inventory.Object.Register(m_ActiveStatsApply[slot], 4);
                }
                else if (slot == 4)
                {
                    m_Inventory.Object.Register(ActiveStats[slot], 3);
                    m_Inventory.Object.Register(ActiveStats[slot], 5);
                    m_Inventory.Object.Register(m_ActiveStatsApply[slot], 3);
                    m_Inventory.Object.Register(m_ActiveStatsApply[slot], 5);
                }
                else if (slot == 5)
                {
                    m_Inventory.Object.Register(ActiveStats[slot], 1);
                    m_Inventory.Object.Register(ActiveStats[slot], 4);
                    m_Inventory.Object.Register(m_ActiveStatsApply[slot], 1);
                    m_Inventory.Object.Register(m_ActiveStatsApply[slot], 4);
                }
            }
            for (int i = 0; i < m_Inventory.Object.Size; i++)
            {
                ActiveGem activeGem = m_Inventory.Object.Items[i].Item as ActiveGem;
                if (activeGem != null)
                {
                    if (ActiveStats[i] != null)
                    {
                        ActiveStats[i].RemoveAllModifiersBySource(activeGem);
                        m_ActiveStatsApply[i].RemoveAllModifiersBySource(activeGem);
                        foreach (Objects.Mods.Attribute attribute in (m_Inventory.Object.Items[i].Item as ActiveGem).Attributes)
                        {
                            ActiveStats[i].GetStat(attribute.ModBase.ModTypeIdentifier).AddStatModifier(new StatModifier(attribute.GetValue(m_Inventory.Object.Items[i].PowerLevel), attribute.ModBase.CalculationType, activeGem));
                            m_ActiveStatsApply[i].GetStat(attribute.ModBase.ModTypeIdentifier).AddStatModifier(new StatModifier(attribute.GetValue(m_Inventory.Object.Items[i].PowerLevel), attribute.ModBase.CalculationType, activeGem));
                        }
                        List<string> displayList = new List<string>();
                        foreach (ModTypeIdentifier identifier in activeGem.DisplayMods)
                        {
                            displayList.Add(identifier.Identifier);
                        }
                        m_DisplayMods[i] = displayList.Distinct().ToArray();
                        m_ActiveStatsApply[i].Notify(ActionTypeManager.AttackApply, new AttackContext(activeGem));
                    }
                }
            }
        }

        public override Entity GetEntityWithApply(int index)
        {
            return m_ActiveStatsApply[index];
        }
        public override Entity CalculateEntity(int index)
        {
            return ActiveStats[index];
            //Entity entity = new Entity();
            //entity.CopyFrom(ActiveStats[index]);
            //ItemStack stack = m_Inventory.Object.Items[index];
            //foreach (Objects.Mods.Attribute attribute in (stack.Item as ActiveGem).Attributes)
            //{
            //    entity.GetStat(attribute.ModBase.ModTypeIdentifier).AddStatModifier(new StatModifier(attribute.Value(stack.PowerLevel), attribute.ModBase.CalculationType, this));
            //}
            //return entity;
        }

        public float GetCooldown(int index)
        {
            return CooldownManager.GetCooldown(m_Inventory.Object.Items[index]);
        }
        public override string[] CalculateDisplayMods(int index)
        {
            return m_DisplayMods[index];
        }
        public override void Spawn(EntityController origin, int index,params Muzzle[] muzzles)
        {
            ItemStack stack = m_Inventory.Object.Items[index];
            if (stack.IsEmpty) return;
            if (CooldownManager.HasCooldown(stack)) return;
            Entity entityWithAppliedStats = GetEntityWithApply(index);
            if (entityWithAppliedStats.GetStat("core.modtypes.skills.casttime").Calculated > 0 && CooldownManager.HasCooldown(origin.SpaceShip.Entity)) return;
            //Entity statsWithout = new Entity();
            //statsWithout.CopyFrom(ActiveStats[index]);
            //Entity entity = new Entity();
            //entity.CopyFrom(CalculateEntity(index,true));
            //statsWithout.EntityBehaviour = origin.SpaceShip;
            //entity.EntityBehaviour = origin.SpaceShip;
            Entity entity = new Entity();
            entity.CopyFrom(entityWithAppliedStats);
            CooldownContext cooldownContext = new CooldownContext(EntityStats.Entity);
            entity.Notify(ActionTypeManager.AttackCooldownGeneration, cooldownContext);
            if (cooldownContext.CanUse)
            {
                CooldownManager.AddCooldown(stack, entity.GetStat("core.modtypes.skills.cooldown").Calculated);
                CooldownManager.AddCooldown(this.EntityStats.Entity, entity.GetStat("core.modtypes.skills.casttime").Calculated);
                entity.Notify(ActionTypeManager.AttackCooldownApply, cooldownContext);
                ActiveGem activeGem = stack.Item as ActiveGem;
                activeGem.Activate(origin, entity, stack.PowerLevel , 0, muzzles);
            }
        }
        #endregion
    }
}
