using System;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Levels
{
    [Serializable]
    public class CompletedLevel
    {
        [SerializeField] private int m_Seed = 0;
        [SerializeField] private int m_Difficulty = 0;
        [SerializeField] private int m_EnemyLevel = 0;

        public int Seed { get => m_Seed; set => m_Seed = value; }
        public int Difficulty { get => m_Difficulty; set => m_Difficulty = value; }
        public int EnemyLevel { get => m_EnemyLevel; set => m_EnemyLevel = value; }
    }
}