using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.Weapons;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Enemies;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Logic.ActiveGemContainers
{
    [RequireComponent(typeof(EnemyModel))]
    [RequireComponent(typeof(EnemyController))]
    public class EnemyActiveGemContainer : GenericActiveGemContainer
    {
        [SerializeField] private List<ItemStack> m_ItemStacks = null;
        private Enemy m_Enemy = null;
        private EnemyController m_EnemyController = null;

        #region Properties

        #endregion

        #region Methods
        private void Start()
        {
            m_EnemyController = GetComponent<EnemyController>();
            EntityStats = m_EnemyController.SpaceShip;
            m_Enemy = GetComponent<EnemyModel>().EntityBehaviour.Entity.Description.Enemy;
            m_ItemStacks = new List<ItemStack>();
            OnLevelChange(null);
            m_EnemyController.Entity.GetStat("core.modtypes.utility.level").OnCalculatedChange += OnLevelChange;
        }

        private void OnLevelChange(Stat stat)
        {
            if (m_Enemy.ActiveGemCollection)
            {
                List<ActiveGem> activeGems = m_Enemy.ActiveGemCollection.ActiveGems;
                UpdateListSizes(ActiveStats, activeGems.Count);
                for (int i = 0; i < ActiveStats.Count; i++)
                {
                    if (ActiveStats[i] == null)
                    {
                        ActiveStats[i] = new Entity();
                    }
                    ItemStack itemStack = activeGems[i].CreateItem();
                    ActiveStats[i].Reset();
                    itemStack.Register(ActiveStats[i]);
                    EntityStats.Entity.Register(ActiveStats[i]);
                    m_ItemStacks.Add(itemStack);
                    foreach (Objects.Mods.Attribute attribute in (itemStack.Item as ActiveGem).Attributes)
                    {
                        ActiveStats[i].GetStat(attribute.ModBase.ModTypeIdentifier).AddStatModifier(new StatModifier(attribute.GetValue(itemStack.PowerLevel + m_EnemyController.Entity.GetStat("core.modtypes.utility.level").Calculated / 5), attribute.ModBase.CalculationType, itemStack.Item));
                    }
                    ActiveStats[i].Notify(ActionTypeManager.AttackApply, new AttackContext(itemStack.Item as ActiveGem));
                }
            }
        }

        public override Entity CalculateEntity(int index)
        {
            return ActiveStats[index];
            //ActiveGem activeGem = m_ItemStacks[index].Item as ActiveGem;
            //Entity entity = new Entity();
            //entity.CopyFrom(ActiveStats[index]);
            //entity.AddFrom(EntityStats.Entity);
            ////entity.Notify(ActionTypeManager.AttackGeneration, new AttackContext(activeGem));
            //return entity;
        }
        public bool IsIndexValid(int index)
        {
            if (index < 0 ||
                index >= m_ItemStacks.Count) return false;

            ActiveGem activeGem = m_ItemStacks[index].Item as ActiveGem;
            if (!activeGem) return false;
            return true;
        }
        public bool RequiresTarget(int index)
        {
            ActiveGem activeGem = m_ItemStacks[index].Item as ActiveGem;
            return activeGem.AIRequiresTarget;
        }
        public bool HasCooldown(EntityController entityController, int index)
        {
            return (CooldownManager.HasCooldown(entityController) || CooldownManager.HasCooldown(m_ItemStacks[index]));
        }
        public float GetAttackPriority(int index,EntityController controller,EntityController target)
        {
            ActiveGem activeGem = m_ItemStacks[index].Item as ActiveGem;
            return activeGem.GetAIPriority(controller, CalculateEntity(index), target);
        }
        public float GetAttackMaxDistance(int index)
        {
            ActiveGem activeGem = m_ItemStacks[index].Item as ActiveGem;
            return activeGem.GetMaxSkillDistance(CalculateEntity(index));
        }
        public override void Spawn(EntityController origin, int index,params Muzzle[] muzzles)
        {
            ItemStack itemStack = m_ItemStacks[index];
            ActiveGem activeGem = itemStack.Item as ActiveGem;
            if (!activeGem) return;
            if (HasCooldown(origin,index)) return;
            Entity entity = new Entity();
            entity.CopyFrom(CalculateEntity(index));
            CooldownManager.AddCooldown(itemStack, entity.GetStat("core.modtypes.skills.cooldown").Calculated);
            CooldownManager.AddCooldown(origin, entity.GetStat("core.modtypes.skills.casttime").Calculated);
            //Entity statsWithout = new Entity();
            //statsWithout.CopyFrom(EntityStats.Entity);
            //statsWithout.EntityBehaviour = origin.SpaceShip;
            //CooldownManager.AddCooldown(m_EnemyController.SpaceShip.Entity,1.0f);
            activeGem.Activate(origin, CalculateEntity(index), itemStack.PowerLevel + origin.SpaceShip.Entity.GetStat("core.modtypes.utility.level").Calculated / 5,0, muzzles);
        }
        #endregion
    }
}
