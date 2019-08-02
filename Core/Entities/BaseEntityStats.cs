using AdventuresUnknownSDK.Core.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Attribute = AdventuresUnknownSDK.Core.Objects.Mods.Attribute;

namespace AdventuresUnknownSDK.Core.Entities
{
    public class BaseEntityStats : MonoBehaviour, IActiveStat
    {
        [SerializeField] private EntityBehaviour m_Entity = null;
        [SerializeField] private Attribute[] m_Attributes = null;

        private List<Attribute> m_ConsistentAttributes = new List<Attribute>();
        #region Properties

        public bool StatsChanged { get; set; }

        #endregion

        #region Methods
        private void Start()
        {
            m_ConsistentAttributes.Clear();
            foreach (Attribute attribute in m_Attributes)
            {
                if (!attribute.ConsistencyCheck())
                {
                    GameConsole.LogWarningFormat("Skipped inconsistent Attribute in BaseEntityStats: {0} in {1}", attribute.ModBaseIdentifier,this.gameObject);
                    continue;
                }
                m_ConsistentAttributes.Add(attribute);
            }
            if (m_Entity)
            {
                m_Entity.Entity.AddActiveStat(this);
            }
            StatsChanged = true;
        }
        public void Initialize(Entity activeStat)
        {
            int level = 0;
            foreach(Attribute attribute in m_ConsistentAttributes)
            {
                activeStat.GetStat(attribute.ModBase.ModTypeIdentifier).AddStatModifier(new StatModifier(attribute.Value(level), attribute.ModBase.CalculationType, this));
            }
        }
        private void OnValidate()
        {
            m_ConsistentAttributes.Clear();
            foreach (Attribute attribute in m_Attributes)
            {
                if (!attribute.ConsistencyCheck())
                {
                    GameConsole.LogWarningFormat("Skipped inconsistent Attribute in BaseEntityStats: {0} in {1}", attribute.ModBaseIdentifier, this.gameObject);
                    continue;
                }
                m_ConsistentAttributes.Add(attribute);
            }
            StatsChanged = true;
        }
        #endregion
    }
}
