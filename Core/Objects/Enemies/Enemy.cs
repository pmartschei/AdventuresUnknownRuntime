using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Localization;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Enemies
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Enemies/Enemy", fileName = "Enemy.asset")]
    public class Enemy : CoreObject
    {

        [SerializeField] private LocalizationString m_EnemyName = null;
        [SerializeField] private EnemyTypeIdentifier[] m_EnemyType = null;
        [SerializeField] private Attribute[] m_Attributes = null;
        [SerializeField] private EnemyModel m_Model = null;
        
        //behaviour for enemy
        //attack list for enemy

        [HideInInspector] private List<EnemyType> m_ConsistentEnemyTypes = new List<EnemyType>();

        #region Properties
        public string EnemyName { get => m_EnemyName.LocalizedString; }
        public EnemyModel Model { get => m_Model; set => m_Model = value; }
        public Attribute[] Attributes { get => m_Attributes; set => m_Attributes = value; }
        public EnemyType[] EnemyTypes { get => m_ConsistentEnemyTypes.ToArray(); }

        #endregion

        #region Methods
        public override bool ConsistencyCheck()
        {
            base.ConsistencyCheck();
            m_ConsistentEnemyTypes.Clear();
            for(int i = 0; i < m_EnemyType.Length; i++)
            {
                if (m_EnemyType[i].ConsistencyCheck())
                {
                    m_ConsistentEnemyTypes.Add(m_EnemyType[i].Object);
                }
            }

            return m_ConsistentEnemyTypes.Count != 0;
        }
        public override void ForceUpdate()
        {
            base.ForceUpdate();
            m_EnemyName.ForceUpdate();
        }

        public void OnValidate()
        {
            foreach(Attribute attribute in m_Attributes)
            {
                attribute.UpdateInspectorElementName();
            }
        }
        #endregion
    }
}
