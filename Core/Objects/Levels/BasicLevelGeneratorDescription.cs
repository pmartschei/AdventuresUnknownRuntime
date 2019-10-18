using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Enemies;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Tags;
using AdventuresUnknownSDK.Core.UI.Items;
using AdventuresUnknownSDK.Core.Utils;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AdventuresUnknownSDK.Core.Objects.Inventories.ItemStack;
using Random = UnityEngine.Random;

namespace AdventuresUnknownSDK.Core.Objects.Levels
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Levels/LevelGenerator/Basic", fileName = "BasicLevelGenerator.asset")]
    public class BasicLevelGeneratorDescription : LevelGeneratorDescription
    {
        [SerializeField] private AbstractLevelDisplay m_LevelDisplay = null;
        [SerializeField] private AnimationCurve m_MinimumWaveCount = null;
        [SerializeField] private AnimationCurve m_MaximumWaveCount = null;
        [SerializeField] private AnimationCurve m_MinimumWaveDelay = null;
        [SerializeField] private AnimationCurve m_MaximumWaveDelay = null;
        [SerializeField] private AnimationCurve m_MinimumAttributeCount = null;
        [SerializeField] private AnimationCurve m_MaximumAttributeCount = null;
        [SerializeField] private AnimationCurve m_MinimumSpawnCredits = null;
        [SerializeField] private AnimationCurve m_MaximumSpawnCredits = null;
        [SerializeField] private AnimationCurve m_MinimumAdditionalEnemiesLevel = null;
        [SerializeField] private AnimationCurve m_MaximumAdditionalEnemiesLevel = null;
        [SerializeField] private AnimationCurve m_MinimumTagCount = null;
        [SerializeField] private AnimationCurve m_MaximumTagCount = null;
        [SerializeField] private AnimationCurve m_MaximumEnemyDifficulty = null;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_ChanceToPickAttributeFromTag = 0.85f;
        [SerializeField] private TagIdentifier m_DefaultTag = null;
        [SerializeField] private List<TagSpawnWeight> m_TagSpawnWeights = new List<TagSpawnWeight>();
        [SerializeField] private string[] m_LevelNames = null;
        [SerializeField] private string[] m_LevelPrefixes = null;
        [SerializeField] private string[] m_LevelSuffixes = null;


        [Serializable]
        public struct TagSpawnWeight
        {
            public TagIdentifier TagIdentifier;
            public int Weight;
            public int RequiredDifficulty;
        }
        #region Properties

        #endregion

        /*How should Difficulty work:
         * 1: Start of the game, the difficulty should be easily and almost no modifiers should apply and the easiest enemies
         *  Waves (2 to 4), lvl most likely lower playerlevel, 0 tags, easy enemies
         * 5: Players should dodge more often and get better
         *  Waves (4 to 5), lvl most likely lowerEquals playerlevel, 1 tag sometimes,easy enemies
         * 20: Average Player
         *  Waves (4 to 10), lvl most likely equals playerlevel, 1-2 tags, middle enemies
         * 40: 95% Player
         *  Waves (8 to 15), lvl most likely Higherequals playerlevel, 2-3 tags, middle and hard enemies
         * 50+: Getting crazier and you need the best of the best skills etc.
         *  Waves (10 to 25), lvl most likely Higher playerlevel, 3 tags, all enemies
         * Each round should last between 2-3 minutes
         * 
         */
        #region Methods
        public override bool ConsistencyCheck()
        {
            base.ConsistencyCheck();
            foreach(TagSpawnWeight tsw in m_TagSpawnWeights)
            {
                tsw.TagIdentifier.ConsistencyCheck();
            }
            m_DefaultTag.ConsistencyCheck();
            return true;
        }
        public override void Init(ProceduralLevel level)
        {
            level.Display = m_LevelDisplay;
        }
        public override void GenerateAttributes(ProceduralLevel level)
        {
            //check if attribute from tag or random
            float minAttributeCount = m_MinimumAttributeCount.Evaluate(level.Difficulty);
            float maxAttributeCount = m_MaximumAttributeCount.Evaluate(level.Difficulty);

            int attributeCount = (int)Math.Round(Random.Range(minAttributeCount, maxAttributeCount));

            List<ValueMod> attributes = new List<ValueMod>();

            for(int i = 0; i < attributeCount; i++)
            {
                float roll = Random.Range(0.0f, 1.0f);
                Mod[] rollableMods = null;
                string tag = "";
                if (roll <= m_ChanceToPickAttributeFromTag)
                {
                    if (level.TagList.Count > 0)
                    {
                        int tagRoll = Random.Range(0, level.TagList.Count);
                        rollableMods = ModifierManager.GetModifiersForDomainAndTag(2, level.TagList[tagRoll].Identifier);
                        tag = level.TagList[tagRoll].Identifier;
                    }
                    if (rollableMods == null || rollableMods.Length == 0)
                    {
                        rollableMods = ModifierManager.GetModifiersForDomain(2);
                        tag = m_DefaultTag.Identifier;
                    }
                }
                else
                {
                    rollableMods = ModifierManager.GetModifiersForDomain(2);
                    tag = m_DefaultTag.Identifier;
                }

                if (rollableMods == null) continue;
                ValueMod valueMod = ModUtils.Roll(rollableMods.ToArray(), tag);
                if (valueMod == null) continue;
                attributes.Add(valueMod);

            }

            level.Attributes = attributes.ToArray();
        }

        public override void GenerateName(ProceduralLevel level)
        {
            int roll = Random.Range(0, m_LevelNames.Length);

            level.LevelName.LocalizedIdentifier = m_LevelNames[roll];

            if (m_LevelPrefixes.Length > 0 )//&& Random.Range(0,1.0f) >= 0.5f)
            {
                roll = Random.Range(0, m_LevelPrefixes.Length);
                level.LevelPrefix.LocalizedIdentifier = m_LevelPrefixes[roll];
            }
            //if (m_LevelSuffixes.Length > 0 && Random.Range(0, 1.0f) >= 0.5f)
            //{
            //    roll = Random.Range(0, m_LevelSuffixes.Length);
            //    level.LevelSuffix.LocalizedIdentifier = m_LevelSuffixes[roll];
            //}
        }

        public override void GeneratePossibleEnemies(ProceduralLevel level)
        {
            float maxEnemyDifficulty = m_MaximumEnemyDifficulty.Evaluate(level.Difficulty);

            Enemy[] enemies = ObjectsManager.GetAllObjects<Enemy>();
            List<Enemy> possibleEnemies = new List<Enemy>();
            foreach(Enemy enemy in enemies)
            {
                if (!enemy.IsSpawnable) continue;
                if (enemy.Difficulty > maxEnemyDifficulty)
                {
                    continue;
                }
                possibleEnemies.Add(enemy);
            }
            if (possibleEnemies.Count == 0) return;

            possibleEnemies.Sort((a,b) => { return a.Difficulty < b.Difficulty ? 1 : -1; });

            List<Enemy> selectedEnemies = new List<Enemy>();
            float chanceForSelection = 0.8f;
            float reductionPerSelection = 0.75f;

            foreach(Enemy enemy in possibleEnemies)
            {
                if (Random.Range(0.0f, 1.0f) <= chanceForSelection)
                {
                    selectedEnemies.Add(enemy);
                    chanceForSelection *= reductionPerSelection;
                }
            }

            if (selectedEnemies.Count == 0)
            {
                selectedEnemies.Add(possibleEnemies[0]);
            }

            level.PossibleEnemies = selectedEnemies.ToArray();
        }

        public override void GenerateTags(ProceduralLevel level)
        {
            List<TagSpawnWeight> tags = new List<TagSpawnWeight>();
            foreach(TagSpawnWeight tsw in m_TagSpawnWeights)
            {
                if (tsw.RequiredDifficulty > level.Difficulty) continue;
                tags.Add(tsw);
            }
            int weight = 0;
            for(int i = 0; i < tags.Count; i++)
            {
                weight += tags[i].Weight;
            }
            float minTagCount = m_MinimumTagCount.Evaluate(level.Difficulty);
            float maxTagCount = m_MaximumTagCount.Evaluate(level.Difficulty);

            int tagCount = (int)Math.Round(Random.Range(minTagCount, maxTagCount));

            TagList tagList = new TagList();
            for (int i = 0; i < tagCount; i++)
            {
                if (tags.Count == 0) break;
                int roll = (int)(Random.Range(0, weight));
                int tagIndex = 0;
                while(roll > tags[tagIndex].Weight)
                {
                    roll -= tags[tagIndex].Weight;
                    tagIndex++;
                }
                TagIdentifier tagIdentifier = tags[tagIndex].TagIdentifier;
                tagList.Add(tagIdentifier.Object);
                weight -= tags[tagIndex].Weight;
                tags.RemoveAt(tagIndex);
            }
            if (tagList.Count == 0)
            {
                tagList.Add(m_DefaultTag.Object);
            }
            level.TagList = tagList;
        }

        public override void GenerateWaveCount(ProceduralLevel level)
        {
            float minWaveCount =  m_MinimumWaveCount.Evaluate(level.Difficulty);
            float maxWaveCount = m_MaximumWaveCount.Evaluate(level.Difficulty);

            level.WaveCount = (int)Math.Round(Random.Range(minWaveCount, maxWaveCount));
        }

        public override void GenerateWaveDelay(ProceduralLevel level)
        {
            float minWaveDelay = m_MinimumWaveDelay.Evaluate(level.Difficulty);
            float maxWaveDelay = m_MaximumWaveDelay.Evaluate(level.Difficulty);

            level.Width = 40f;
            level.Height = 40f;

            level.TimeBetweenWaves = (int)Math.Round(Random.Range(minWaveDelay, maxWaveDelay));
        }

        public override void GenerateEnemyLevel(ProceduralLevel level, int playerLevel)
        {
            float minEnemyLevel = m_MinimumAdditionalEnemiesLevel.Evaluate(level.Difficulty);
            float maxEnemyLevel = m_MaximumAdditionalEnemiesLevel.Evaluate(level.Difficulty);

            level.EnemyLevel =  playerLevel + (int)Math.Round(Random.Range(minEnemyLevel, maxEnemyLevel));
        }

        public override void GenerateSpawnCredits(ProceduralLevel level)
        {
            level.MinimumSpawnCredits = (int)m_MinimumSpawnCredits.Evaluate(level.Difficulty);
            level.MaximumSpawnCredits = (int)m_MaximumSpawnCredits.Evaluate(level.Difficulty);
        }
        #endregion
    }
}
