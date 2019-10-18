using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AdventuresUnknownSDK.Core.Utils.Extensions
{
    public static class ExecuteEventsExtension
    {

        public static bool ExecuteHierarchyDown<T>(GameObject root,BaseEventData eventData,ExecuteEvents.EventFunction<T> eventFunction) where T : IEventSystemHandler
        {
            bool hadEvent = false;

            Queue<GameObject> gameObjectQueue = new Queue<GameObject>();
            gameObjectQueue.Enqueue(root);

            while(gameObjectQueue.Count > 0)
            {
                GameObject gameObject = gameObjectQueue.Dequeue();

                hadEvent |= ExecuteEvents.Execute(gameObject, eventData, eventFunction);

                foreach (Transform child in gameObject.transform)
                {
                    gameObjectQueue.Enqueue(child.gameObject);
                }
            }
            return hadEvent;
        } 
        
    }
}
