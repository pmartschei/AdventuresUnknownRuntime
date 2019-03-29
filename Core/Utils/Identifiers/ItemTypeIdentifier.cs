using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Items;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class ItemTypeIdentifier : ObjectIdentifier
    {


        #region Properties
        [SerializeField]
        public new ItemType Object => base.Object as ItemType;

        #endregion

        #region Methods

        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(ItemType) };
        }
        #endregion
    }
}
