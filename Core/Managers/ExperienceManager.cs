using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class ExperienceManager : SingletonBehaviour<ExperienceManager>
    {

        #region Properties
        public static int MaxLevel => Instance.MaxLevelImpl;

        public abstract int MaxLevelImpl { get; }
        #endregion

        #region Methods
        public static int GetExperienceForLevel(int level)
        {
            return Instance.GetExperienceForLevelImpl(level);
        }

        public abstract int GetExperienceForLevelImpl(int level);

        #endregion

    }
}
