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
        [SerializeField]
        private UIHealthBar m_UIHealthBarPrefab = null;
        [SerializeField]
        private Vector3 m_UIHealthBarOffset = Vector3.zero;
        [SerializeField]
        private float m_UIHealthBarWidth = 1.6f;

        private EntityBehaviour m_SpaceShip;
        private Vector3 m_LookingDestination;
        private EntityController m_Target;
        private Animator m_Animator;

        private UIHealthBar m_UIHealthBar = null;
        #region Properties
        public EntityBehaviour SpaceShip { get => m_SpaceShip; set => m_SpaceShip = value; }
        public Vector3 LookingDestination { get => m_LookingDestination; set => m_LookingDestination = value; }
        public Animator Animator { get => m_Animator; set => m_Animator = value; }
        public EntityController Target { get => m_Target; set => m_Target = value; }
        public UIHealthBar UIHealthBar { get => m_UIHealthBar; set => m_UIHealthBar = value; }

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
    }
}
