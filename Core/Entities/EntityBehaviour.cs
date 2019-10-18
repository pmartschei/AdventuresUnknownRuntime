using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities
{
    public class EntityBehaviour : MonoBehaviour
    {

        [SerializeField] private Entity m_Entity = new Entity();
        [SerializeField] private EntityController m_EntityController = null;


        #region Properties
        public Entity Entity { get => m_Entity; set => m_Entity = value; }
        public EntityController EntityController { get => m_EntityController; set => m_EntityController = value; }
        #endregion

        #region Methods
        private void Start()
        {
            Entity.EntityBehaviour = this;
            Entity.Start();
        }
        private void Update()
        {
            if (Entity.Description.IsMinion && (Entity.Description.Parent == null || Entity.Description.Parent.Entity.EntityBehaviour == null))
            {
                Entity.Notify(ActionTypeManager.Death);
            }
            if (!Entity.IsDead)
            {
                Entity.Tick(Time.deltaTime);
            }
        }
        private void OnDestroy()
        {
            if (Entity.Description.IsMinion)
            {
                EntityBehaviour parent = Entity.Description.Parent;
                if (!parent) return;
                parent.Entity.RemoveMinion(Entity.Description.MinionCountModType, Entity);
            }
        }
        #endregion
    }
}
