using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class ObjectsManager : SingletonBehaviour<ObjectsManager>
    {

        #region Properties

        #endregion

        #region Methods
        
        public static T FindObjectByIdentifier<T>(string fullIdentifier)
        {
            return Instance.FindObjectByIdentifierImpl<T>(fullIdentifier);
        }
        public static CoreObject FindObjectByIdentifier(string fullIdentifier, Type type)
        {
            return Instance.FindObjectByIdentifierImpl(fullIdentifier, type);
        }
        
        public static T[] GetAllObjects<T>()
        {
            return Instance.GetAllObjectsImpl<T>();
        }
        protected abstract T FindObjectByIdentifierImpl<T>(string fullIdentifier);
        protected abstract CoreObject FindObjectByIdentifierImpl(string fullIdentifier, Type type);
        protected abstract T[] GetAllObjectsImpl<T>();
        #endregion
    }
}
