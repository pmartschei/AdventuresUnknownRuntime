using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI.Tooltip
{
    public abstract class Tooltip : MonoBehaviour
    {
        [SerializeField] private RectTransform m_RootTransform = null;
        [SerializeField] private RectTransform m_VisibleTransform = null;
        public virtual RectTransform RootTransform { get => m_RootTransform; set => m_RootTransform = value; }
        public RectTransform VisibleTransform { get => m_VisibleTransform; set => m_VisibleTransform = value; }

        public abstract void Display(object obj);
        public virtual void Anchor(TextAnchor anchor)
        {

        }
    }

    public abstract class Tooltip<T> : Tooltip
    {
        public abstract void Display(T t);
        public override void Display(object obj)
        {
            if (obj == null) return;
            if (obj is T)
            {
                Display((T)(obj));
            }
        }
    }
}
