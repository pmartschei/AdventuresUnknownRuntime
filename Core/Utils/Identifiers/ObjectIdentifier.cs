using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public abstract class ObjectIdentifier
    {
        [SerializeField] private string m_Identifier = "empty_identifier";
        [NonSerialized] protected CoreObject m_Object = null;
        [NonSerialized] protected bool m_IdentifierDataChanged = false;

        #region Properties
        public string Identifier
        {
            get => m_Identifier;
            set
            {
                if (!m_Identifier.Equals(value))
                {
                    m_IdentifierDataChanged = true;
                    m_Identifier = value;
                }
            }
        }
        public CoreObject Object
        {
            get
            {
                UpdateIfChanged();
                return m_Object;
            }
            internal set
            {
                m_Object = value;
            }
        }
#endregion

#region Methods
        public virtual bool ConsistencyCheck()
        {
            ForceUpdate();
            return Object != null;
        }
        private void UpdateIfChanged()
        {
            if (m_IdentifierDataChanged)
            {
                m_IdentifierDataChanged = false;
                Type[] types = GetSupportedTypes();
                if (types != null)
                {
                    foreach(Type type in types)
                    {
                        m_Object = ObjectsManager.FindObjectByIdentifier(Identifier, type);
                        if (m_Object != null) break;
                    }
                }
            }
        }
        public virtual void ForceUpdate()
        {
            m_IdentifierDataChanged = true;
        }
        public abstract Type[] GetSupportedTypes();
#endregion
    }
}
