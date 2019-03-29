using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Utils
{
    public abstract class LocalizedMonoBehaviour : MonoBehaviour, ILocalize
    {
        public abstract void OnLanguageChange();
    }
}
