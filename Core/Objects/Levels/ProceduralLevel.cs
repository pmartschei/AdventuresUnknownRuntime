using System;
using AdventuresUnknownSDK.Core.Objects.Localization;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using UnityEngine;
using Random = UnityEngine.Random;
using Attribute = AdventuresUnknownSDK.Core.Objects.Mods.Attribute;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Enemies;
using System.Collections.Generic;

namespace AdventuresUnknownSDK.Core.Objects.Levels
{
    public class ProceduralLevel : Level
    {

        [SerializeField] private int m_MaximumSpawnCredits = 0;
        [SerializeField] private int m_MinimumSpawnCredits = 0;

        private float m_CurrentWaveTimer = 0.0f;
        private int m_CurrentWave = 0;

        private List<EnemyModel> m_AliveEnemies = new List<EnemyModel>();
        private Wave m_NextWave = null;

        public class Wave{
            public int Index;
            public Enemy Enemy;
            public int Count;
        }
        #region Properties
        public int MaximumSpawnCredits { get => m_MaximumSpawnCredits; set => m_MaximumSpawnCredits = value; }
        public int MinimumSpawnCredits { get => m_MinimumSpawnCredits; set => m_MinimumSpawnCredits = value; }
        public Wave NextWave {
            get { 
                if (m_NextWave == null)
                {
                    if (m_CurrentWave < WaveCount)
                        GenerateNextWave(m_CurrentWave);
                }
                else
                {
                    if (m_CurrentWave < WaveCount)
                    {
                        if (m_NextWave.Index != m_CurrentWave)
                            GenerateNextWave(m_CurrentWave);
                    }
                    else
                    {
                        m_NextWave = null;
                    }
                }
                return m_NextWave;
            }
            set => m_NextWave = value; }
        public int CurrentWave { get => m_CurrentWave; set => m_CurrentWave = value; }
        public float CurrentWaveTimer { get => m_CurrentWaveTimer; set => m_CurrentWaveTimer = value; }
        #endregion

        #region Methods
        public ProceduralLevel(int difficulty)
        {
            this.Difficulty = difficulty;
        }
        
        public override void Update()
        {
            if (LevelManager.IsPaused) return;
            if (NextWave == null)
            {
                for(int i = 0; i < m_AliveEnemies.Count; i++)
                {
                    if (m_AliveEnemies[i].EntityBehaviour == null || m_AliveEnemies[i].EntityBehaviour.Entity.IsDead)
                    {
                        m_AliveEnemies.RemoveAt(i);
                        i--;
                    }
                }
                if (m_AliveEnemies.Count == 0)
                {
                    LevelManager.IsPaused = true;
                    LevelManager.Success();
                }
            }
            else
            {
                m_CurrentWaveTimer -= Time.deltaTime;
                if (m_CurrentWaveTimer <= 0.0f)
                {
                    m_CurrentWaveTimer = TimeBetweenWaves;
                    SpawnWave(NextWave);
                    m_CurrentWave++;
                }
            }
        }

        public override void Reset()
        {
            m_CurrentWave = 0;
            m_CurrentWaveTimer = TimeBetweenWaves;
            m_NextWave = null;
        }

        private void GenerateNextWave(int waveIndex)
        {
            if (PossibleEnemies == null) return;

            Random.InitState(Seed + Difficulty + waveIndex);

            int enemyRoll = Random.Range(0, PossibleEnemies.Length);

            int spawnCredits = Random.Range(MinimumSpawnCredits, MaximumSpawnCredits);
            int enemyCount = 10;
            if (PossibleEnemies[enemyRoll].Difficulty != 0)
            {
                enemyCount = spawnCredits / PossibleEnemies[enemyRoll].Difficulty;
            }

            Wave wave = new Wave();
            wave.Enemy = PossibleEnemies[enemyRoll];
            wave.Count = enemyCount;
            wave.Index = waveIndex;
            m_NextWave = wave;
        }

        private void SpawnWave(Wave wave)
        {
            Vector2 centerPosition = Random.insideUnitCircle * new Vector2(Width * 0.5f,Height * 0.5f);
            float roll = Random.Range(0.05f + wave.Enemy.Difficulty * 0.010f, 0.15f + wave.Enemy.Difficulty * 0.05f);
            Vector2 spawnSize = new Vector2(Width * roll, Height * roll);
            for (int i = 0; i < wave.Count; i++)
            {
                Vector2 spawnPosition = centerPosition + Random.insideUnitCircle * spawnSize;
                EnemyModel em = LevelManager.SpawnEnemy(wave.Enemy, spawnPosition);
                m_AliveEnemies.Add(em);
            }
        }

        #endregion
    }
}
