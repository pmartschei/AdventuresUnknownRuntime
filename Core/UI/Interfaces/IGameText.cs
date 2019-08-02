using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI.Interfaces
{
    public abstract class IGameText : MonoBehaviour
    {
        #region Methods
        public abstract void SetText(object obj);
        #endregion
    }
}
