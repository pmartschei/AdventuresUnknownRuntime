using AdventuresUnknownSDK.Core.Utils.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Entities
{
    public class TimerObject
    {
        private object m_Source = null;
        private float m_Duration = 0.0f;
        private UnityAction<Entity> m_Callback = null;

        #region Properties
        public object Source { get => m_Source; set => m_Source = value; }
        public float Duration { get => m_Duration; }
        #endregion

        #region Methods
        public TimerObject(object source, float duration, UnityAction<Entity> callback)
        {
            m_Source = source;
            m_Duration = duration;
            m_Callback = callback;
        }
        public TimerObject(object source, float duration) : this(source,duration,null)
        {
        }
        public bool IsFinished()
        {
            return m_Duration <= 0.0f;
        }
        public void Update(float time)
        {
            m_Duration -= time;
        }
        public void Callback(Entity activeStats)
        {
            if (m_Callback != null)
                m_Callback.Invoke(activeStats);
        }

        public class CompareSource : IEqualityComparer<TimerObject>
        {
            public bool Equals(TimerObject x, TimerObject y)
            {
                if (x == null && y == null) return true;
                if (x == null || y == null) return false;
                return x.Source.Equals(y.Source);
            }

            public int GetHashCode(TimerObject obj)
            {
                if (obj.Source == null) return 0;
                return obj.Source.GetHashCode();
            }
        }
        #endregion
    }
}
