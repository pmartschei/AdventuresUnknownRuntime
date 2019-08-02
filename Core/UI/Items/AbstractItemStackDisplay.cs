using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI.Items
{
    public abstract class AbstractItemStackDisplay : MonoBehaviour
    {


        #region Properties

        #endregion

        #region Methods
        public abstract bool Display(ItemStack itemStack);
        #endregion
    }
}
