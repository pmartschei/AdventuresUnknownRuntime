using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class SceneManager : SingletonBehaviour<SceneManager>
    {
        #region Methods
        public static void LoadScene(string sceneName)
        {
            Instance.LoadSceneImpl(sceneName);
        }
        public static bool IsValidScene(string sceneName)
        {
            return Instance.IsValidSceneImpl(sceneName);
        }
        protected abstract void LoadSceneImpl(string sceneName);
        protected abstract bool IsValidSceneImpl(string sceneName);
        #endregion
    }
}
