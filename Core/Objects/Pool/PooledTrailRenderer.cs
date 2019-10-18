using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Pool
{
    [RequireComponent(typeof(TrailRenderer))]
    public class PooledTrailRenderer : MonoBehaviour,IPooledComponent
    {
        private TrailRenderer m_TrailRenderer = null;

        public void OnPostDisable()
        {
        }

        public void OnPostEnable()
        {
            m_TrailRenderer.Clear();
        }

        public void OnPreDisable()
        {
        }

        public void OnPreEnable()
        {
        }

        public void OnSpawn()
        {
        }

        private void Awake()
        {
            m_TrailRenderer = GetComponent<TrailRenderer>();
        }
    }
}
