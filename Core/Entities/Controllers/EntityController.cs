using AdventuresUnknownSDK.Core.Entities.Weapons;
using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.Controllers
{
    public abstract class EntityController : MonoBehaviour
    {
        [SerializeField] private Transform m_Head = null;
        [SerializeField]
        private UIHealthBar m_UIHealthBarPrefab = null;
        [SerializeField]
        private Vector3 m_UIHealthBarOffset = new Vector3(0,0,1.0f);
        [SerializeField]
        private float m_UIHealthBarWidth = 1.6f;
        [SerializeField] private Entity m_Entity = null;
        private EntityBehaviour m_SpaceShip;
        private Vector3 m_LookingDestination;
        private EntityController m_Target;
        private Animator m_Animator;

        private UIHealthBar m_UIHealthBar = null;
        #region Properties
        public EntityBehaviour SpaceShip { get => m_SpaceShip; set => m_SpaceShip = value; }
        public Entity Entity { get => m_Entity; set => m_Entity = value; }
        public Vector3 LookingDestination { get => m_LookingDestination; set => m_LookingDestination = value; }
        public Animator Animator { get => m_Animator; set => m_Animator = value; }
        public EntityController Target { get => m_Target; set => m_Target = value; }
        public UIHealthBar UIHealthBar { get => m_UIHealthBar; set => m_UIHealthBar = value; }
        
        public Transform Head
        {
            get
            {
                if (m_Head != null) return m_Head;
                return this.transform;
            }
            set => m_Head = value;
        }

        #endregion

        private void Start()
        {
            OnStart();
        }
        public virtual void OnStart()
        {
            if (m_UIHealthBarPrefab)
            {
                m_UIHealthBar = Instantiate(m_UIHealthBarPrefab, UIManager.HealthBarsTransform);
                m_UIHealthBar.EntityController = this;
                m_UIHealthBar.Offset = m_UIHealthBarOffset;
                m_UIHealthBar.SetWidthRelativeInGame(m_UIHealthBarWidth);
            }
        }

        public virtual void SwitchTarget(EntityController newTarget)
        {
            Target = newTarget;
        }

        public virtual void KillEntity()
        {
            if (Entity.IsDead)
            {
                Entity.Notify(ActionTypeManager.Death);
            }
            Entity.Notify(ActionTypeManager.PostDeath);
            Destroy(this.gameObject);
        }
    }
}
