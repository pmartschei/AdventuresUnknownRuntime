using AdventuresUnknownSDK.Core.UI.Items;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class UIManager : SingletonBehaviour<UIManager>
    {


        #region Properties
        public static Transform Overlay
        {
            get => Instance.OverlayImpl;
        }
        public static AbstractItemStackDisplay MissingItemDisplay
        {
            get => Instance.MissingItemDisplayImpl;
        }
        public abstract Transform OverlayImpl { get; }
        public abstract AbstractItemStackDisplay MissingItemDisplayImpl { get; }
        #endregion

        #region Methods

        #endregion
    }
}
