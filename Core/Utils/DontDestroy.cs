using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Utils
{
    public class DontDestroy : MonoBehaviour
    {

        #region Properties

        #endregion

        #region Methods

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        #endregion
    }
}
