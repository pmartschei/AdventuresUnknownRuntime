using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Attribute = AdventuresUnknownSDK.Core.Objects.Mods.Attribute;

namespace AdventuresUnknownSDK.Core.Entities
{
    public class BaseEntityStats : MonoBehaviour, IActiveStat
    {
        [SerializeField] private EntityBehaviour m_Entity = null;
        [SerializeField] private Attribute[] m_Attributes = null;

        private List<Attribute> m_ConsistentAttributes = new List<Attribute>();

        private List<Entity> m_RegisteredEntities = new List<Entity>();

        public Attribute[] Attributes { get => m_Attributes;
            set
            {
                m_Attributes = value;
                OnValidate();
            }
        }

        private class InternalBaseAction : BaseAction
        {
            public override ActionType ActionType => ActionTypeManager.Calculation;

            private Action m_Action = null;
            public InternalBaseAction(Action action)
            {
                m_Action = action;
            }
            public override void Notify(Entity activeStats, ActionContext actionContext)
            {
                m_Action.Invoke();
            }
        }
        #region Properties

        #endregion

        #region Methods
        private void Start()
        {
            if (m_Entity)
            {
                Register(m_Entity.Entity);
                m_Entity.Entity.NotifyOnStatChange(m_Entity.Entity.GetStat("core.modtypes.utility.level"), new InternalBaseAction(ChangeModifiersAll));
            }
            OnValidate();
        }
        private void OnValidate()
        {
            m_ConsistentAttributes.Clear();
            foreach (Attribute attribute in m_Attributes)
            {
                if (!attribute.ConsistencyCheck())
                {
                    GameConsole.LogWarningFormat("Skipped inconsistent Attribute in BaseEntityStats: {0} in {1}", attribute.ModBaseIdentifier, this.gameObject);
                    continue;
                }
                m_ConsistentAttributes.Add(attribute);
            }
            ChangeModifiersAll();
        }

        private void ChangeModifiersAll()
        {
            foreach(Entity entity in m_RegisteredEntities)
            {
                AddModifiers(entity);
            }
        }
        
        private void AddModifiers(Entity entity)
        {
            RemoveAllModifiers(entity);

            foreach(Attribute attribute in m_ConsistentAttributes)
            {
                attribute.ModBase.AddStatModifierTo(entity,attribute.Value(entity.GetStat("core.modtypes.utility.level").Calculated),this);
            }
        }

        private void RemoveAllModifiers(Entity entity)
        {
            entity.RemoveAllModifiersBySource(this);
        }

        public void Register(Entity entity)
        {
            if (m_RegisteredEntities.Contains(entity)) return;
            m_RegisteredEntities.Add(entity);
            AddModifiers(entity);
        }

        public void Unregister(Entity entity)
        {
            m_RegisteredEntities.Remove(entity);
            RemoveAllModifiers(entity);
        }
        #endregion
    }
}
