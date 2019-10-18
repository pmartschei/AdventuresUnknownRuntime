using AdventuresUnknownSDK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Effects
{
    public class EffectContext 
    {
        private Entity m_Entity = null;
        private Effect m_Effect = null;
        private float m_Duration = 0.0f;
        private float m_Value = 0.0f;
        private bool m_IsInfinityDuration = false;

        private Dictionary<object, object> m_ObjectDictionary = new Dictionary<object, object>();

        #region Properties
        public Entity Entity { get => m_Entity; set => m_Entity = value; }
        public Effect Effect { get => m_Effect; set => m_Effect = value; }
        public float Duration { get => m_Duration; set => m_Duration = value; }
        public float Value
        {
            get => m_Value;
            set
            {
                if (m_Value != value)
                {
                    m_Value = value;
                    Remove();
                    Effect.BasicModBase.AddStatModifierTo(Entity, m_Value, this);
                }
            }
        }
        public bool IsInfinityDuration { get => m_IsInfinityDuration; set => m_IsInfinityDuration = value; }

        #endregion

        #region Methods
        public EffectContext(Entity entity,Effect effect)
        {
            m_Entity = entity;
            m_Effect = effect;
        }
        public void Update(float time)
        {
            if (m_Effect)
                m_Effect.Update(this, time);
        }

        public bool IsGone()
        {
            if (m_Effect == null) return true;
            return m_Effect.IsGone(this);
        }

        public void AddEffect(float duration,float value)
        {
            if (duration == -1.0f)
            {
                m_IsInfinityDuration = true;
            }
            if (m_Effect)
                m_Effect.AddEffect(this, duration, value);
        }

        public void Remove()
        {
            Entity.RemoveAllModifiersBySource(this);
        }


        public bool AddObject(object source, object obj)
        {
            return AddObject(source, obj, false);
        }
        public bool AddObject(object source, object obj, bool replace)
        {
            object value;
            if (m_ObjectDictionary.TryGetValue(source, out value))
            {
                if (!replace) return false;
                m_ObjectDictionary.Remove(source);
            }
            m_ObjectDictionary.Add(source, obj);
            return true;
        }
        public object GetObject(object source)
        {
            if (!m_ObjectDictionary.ContainsKey(source)) return null;
            return m_ObjectDictionary[source];
        }

        public T GetObject<T>(object source)
        {
            object obj = GetObject(source);

            if (obj != null && obj is T)
            {
                return (T)(obj);
            }
            return default(T);
        }
        #endregion
    }
}
