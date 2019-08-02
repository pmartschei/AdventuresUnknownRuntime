using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI.Items
{
    public abstract class AbstractActiveGemDisplay : MonoBehaviour
    {


        #region Properties

        #endregion

        #region Methods
        public abstract bool Display(ActiveGem activeGem, Entity stats,string[] modTypes);
        #endregion
    }
}
