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
using System.Collections;
using AdventuresUnknownSDK.Core.Entities;

namespace AdventuresUnknownSDK.Core.Objects.Levels
{
    public class ProceduralLevel : Level
    {
        [SerializeField] private int m_MaximumSpawnCredits = 0;
        [SerializeField] private int m_MinimumSpawnCredits = 0;

        private float m_CurrentWaveTimer = 0.0f;
        private int m_CurrentWave = 0;

        private List<EnemyModel> m_AliveEnemies = new List<EnemyModel>();
        private List<SpawnObject> m_SpawnObjects = new List<SpawnObject>();
        private Wave m_NextWave = null;

        private bool m_IsSpawning = false;

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
        public int RemainingEnemies { get => m_AliveEnemies.Count;}
        #endregion

        #region Methods
        public ProceduralLevel(int difficulty)
        {
            this.Difficulty = difficulty;
        }
        
        public override void Update()
        {
            if (LevelManager.IsPaused) return;
            for (int i = 0; i < m_AliveEnemies.Count; i++)
            {
                if (m_AliveEnemies[i].EntityBehaviour == null || m_AliveEnemies[i].EntityBehaviour.Entity.IsDead)
                {
                    m_AliveEnemies.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < m_SpawnObjects.Count; i++)
            {
                if (!m_SpawnObjects[i].IsSpawning)
                {
                    m_SpawnObjects.RemoveAt(i);
                    i--;
                }
            }
            if (NextWave == null)
            {
                if (RemainingEnemies == 0 && m_SpawnObjects.Count == 0)
                {
                    LevelManager.IsPaused = true;
                    LevelManager.Success();
                }
            }
            else
            {
                if (RemainingEnemies == 0 && m_CurrentWaveTimer > 3.0f)
                {
                    m_CurrentWaveTimer = 3.0f;
                }
                else
                {
                    m_CurrentWaveTimer -= Time.deltaTime;
                }
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

            float spawnCredits = Random.Range(MinimumSpawnCredits, MaximumSpawnCredits);
            float enemyCount = 10;
            if (PossibleEnemies[enemyRoll].Difficulty != 0)
            {
                enemyCount = spawnCredits / (float)PossibleEnemies[enemyRoll].Difficulty;
                enemyCount = Mathf.Max(enemyCount, 1);
            }

            Wave wave = new Wave();
            wave.Enemy = PossibleEnemies[enemyRoll];
            wave.Count = Mathf.RoundToInt(enemyCount);
            wave.Index = waveIndex;
            m_NextWave = wave;
        }

        private void SpawnWave(Wave wave)
        {
            Random.InitState((int)DateTime.Now.Ticks);
            Vector2 centerPosition = Random.insideUnitCircle * new Vector2(Width * 0.45f,Height * 0.45f);

            SpawnObject so = GameObject.Instantiate(wave.Enemy.SpawnObject,new Vector3(centerPosition.x,0.0f,centerPosition.y),Quaternion.identity, UIManager.EntityTransform);
            so.Enemy = wave.Enemy;
            so.Count = wave.Count;
            so.EnemyLevel = EnemyLevel;
            so.OnEnemySpawn += OnEnemySpawn;
            m_SpawnObjects.Add(so);
        }

        private void OnEnemySpawn(EnemyModel obj)
        {
            m_AliveEnemies.Add(obj);
        }

        public override void Build(Transform parent)
        {
            GameObject southWall = GameObject.Instantiate(LevelObjectManager.Wall, parent);
            GameObject northWall = GameObject.Instantiate(LevelObjectManager.Wall, parent);
            GameObject westWall = GameObject.Instantiate(LevelObjectManager.Wall, parent);
            GameObject eastWall = GameObject.Instantiate(LevelObjectManager.Wall, parent);
            southWall.name = "SouthWall";
            northWall.name = "NorthWall";
            westWall.name = "WestWall";
            eastWall.name = "EastWall";
            float extHeight = Height + 1;
            float extWidth = Width + 1;
            southWall.transform.position = new Vector3(0, 0, -extWidth / 2);
            northWall.transform.position = new Vector3(0, 0, extWidth / 2);
            westWall.transform.position = new Vector3(-extHeight / 2, 0,0);
            eastWall.transform.position = new Vector3(extHeight / 2, 0,0);
            southWall.transform.localScale = new Vector3(extHeight, 1, 0.05f);
            northWall.transform.localScale = new Vector3(extHeight, 1, 0.05f);
            westWall.transform.localScale = new Vector3(0.05f, 1, extWidth);
            eastWall.transform.localScale = new Vector3(0.05f, 1, extWidth);
        }
        #endregion
    }
}
