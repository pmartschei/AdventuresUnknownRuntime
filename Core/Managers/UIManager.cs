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
        public static Transform HealthBarsTransform
        {
            get => Instance.HealthBarsTransformImpl;
            set => Instance.HealthBarsTransformImpl = value;
        }
        public static Transform ItemDropsTransform
        {
            get => Instance.ItemDropsTransformImpl;
            set => Instance.ItemDropsTransformImpl = value;
        }
        public static Transform EntityTransform
        {
            get => Instance.EntityTransformImpl;
            set => Instance.EntityTransformImpl = value;
        }
        public static Transform AttacksTransform
        {
            get => Instance.AttacksTransformImpl;
            set => Instance.AttacksTransformImpl = value;
        }
        public abstract Transform OverlayImpl { get; }
        public abstract AbstractItemStackDisplay MissingItemDisplayImpl { get; }
        public abstract Transform HealthBarsTransformImpl { get; set; }
        public abstract Transform ItemDropsTransformImpl { get; set; }
        public abstract Transform EntityTransformImpl { get; set; }
        public abstract Transform AttacksTransformImpl { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
