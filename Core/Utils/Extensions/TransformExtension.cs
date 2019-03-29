using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Utils.Extensions
{
    public static class TransformExtension
    {

        public static Transform Clear(this Transform transform)
        {
            foreach(Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            return transform;
        }
    }
}
