using AdventuresUnknownSDK.Core.Objects.Pool;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class PoolManager : SingletonBehaviour<PoolManager>
    {

        public static GameObject Instantiate(GameObject gameObject)
        {
            if (!Instance) return Object.Instantiate(gameObject);
            return Instance.InstantiateImpl(gameObject);
        }
        public static GameObject Instantiate(GameObject gameObject, Transform parent)
        {
            if (!Instance) return Object.Instantiate(gameObject, parent);
            return Instance.InstantiateImpl(gameObject, parent);
        }
        public static GameObject Instantiate(GameObject gameObject, Vector3 position, Quaternion rotation)
        {
            if (!Instance) return Object.Instantiate(gameObject, position,rotation);
            return Instance.InstantiateImpl(gameObject, position,rotation);
        }
        public static GameObject Instantiate(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (!Instance) return Object.Instantiate(gameObject, position, rotation,parent);
            return Instance.InstantiateImpl(gameObject, position, rotation,parent);
        }

        public static void CreatePool(PoolDescription poolDescription)
        {
            if (!Instance) return;
            Instance.CreatePoolImpl(poolDescription);
        }

        public static bool EnqueueInstance(GameObject instance,PoolDescription poolDescription)
        {
            if (!Instance) return false;
            return Instance.EnqueueInstanceImpl(instance, poolDescription);
        }

        protected abstract GameObject InstantiateImpl(GameObject gameObject);
        protected abstract GameObject InstantiateImpl(GameObject gameObject, Transform parent);
        protected abstract GameObject InstantiateImpl(GameObject gameObject, Vector3 position, Quaternion rotation);
        protected abstract GameObject InstantiateImpl(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent);
        protected abstract void CreatePoolImpl(PoolDescription poolDescription);
        protected abstract bool EnqueueInstanceImpl(GameObject instance, PoolDescription poolDescription);
    }
}
