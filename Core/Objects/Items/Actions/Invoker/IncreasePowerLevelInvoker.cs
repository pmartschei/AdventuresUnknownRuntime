﻿using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items.Interfaces;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Tags;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AdventuresUnknownSDK.Core.Objects.Inventories.ItemStack;

namespace AdventuresUnknownSDK.Core.Objects.Items.Actions.Invoker
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Actions/Invoker/IncreasePowerLevelInvoker", fileName = "IncreasePowerLevelInvoker.asset")]
    public class IncreasePowerLevelInvoker : AbstractInvoker
    {
        [SerializeField] private int m_Count = 1;

        #region Properties

        #endregion

        #region Methods
        public override void Invoke(ItemStack itemStack)
        {
            itemStack.PowerLevel += m_Count;
        }
        #endregion
    }
}
