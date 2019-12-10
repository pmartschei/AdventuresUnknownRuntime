using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.GameModes;
using AdventuresUnknownSDK.Core.Utils.UnityEvents;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Objects.Datas
{
    [CreateAssetMenu(menuName="AdventuresUnknown/Core/Datas/ContextData",fileName="ContextData.asset")]
    public class ContextData : IPlayerData
    {
        [SerializeField] private string m_ShipName = "";
        [SerializeField] private int m_Level = 1;
        [SerializeField] private int m_Experience = 0;
        [SerializeField] private float m_PlayTime = 0;
        [SerializeField] private string m_GameMode = "";

        [NonSerialized]
        private string m_SaveFileName = "";

        [NonSerialized]
        private IntEvent m_OnLevelChange = new IntEvent();
        [NonSerialized]
        private IntEvent m_OnExperienceChange = new IntEvent();
        [NonSerialized]
        private FloatEvent m_OnPlayTimeChange = new FloatEvent();


        #region Properties

        public string ShipName { get => m_ShipName; set => m_ShipName = value; }
        public int Level
        {
            get => m_Level;
            set
            {
                if (m_Level != value)
                {
                    m_Level = value;
                    m_OnLevelChange.Invoke(m_Level);
                }
            }
        }
        public int Experience {
            get => m_Experience;
            set
            {
                if (m_Experience != value)
                {
                    if (m_Level < ExperienceManager.MaxLevel)
                    {
                        m_Experience = value;
                        int requiredExperience = ExperienceManager.GetExperienceForLevel(m_Level + 1);
                        while (m_Experience >= requiredExperience)
                        {
                            Level++;
                            m_Experience -= requiredExperience;
                            requiredExperience = ExperienceManager.GetExperienceForLevel(m_Level + 1);
                        }
                        m_OnExperienceChange.Invoke(m_Experience);
                    }
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
                    m_OnPlayTimeChange.Invoke(m_PlayTime);
                }
            }
        }
        public IntEvent OnLevelChange { get => m_OnLevelChange; }
        public IntEvent OnExperienceChange { get => m_OnExperienceChange;}
        public FloatEvent OnPlayTimeChange { get => m_OnPlayTimeChange; }
        public string GameMode { get => m_GameMode; set => m_GameMode = value; }
        public string SaveFileName { get => m_SaveFileName; set => m_SaveFileName = value; }

        #endregion

        #region Methods
        public override void Reset()
        {
            m_Level = 1;
            m_Experience = 0;
            m_GameMode = "";
            m_ShipName = "";
            m_SaveFileName = "";
            m_PlayTime = 0;
        }
        public override void Load()
        {
            ContextData contextData = FindScriptableObject<ContextData>();
            if (!contextData) return;
            contextData.ShipName = this.ShipName;
            contextData.Level = this.Level;
            contextData.Experience = this.Experience;
            contextData.PlayTime = this.PlayTime;
            contextData.GameMode = this.GameMode;
            contextData.SaveFileName = this.SaveFileName;
            contextData.OnValidate();
        }

        private void OnValidate()
        { 
            m_OnExperienceChange.Invoke(m_Experience);
            m_OnLevelChange.Invoke(m_Level);
            m_OnPlayTimeChange.Invoke(m_PlayTime);
        }
        #endregion
    }
}
