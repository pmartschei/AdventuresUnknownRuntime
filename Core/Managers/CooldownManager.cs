using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class CooldownManager : SingletonBehaviour<CooldownManager>
    {


        #region Properties

        #endregion

        #region Methods
        public static void AddCooldown(object source,float duration)
        {
            Instance.AddCooldownImpl(source,duration);
        }
        public static bool HasCooldown(object source)
        {
            return Instance.HasCooldownImpl(source);
        }
        public static float GetCooldown(object source)
        {
            return Instance.GetCooldownImpl(source);
        }
        protected abstract void AddCooldownImpl(object source, float duration);
        protected abstract bool HasCooldownImpl(object source);
        protected abstract float GetCooldownImpl(object source);
        #endregion
    }
}
