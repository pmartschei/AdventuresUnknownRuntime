using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods;
using System;
using System.Collections;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Enemies
{
    public class SpawnObject : MonoBehaviour
    {
        [SerializeField] private float m_SpawnDelay = 0.5f;
        [SerializeField] private float m_CooldownMin = 0.5f;
        [SerializeField] private float m_CooldownMax = 0.5f;
        protected Action<EnemyModel> m_EnemySpawnEvent;
        public bool IsSpawning { get; protected set; }

        public Enemy Enemy { get; set; }

        public int Count { get; set; }

        public int EnemyLevel { get; set; }

        public int SpawnedEnemies { get; protected set; }
        public float SpawnDelay { get => m_SpawnDelay; }
        public float CooldownMin { get => m_CooldownMin; set => m_CooldownMin = value; }
        public float CooldownMax { get => m_CooldownMax; set => m_CooldownMax = value; }

        public event Action<EnemyModel> OnEnemySpawn { add => m_EnemySpawnEvent += value; remove => m_EnemySpawnEvent -= value; }

        void Start()
        {
            IsSpawning = true;
            SpawnedEnemies = 0;
            StartCoroutine(Spawn());
        }

        protected virtual IEnumerator Spawn()
        {
            yield return new WaitForSeconds(m_SpawnDelay);
            for (int i = 0; i < Count; i++)
            {
                EnemyModel em = LevelManager.SpawnEnemy(Enemy, this.transform.position);
                em.EntityBehaviour.Entity.GetStat("core.modtypes.utility.level").AddStatModifier(new StatModifier(EnemyLevel, CalculationType.Flat, this));
                CooldownManager.AddCooldown(em.EntityController, UnityEngine.Random.Range(m_CooldownMin,m_CooldownMax));
                m_EnemySpawnEvent?.Invoke(em);
                SpawnedEnemies++;
                yield return new WaitForSeconds(m_SpawnDelay);
            }
            IsSpawning = false;
            GameObject.Destroy(this.gameObject);
            yield break;
        }
    }
}