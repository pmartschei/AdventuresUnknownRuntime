using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class LogManager : SingletonBehaviour<LogManager>
    {
        
        #region Methods
        public static void Log(string s)
        {
            Instance.LogImpl(s);
        }
        public static void LogWarning(string s)
        {
            Instance.LogWarningImpl(s);
        }
        public static void LogError(string s)
        {
            Instance.LogErrorImpl(s);
        }
        public static void LogFormat(string s, params object[] objs)
        {
            Instance.LogFormatImpl(s,objs);
        }
        public static void LogWarningFormat(string s, params object[] objs)
        {
            Instance.LogWarningFormatImpl(s, objs);
        }
        public static void LogErrorFormat(string s, params object[] objs)
        {
            Instance.LogErrorFormatImpl(s, objs);
        }
        public static void ClearLog()
        {
            Instance.ClearLogImpl();
        }
        protected abstract void LogImpl(string s);
        protected abstract void LogWarningImpl(string s);
        protected abstract void LogErrorImpl(string s);
        protected abstract void LogFormatImpl(string s,params object[] objs);
        protected abstract void LogWarningFormatImpl(string s, params object[] objs);
        protected abstract void LogErrorFormatImpl(string s, params object[] objs);
        protected abstract void ClearLogImpl();
        #endregion
    }
}
