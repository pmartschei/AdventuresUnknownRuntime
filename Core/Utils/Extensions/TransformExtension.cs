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
            var tempList = transform.Cast<Transform>().ToList();
            foreach (Transform child in tempList)
            {
                if (!Application.isPlaying)
                {
                    GameObject.DestroyImmediate(child.gameObject);
                }
                else
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            return transform;
        }
        public static Transform MoveToLayer(this Transform root,int layer)
        {
            Stack<Transform> moveTargets = new Stack<Transform>();
            moveTargets.Push(root);
            Transform currentTarget;
            while (moveTargets.Count != 0)
            {
                currentTarget = moveTargets.Pop();
                currentTarget.gameObject.layer = layer;
                foreach (Transform child in currentTarget)
                    moveTargets.Push(child);
            }
            return root;
        }
    }
}
