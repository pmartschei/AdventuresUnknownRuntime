using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI.Statics
{
    public class UIRegisterAsAttackTransform : MonoBehaviour
    {


        #region Properties

        #endregion

        #region Methods
        private void Awake()
        {
            UIManager.AttacksTransform = this.transform;
        }
        #endregion
    }
}
