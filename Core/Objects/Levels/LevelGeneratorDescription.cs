using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Levels
{
    public abstract class LevelGeneratorDescription : CoreObject
    {


        #region Properties

        #endregion

        #region Methods
        public abstract void Init(ProceduralLevel level);
        public abstract void GenerateName(ProceduralLevel level);
        public abstract void GenerateWaveDelay(ProceduralLevel level);
        public abstract void GenerateWaveCount(ProceduralLevel level);
        public abstract void GenerateTags(ProceduralLevel level);
        public abstract void GenerateAttributes(ProceduralLevel level);
        public abstract void GeneratePossibleEnemies(ProceduralLevel level);
        public abstract void GenerateEnemyLevel(ProceduralLevel level,int playerLevel);
        public abstract void GenerateSpawnCredits(ProceduralLevel level);
        #endregion
    }
}
