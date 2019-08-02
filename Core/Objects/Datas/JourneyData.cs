using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Objects.Datas
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Datas/JourneyData", fileName = "JourneyData.asset")]
    public class JourneyData : IPlayerData
    {

        [SerializeField] private int m_Seed = 0;
        [SerializeField] private int m_Difficulty = 0;
        [NonSerialized] private List<Level> m_NextLevels = new List<Level>();

        [NonSerialized] private UnityEvent m_NextLevelChangeEvent = new UnityEvent();

        #region Properties
        public int Seed { get => m_Seed; set => m_Seed = value; }
        public Level[] NextLevels { get => m_NextLevels.ToArray(); }
        public UnityEvent OnNextLevelChange { get => m_NextLevelChangeEvent; set => m_NextLevelChangeEvent = value; }
        public int Difficulty { get => m_Difficulty; set => m_Difficulty = value; }
        #endregion

        #region Methods
        public override void Load()
        {
            JourneyData journeyData = FindScriptableObject<JourneyData>();
            if (!journeyData) return;
            journeyData.Seed = m_Seed;
            journeyData.Difficulty = m_Difficulty;
            GenerateNextLevels();
        }

        public void GenerateNextLevels()
        {
            m_NextLevels.Clear();
            UnityEngine.Random.InitState(m_Seed);
            m_NextLevels.Add(LevelManager.GenerateLevel(m_Difficulty));
            m_NextLevels.Add(LevelManager.GenerateLevel(m_Difficulty));
            m_NextLevels.Add(LevelManager.GenerateLevel(m_Difficulty));
            m_NextLevels.Add(LevelManager.GenerateLevel(m_Difficulty));
            m_NextLevels.Add(LevelManager.GenerateLevel(m_Difficulty));
            m_NextLevels.Add(LevelManager.GenerateLevel(m_Difficulty));
            m_NextLevels.Add(LevelManager.GenerateLevel(m_Difficulty));
            if (OnNextLevelChange != null)
                OnNextLevelChange.Invoke();
        }

        public override void Reset()
        {
            m_Difficulty = 0;
            m_Seed = 0;
        }
        #endregion
    }
}
