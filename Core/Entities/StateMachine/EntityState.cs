using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//Source RoR2

namespace AdventuresUnknownSDK.Core.Entities.StateMachine
{
    public class EntityState : ScriptableObject
    {
        private float m_Age = 0.0f;
        private float m_FixedAge = 0.0f;

        #region Properties
        protected float Age { get => m_Age; set => m_Age = value; }
        protected float FixedAge { get => m_FixedAge; set => m_FixedAge = value; }
        public EntityStateMachine EntityStateMachine { get; set; }
        #endregion

        #region Methods
        public static EntityState Instantiate(Type stateType)
        {
            if (stateType != null && stateType.IsSubclassOf(typeof(EntityState)))
            {
                EntityState entityState = Activator.CreateInstance(stateType) as EntityState;
                if (entityState != null)
                {
                    entityState.Initialize();
                }
                return entityState;
            }
            Debug.LogFormat("Bad stateType {0}", new object[]
            {
                (stateType == null) ? "null" : stateType.FullName
            });
            return null;
        }
        public static EntityState Instantiate(SerializableEntityStateType serializableEntityStateType)
        {
            return EntityState.Instantiate(serializableEntityStateType.StateType);
        }
        public virtual void Initialize()
        {

        }
        public virtual void OnEnter()
        {
        }
        public virtual void Update()
        {
            Age += Time.deltaTime;
        }
        public virtual void FixedUpdate()
        {
            FixedAge += Time.fixedDeltaTime;
        }
        public virtual void OnExit()
        {

        }
        public virtual void OnDrawGizmos()
        {

        }
        public virtual void OnDrawGizmosSelected()
        {

        }
        protected GameObject gameObject
        {
            get { return EntityStateMachine.gameObject; }
        }
        protected T GetComponent<T>() where T : Component
        {
            return EntityStateMachine.GetComponent<T>();
        }

        protected Component GetComponent(Type type)
        {
            return EntityStateMachine.GetComponent(type);
        }

        protected Component GetComponent(string type)
        {
            return EntityStateMachine.GetComponent(type);
        }
        #endregion
    }
}
