using AdventuresUnknownSDK.Core.Objects.Localization;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class LocalizationsManager : SingletonBehaviour<LocalizationsManager>
    {


        #region Properties

        #endregion

        #region Methods
        public static string Localize(string identifier)
        {
            return Instance.LocalizeImpl(identifier);
        }
        public static string Localize(string identifier,LocalizationLanguage language)
        {
            return Instance.LocalizeImpl(identifier,language);
        }
        protected abstract string LocalizeImpl(string identifier);
        protected abstract string LocalizeImpl(string identifier,LocalizationLanguage language);
        #endregion
    }
}
