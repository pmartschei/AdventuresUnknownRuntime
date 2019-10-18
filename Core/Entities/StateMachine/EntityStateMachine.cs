using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.Controllers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//Source RoR2

namespace AdventuresUnknownSDK.Core.Entities.StateMachine
{
    public class EntityStateMachine : MonoBehaviour
    {
        [SerializeField] private string m_CustomName = "";
        [SerializeField]
        private EntityState m_MainStateSO = null;
        [SerializeField]
        private EntityState m_InitialStateSO = null;

        private EntityState m_CurrentState = null;
        private EntityState m_NextState = null;
        
        public class CommonComponents
        {
            public EntityController EntityController { get; internal set; }
            public EnemyController EnemyController { get; internal set; }
            public IAttackController AttackController { get; internal set; }
            public IRotationalController RotationalController { get; internal set; }
            public ITranslationalController TranslationalController { get; internal set; }
            public IMuzzleComponentController MuzzleComponentController { get; internal set; }
        }

        #region Properties
        public CommonComponents GameObjectComponents { get; private set; }
        #endregion

        #region Methods
        public void SetNextState(EntityState nextState)
        {
            if (nextState)
            {
                m_NextState = Instantiate(nextState);
            }
        }

        public void SetNextStateToMain()
        {
            if (m_MainStateSO)
                this.m_NextState = Instantiate(m_MainStateSO);
            else
            {
                this.m_NextState = ScriptableObject.CreateInstance(typeof(Uninitalized)) as Uninitalized;
            }
        }

        public bool HasPendingState()
        {
            return m_NextState != null;
        }

        public void SetState(EntityState newState)
        {
            if (newState == null) return;
            this.m_NextState = null;
            newState.EntityStateMachine = this;
            if (m_CurrentState == null)
            {
                Debug.LogErrorFormat("State machine {0} on object {1} does not have a state", m_CustomName, gameObject);
            }

            m_CurrentState.OnExit();
            m_CurrentState = newState;
            m_CurrentState.OnEnter();
        }
        private void Awake()
        {
            m_CurrentState = ScriptableObject.CreateInstance(typeof(Uninitalized)) as Uninitalized;
            m_CurrentState.EntityStateMachine = this;
            CommonComponents commonComponents = new CommonComponents();
            commonComponents.EntityController = gameObject.GetComponent<EntityController>();
            commonComponents.EnemyController = commonComponents.EntityController as EnemyController;
            commonComponents.AttackController = commonComponents.EntityController as IAttackController;
            commonComponents.RotationalController = commonComponents.EntityController as IRotationalController;
            commonComponents.TranslationalController = commonComponents.EntityController as ITranslationalController;
            commonComponents.MuzzleComponentController = commonComponents.EntityController as IMuzzleComponentController;
            GameObjectComponents = commonComponents;
        }
        private void Start()
        {
            if (m_NextState != null)
            {
                SetState(m_NextState);
                return;
            }
            SetState(Instantiate(m_InitialStateSO));
        }
        private void Update()
        {
            if (m_NextState != null)
            {
                SetState(m_NextState);
            }
            if (m_CurrentState && !GameObjectComponents.EntityController.Entity.IsDead)
                m_CurrentState.Update();
        }
        private void FixedUpdate()
        {
            if (m_NextState != null)
            {
                SetState(m_NextState);
            }
            if (m_CurrentState && !GameObjectComponents.EntityController.Entity.IsDead)
                m_CurrentState.FixedUpdate();
        }
        private void OnDestroy()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.OnExit();
                m_CurrentState = null;
            }
        }
        private void OnDrawGizmos()
        {
            if (m_CurrentState)
                m_CurrentState.OnDrawGizmos();
        }
        private void OnDrawGizmosSelected()
        {
            if (m_CurrentState)
                m_CurrentState.OnDrawGizmosSelected();
        }
        #endregion
    }
}
