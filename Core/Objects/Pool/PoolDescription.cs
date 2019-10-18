using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Pool
{
    public class PoolDescription : MonoBehaviour
    {
        [SerializeField] private int m_PoolSize = 64;
        [NonSerialized] private GameObject m_OriginalPrefab = null;
        public GameObject Prefab { get => m_OriginalPrefab == null ? gameObject : m_OriginalPrefab; set => m_OriginalPrefab = value; }
        public int PoolSize { get => m_PoolSize; set => m_PoolSize = value; }

        public void DisableGameObject(GameObject gameObject)
        {
            if (PoolManager.EnqueueInstance(gameObject, this)) return;

            Destroy(gameObject);
        }
    }
}
