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

        private EntityState m_MainState = null;
        private EntityState m_CurrentState = null;
        private EntityState m_NextState = null;
        private bool m_Destroying = false;

        #region Properties

        #endregion

        #region Methods
        public void SetNextState(EntityState nextState)
        {
            m_NextState = nextState;
        }

        public void SetNextStateToMain()
        {
            this.m_NextState = Instantiate(m_MainState);
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
                Debug.LogErrorFormat("State machine {0} on object {1} does not have a state!", m_CustomName, gameObject);
            }

            m_CurrentState.OnExit();
            m_CurrentState = newState;
            m_CurrentState.OnEnter();
        }
        private void Awake()
        {
            m_MainState = Instantiate(m_MainStateSO);
            m_CurrentState = new Uninitalized();
            m_CurrentState.EntityStateMachine = this;
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
            m_CurrentState.Update();
        }
        private void FixedUpdate()
        {
            if (m_NextState != null)
            {
                SetState(m_NextState);
            }
            m_CurrentState.FixedUpdate();
        }
        private void OnDestroy()
        {
            m_Destroying = true;
            if (m_CurrentState != null)
            {
                m_CurrentState.OnExit();
                m_CurrentState = null;
            }
        }
        private void OnDrawGizmos()
        {
            m_CurrentState.OnDrawGizmos();
        }
        private void OnDrawGizmosSelected()
        {
            m_CurrentState.OnDrawGizmosSelected();
        }
        #endregion
    }
}
