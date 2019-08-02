using System;
using System.Reflection;
using UnityEngine;

//Source RoR2

namespace AdventuresUnknownSDK.Core.Entities.StateMachine
{
    [Serializable]
    public class SerializableEntityStateType
    {

        [SerializeField] private string m_TypeName = "";

        public SerializableEntityStateType(string typeName)
        {
            m_TypeName = typeName;
        }
        public SerializableEntityStateType(Type stateType)
        {
            StateType = stateType;
        }

        public Type StateType
        {
            get
            {
                if (m_TypeName == null)
                {
                    return null;
                }
                Type type = Type.GetType(m_TypeName);//HMM
                if (type == null || !type.IsSubclassOf(typeof(EntityState)))
                {
                    return null;
                }
                return type;
            }
            set
            {
                this.m_TypeName = (value != null && value.IsSubclassOf(typeof(EntityState)) ? value.FullName : "");
            }
        }

    }
}