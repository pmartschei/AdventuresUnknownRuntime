using AdventuresUnknownSDK.Core.Objects.Enemies;
using AdventuresUnknownSDK.Core.Objects.Levels;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class LevelManager : SingletonBehaviour<LevelManager>
    {
        #region Properties
        public static Level CurrentLevel { get => Instance.CurrentLevelImpl; set => Instance.CurrentLevelImpl = value; }
        public static UnityEvent OnSuccess => Instance.OnSuccessImpl;
        public static UnityEvent OnFail => Instance.OnFailImpl;
        public static bool IsPaused = false;
        protected abstract Level CurrentLevelImpl { get; set; }
        protected abstract UnityEvent OnSuccessImpl { get; }
        protected abstract UnityEvent OnFailImpl { get; }
        #endregion

        #region Methods
        public static Level GenerateLevel(int difficulty)
        {
            return Instance.GenerateLevelImpl(difficulty);
        }
        protected abstract Level GenerateLevelImpl(int difficulty);

        public static void SetLevelGenerator(LevelGeneratorDescription levelGeneratorDescription)
        {
            Instance.SetLevelGeneratorImpl(levelGeneratorDescription);
        }

        public static void Success()
        {
            Instance.SuccessImpl();
        }

        public static void Fail()
        {
            Instance.FailImpl();
        }

        public static EnemyModel SpawnEnemy(Enemy enemy, Vector3 pos)
        {
            return Instance.SpawnEnemyImpl(enemy, pos);
        }

        public static Level GenerateFromCompletedLevel(CompletedLevel level)
        {
            return Instance.GenerateFromCompletedLevelImpl(level);
        }

        protected abstract void SetLevelGeneratorImpl(LevelGeneratorDescription levelGeneratorDescription);
        protected abstract void SuccessImpl();
        protected abstract void FailImpl();
        protected abstract EnemyModel SpawnEnemyImpl(Enemy enemy, Vector3 pos);
        protected abstract Level GenerateFromCompletedLevelImpl(CompletedLevel level);
        #endregion
    }
}
