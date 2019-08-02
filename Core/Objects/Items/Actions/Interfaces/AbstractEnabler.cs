using AdventuresUnknownSDK.Core.Objects.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Items.Interfaces
{
    public abstract class AbstractEnabler : ScriptableObject
    {
        #region Methods
        public abstract bool IsEnabled(ItemStack itemStack);
        #endregion
    }
}
