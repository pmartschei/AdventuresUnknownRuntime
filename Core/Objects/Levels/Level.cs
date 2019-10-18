using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Enemies;
using AdventuresUnknownSDK.Core.Objects.Localization;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Tags;
using AdventuresUnknownSDK.Core.UI.Items;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AdventuresUnknownSDK.Core.Objects.Inventories.ItemStack;

namespace AdventuresUnknownSDK.Core.Objects.Levels
{
    public abstract class Level
    {

        [SerializeField] private LocalizationString m_LevelPrefix = new LocalizationString();
        [SerializeField] private LocalizationString m_LevelName = new LocalizationString();
        [SerializeField] private LocalizationString m_LevelSuffix = new LocalizationString();
        [SerializeField] private int m_Difficulty = 0;
        [SerializeField] private int m_WaveCount = 0;
        [SerializeField] private int m_TimeBetweenWaves = 0;
        [SerializeField] private int m_EnemyLevel = 0;
        [SerializeField] private TagList m_TagList = null;
        //Seed will determine the waves and other things per level
        [SerializeField] private int m_Seed = 0;
        [SerializeField] private ValueMod[] m_Attributes = null;
        [SerializeField] private Enemy[] m_PossibleEnemies = null;
        [SerializeField] private AbstractLevelDisplay m_Display = null;
        [SerializeField] private float m_Width = 0.0f;
        [SerializeField] private float m_Height = 0.0f;


        #region Properties
        public int Difficulty { get => m_Difficulty; set => m_Difficulty = value; }
        public int WaveCount { get => m_WaveCount; set => m_WaveCount = value; }
        public int TimeBetweenWaves { get => m_TimeBetweenWaves; set => m_TimeBetweenWaves = value; }
        public int Seed { get => m_Seed; set => m_Seed = value; }
        public ValueMod[] Attributes { get => m_Attributes; set => m_Attributes = value; }
        public Enemy[] PossibleEnemies { get => m_PossibleEnemies; set => m_PossibleEnemies = value; }
        public LocalizationString LevelSuffix { get => m_LevelSuffix; set => m_LevelSuffix = value; }
        public LocalizationString LevelName { get => m_LevelName; set => m_LevelName = value; }
        public LocalizationString LevelPrefix { get => m_LevelPrefix; set => m_LevelPrefix = value; }
        public TagList TagList { get => m_TagList; set => m_TagList = value; }
        public AbstractLevelDisplay Display { get => m_Display; set => m_Display = value; }
        public int EnemyLevel { get => m_EnemyLevel; set => m_EnemyLevel = value; }
        public float Width { get => m_Width; set => m_Width = value; }
        public float Height { get => m_Height; set => m_Height = value; }
        #endregion


        #region Methods
        public abstract void Update();
        public abstract void Reset();
        public abstract void Build(Transform parent);
        #endregion
    }
}
