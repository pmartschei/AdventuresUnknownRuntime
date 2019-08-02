using AdventuresUnknownSDK.Core.Objects.Items.Actions;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class CraftingActionManager : SingletonBehaviour<CraftingActionManager>
    {


        #region Properties

        #endregion

        #region Methods
        public static CraftingAction[] GetDefaultCraftingActions(string itemTypeIdentifier)
        {
            return Instance.GetDefaultCraftingActionsImpl(itemTypeIdentifier);
        }
        protected abstract CraftingAction[] GetDefaultCraftingActionsImpl(string itemTypeIdentifier);
        #endregion
    }
}
