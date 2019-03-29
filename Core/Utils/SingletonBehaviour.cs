
using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Utils
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_T;
        #region Properties
        public static T Instance
        {
            get { return FindSingleton(); }
        }
        #endregion

        #region Methods
        private static T FindSingleton()
        {
            if (!m_T)
            {
                m_T = FindObjectOfType<T>();
                if (!m_T)
                {
                    GameConsole.LogError("Could not find a Singleton Object for Type " + typeof(T));
                }
            }
            return m_T;
        }
        #endregion
    }
}
