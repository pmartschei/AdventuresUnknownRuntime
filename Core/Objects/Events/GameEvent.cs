// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// Changed to fit project
// ----------------------------------------------------------------------------

using AdventuresUnknownSDK.Core.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Events
{
    [CreateAssetMenu(menuName ="AdventuresUnknown/Core/Events/GameEvent",fileName ="GameEvent.asset")]
    public class GameEvent : CoreObject
    {
        private readonly List<GameEventListener> eventListeners = 
            new List<GameEventListener>();

        public void Raise()
        {
            for(int i = eventListeners.Count -1; i >= 0; i--)
                eventListeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}