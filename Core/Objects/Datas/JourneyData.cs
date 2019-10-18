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
        [SerializeField] private List<CompletedLevel> m_CompletedLevels = new List<CompletedLevel>();
        [NonSerialized] private List<Level> m_NextLevels = new List<Level>();

        [NonSerialized] private UnityEvent m_NextLevelChangeEvent = new UnityEvent();
        [NonSerialized] private CompletedLevelEvent m_CompletedLevelAddEvent = new CompletedLevelEvent();

        #region Properties
        public int Seed { get => m_Seed; set => m_Seed = value; }
        public Level[] NextLevels { get => m_NextLevels.ToArray(); }
        public UnityEvent OnNextLevelChange { get => m_NextLevelChangeEvent; set => m_NextLevelChangeEvent = value; }
        public CompletedLevelEvent OnCompletedLevelAdd { get => m_CompletedLevelAddEvent; set => m_CompletedLevelAddEvent = value; }
        public int Difficulty { get => m_Difficulty; set => m_Difficulty = value; }
        public List<CompletedLevel> CompletedLevels { get => m_CompletedLevels; set => m_CompletedLevels = value; }
        #endregion

        #region Methods
        public override void Load()
        {
            JourneyData journeyData = FindScriptableObject<JourneyData>();
            if (!journeyData) return;
            journeyData.Seed = m_Seed;
            journeyData.Difficulty = m_Difficulty;
            journeyData.CompletedLevels = m_CompletedLevels;
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

        public void AddCompletedLevel(Level level)
        {
            CompletedLevel completedLevel = new CompletedLevel();
            completedLevel.Seed = level.Seed;
            completedLevel.Difficulty = level.Difficulty;
            completedLevel.EnemyLevel = level.EnemyLevel;
            m_CompletedLevels.Add(completedLevel);
            if (OnCompletedLevelAdd != null)
                OnCompletedLevelAdd.Invoke(completedLevel);
        }

        public override void Reset()
        {
            m_Difficulty = 0;
            m_Seed = 0;
            m_CompletedLevels.Clear();
        }
        #endregion
    }
}
