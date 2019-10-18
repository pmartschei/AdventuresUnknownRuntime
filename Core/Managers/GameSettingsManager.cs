using AdventuresUnknownSDK.Core.Objects.Localization;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Utils;
using AdventuresUnknownSDK.Core.Utils.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class GameSettingsManager : SingletonBehaviour<GameSettingsManager>
    {

        protected bool m_IsPaused = false;
        protected BoolEvent m_OnPauseChangeEvent = new BoolEvent();
        #region Properties
        public static LocalizationLanguage Language
        {
            get
            {
                return Instance.LanguageImpl;
            }
            set
            {
                Instance.LanguageImpl = value;
            }
        }
        public static ModTypeFormatter DefaultModTypeFormatter
        {
            get
            {
                return Instance.DefaultModTypeFormatterImpl;
            }
        }

        public static bool IsPaused { get => Instance.m_IsPaused;
            set
            {
                if (Instance.m_IsPaused != value)
                {
                    Instance.m_IsPaused = value;
                    if (Instance.m_OnPauseChangeEvent!= null)
                    {
                        Instance.m_OnPauseChangeEvent.Invoke(value);
                    }
                }
            }
        }
        public static BoolEvent OnPauseChangeEvent { get => Instance.m_OnPauseChangeEvent; }
        public static UnityEvent OnLanguageChange
        {
            get
            {
                return Instance.OnLanguageChangeImpl;
            }
        }
        protected abstract LocalizationLanguage LanguageImpl { get; set; }
        protected abstract ModTypeFormatter DefaultModTypeFormatterImpl { get; }
        protected abstract UnityEvent OnLanguageChangeImpl { get; }
        #endregion

        #region Methods

        #endregion
    }
}
