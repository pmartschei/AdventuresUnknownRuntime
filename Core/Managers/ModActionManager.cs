using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class ModActionManager : SingletonBehaviour<ModActionManager>
    {


        #region Properties

        #endregion

        #region Methods
        public static List<BaseAction> GetActions(ActionType actionType, params ModType[] modTypes)
        {
            return Instance.GetActionsImpl(actionType,modTypes);
        }
        public static List<BaseAction> GetActions(int actionValue,params ModType[] modTypes)
        {
            return Instance.GetActionsImpl(actionValue, modTypes);
        }
        public static List<int> GetAllRegisteredActionValues()
        {
            return Instance.GetAllRegisteredActionValuesImpl();
        }
        protected abstract List<BaseAction> GetActionsImpl(ActionType actionType, params ModType[] modTypes);
        protected abstract List<BaseAction> GetActionsImpl(int actionValue, params ModType[] modTypes);
        protected abstract List<int> GetAllRegisteredActionValuesImpl();
        #endregion
    }
}
