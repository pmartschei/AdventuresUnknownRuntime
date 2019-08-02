using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Utils;
using AdventuresUnknownSDK.Core.Utils.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects
{
    public abstract class CoreObject : ScriptableObject, IAdventuresUnknownSerializeCallback
    {
        [SerializeField] private string m_Identifier = "";
        [SerializeField] private bool m_Overrides = false;
        

        [NonSerialized]
        private bool m_IdentifierDataChanged = false;
        [NonSerialized]
        private bool m_ShouldRename = false;

        #region Properties
        public string Identifier {
            get
            {
                UpdateIfChanged();
                return m_Identifier;
            }
            set
            {
                if (!m_Identifier.Equals(value))
                {
                    m_Identifier = value;
                    ForceUpdate();
                }
            }
        }
        public bool Overrides
        {
            get { return m_Overrides; }
        }
        internal bool ShouldRename { get => m_ShouldRename; set => m_ShouldRename = value; }
        #endregion

        #region Methods

        public virtual void InitializeObject()
        {
        }
        public virtual bool OnBeforeSerialize()
        {
            return true;
        }
        public virtual bool OnAfterDeserialize()
        {
            return true;
        }
        public virtual bool ConsistencyCheck()
        {
            ForceUpdate();
            return true;
        }
        public virtual void Initialize()
        {
            GameSettingsManager.OnLanguageChange.AddListener(OnLanguageChange);
        }

        public virtual void OnLanguageChange()
        {
            ForceUpdate();
        }
        public virtual void ForceUpdate()
        {
            m_IdentifierDataChanged = true;
            m_ShouldRename = true;
        }
        public void ForceUpdateImmediately()
        {
            ForceUpdate();
            UpdateIfChanged();
        }
        public virtual bool IsIdentifierEditableInEditor()
        {
            return true;
        }
        protected virtual string IdentifierCalculation()
        {
            return m_Identifier;
        }
        private void UpdateIfChanged()
        {
            if (m_IdentifierDataChanged)
            {
                m_Identifier = IdentifierCalculation();
                m_IdentifierDataChanged = false;
            }
        }
        public T FindScriptableObject<T>() where T : CoreObject
        {
            return ObjectsManager.FindObjectByIdentifier<T>(Identifier);
        }
        #endregion
    }
}
