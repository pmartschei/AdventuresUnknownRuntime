using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.Weapons;
using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Objects.Pool;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AdventuresUnknownSDK.Core.Objects.Inventories.ItemStack;
using Attribute = AdventuresUnknownSDK.Core.Objects.Mods.Attribute;

namespace AdventuresUnknownSDK.Core.Objects.Items
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/AuraGem", fileName = "AuraGem.asset")]
    public class AuraGem : ActiveGem
    {

        #region Methods
        public override void Activate(EntityController entityController, Entity stats ,float level = 0.0f, ulong ID = 0, params Muzzle[] muzzles)
        {
            Entity spaceShip = entityController.Entity;
            if (spaceShip.GetObject<ActiveGem>(this) == this)
            {
                spaceShip.RemoveObject(this);
                spaceShip.RemoveAllModifiersBySource(this);
            }
            else
            {
                spaceShip.AddObject(this, this);
                stats.Notify(ActionTypeManager.AuraApply, new AuraContext(this));
            }
        }
        #endregion
    }
}
