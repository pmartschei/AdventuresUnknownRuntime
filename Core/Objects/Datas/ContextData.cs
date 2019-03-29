using AdventuresUnknownSDK.Core.Datas;
using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Objects.Datas
{
    [CreateAssetMenu(menuName="AdventuresUnknown/Core/Datas/ContextData",fileName="ContextData.asset")]
    public class ContextData : IPlayerData
    {
        [SerializeField] private string m_ShipName = "";
        [SerializeField] private int m_Level = 0;
        [SerializeField] private int m_Experience = 0;
        [SerializeField] private float m_PlayTime = 0;

        [NonSerialized]
        private UnityEvent m_OnLevelChange = new UnityEvent();
        [NonSerialized]
        private UnityEvent m_OnExperienceChange = new UnityEvent();
        [NonSerialized]
        private UnityEvent m_OnPlayTimeChange = new UnityEvent();


        #region Properties

        public string ShipName { get => m_ShipName; private set => m_ShipName = value; }
        public int Level
        {
            get => m_Level;
            set
            {
                if (m_Level != value)
                {
                    m_Level = value;
                    m_OnLevelChange.Invoke();
                }
            }
        }
        public int Experience {
            get => m_Experience;
            set
            {
                if (m_Experience != value)
                {
                    m_Experience = value;
                    m_OnExperienceChange.Invoke();
                }
            }
        }
        public float PlayTime {
            get => m_PlayTime;
            set
            {
                if (m_PlayTime != value)
                {
                    m_PlayTime = value;
                    m_OnPlayTimeChange.Invoke();
                }
            }
        }
        public UnityEvent OnLevelChange { get => m_OnLevelChange; }
        public UnityEvent OnExperienceChange { get => m_OnExperienceChange;}
        public UnityEvent OnPlayTimeChange { get => m_OnPlayTimeChange; }
        #endregion

        #region Methods
        public override bool OnAfterDeserialize()
        {
            ContextData contextData = FindScriptableObject<ContextData>();
            if (!contextData) return false;
            contextData.ShipName = this.ShipName;
            contextData.Level = this.Level;
            contextData.Experience = this.Experience;
            contextData.PlayTime = this.PlayTime;
            return true;
        }
        #endregion
    }
}
