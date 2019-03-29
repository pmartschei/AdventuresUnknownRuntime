using AdventuresUnknownSDK.Core.Objects.Localization;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class GameSettingsManager : SingletonBehaviour<GameSettingsManager>
    {
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
