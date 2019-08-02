using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
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
        private int m_LastAttack = -1;
        private Enemy m_Enemy = null;
        private EnemyController m_EnemyController = null;

        private Entity m_NextEntity = null;

        #region Properties

        #endregion

        #region Methods
        private void Start()
        {
            m_EnemyController = GetComponent<EnemyController>();
            EntityStats = m_EnemyController.SpaceShip;
            m_Enemy = GetComponent<EnemyModel>().EntityBehaviour.Entity.Description.Enemy;
            UpdateListSizes(ActiveStats, m_Enemy.AttackPattern.GetAttackCount());
            m_ItemStacks = new List<ItemStack>();
            for(int i = 0; i < ActiveStats.Count; i++)
            {
                if (ActiveStats[i] == null)
                {
                    ActiveStats[i] = new Entity();
                }
                ItemStack itemStack = m_Enemy.AttackPattern.GetAttack(i).CreateItem();
                ActiveStats[i].Reset();
                ActiveStats[i].AddActiveStat(itemStack);
                m_ItemStacks.Add(itemStack);
            }
        }

        public override Entity CalculateEntity(int index)
        {
            int nextAttack = m_Enemy.AttackPattern.ChooseAttack(m_LastAttack);
            ActiveGem activeGem = m_ItemStacks[nextAttack].Item as ActiveGem;
            Entity entity = new Entity();
            entity.CopyFrom(ActiveStats[index]);
            entity.AddFrom(EntityStats.Entity);
            entity.Notify(ActionTypeManager.AttackGeneration, new AttackContext(activeGem));
            return entity;
        }
        public float GetNextAttackMaxDistance()
        {
            int nextAttack = m_Enemy.AttackPattern.ChooseAttack(m_LastAttack);
            ActiveGem activeGem = m_ItemStacks[nextAttack].Item as ActiveGem;
            Entity entity = CalculateEntity(nextAttack);
            return activeGem.GetMaxSkillDistance(entity);
        }
        public override void Spawn(int index, Vector3 origin, Vector3 destination)
        {
            int nextAttack = m_Enemy.AttackPattern.ChooseAttack(m_LastAttack);
            ItemStack itemStack = m_ItemStacks[nextAttack];
            m_LastAttack = nextAttack;
            ActiveGem activeGem = itemStack.Item as ActiveGem;
            if (!activeGem) return;
            if (CooldownManager.HasCooldown(itemStack)) return;
            CooldownManager.AddCooldown(itemStack, 1.0f);
            Entity entity = CalculateEntity(nextAttack);
            entity.GameObject = this.gameObject;
            CooldownManager.AddCooldown(m_EnemyController.SpaceShip.Entity,1.0f);
            activeGem.Activate(m_EnemyController, entity, origin, destination);
        }
        #endregion
    }
}
