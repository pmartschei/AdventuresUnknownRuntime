using AdventuresUnknownSDK.Core.Entities;
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
            ActiveStats[slot] = new Entity();
            ActiveStats[slot].AddActiveStat(m_Inventory.Object.Items[slot]);
            Gem gem = m_Inventory.Object.Items[slot].Item as Gem;
            if (gem != null)
            {
                List<string> displayList = new List<string>();
                foreach(ModTypeIdentifier identifier in gem.DisplayMods)
                {
                    displayList.Add(identifier.Identifier);
                }
                m_DisplayMods[slot] = displayList.Distinct().ToArray();
            }
            if (slot == 0)
            {
                if (!m_Inventory.Object.Items[1].IsEmpty)
                    ActiveStats[slot].AddActiveStat(m_Inventory.Object.Items[1]);
            }
        }
        public override Entity CalculateEntity(int index)
        {
            ItemStack stack = m_Inventory.Object.Items[index];
            if (stack.IsEmpty) return null;
            ActiveGem activeGem = stack.Item as ActiveGem;
            Entity entity = new Entity();
            entity.CopyFrom(ActiveStats[index]);
            entity.AddFrom(EntityStats.Entity);
            entity.Notify(ActionTypeManager.AttackGeneration, new AttackContext(activeGem));
            return entity;
        }
        public string[] CalculateDisplayMods(int index)
        {
            return m_DisplayMods[index];
        }
        #endregion
        public override void Spawn(int index,Vector3 origin, Vector3 destination)
        {
            ItemStack stack = m_Inventory.Object.Items[index];
            if (stack.IsEmpty) return;
            if (CooldownManager.HasCooldown(stack)) return;
            CooldownManager.AddCooldown(stack, 1.0f);
            Entity entity = CalculateEntity(index);
            entity.GameObject = EntityStats.gameObject;
            CooldownContext cooldownContext = new CooldownContext(EntityStats.Entity);
            entity.Notify(ActionTypeManager.AttackCooldownGeneration, cooldownContext);
            CooldownManager.AddCooldown(this.EntityStats.Entity, 1.0f);
            if (cooldownContext.CanUse)
            {
                entity.Notify(ActionTypeManager.AttackCooldownApply, cooldownContext);
                ActiveGem activeGem = stack.Item as ActiveGem;
                activeGem.Activate(PlayerManager.PlayerController, entity, origin, destination);
            }
        }
    }
}
