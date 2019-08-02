using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Localization;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Tags;
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
        [SerializeField] private TagList m_TagList = null;
        [SerializeField] private Attribute[] m_Attributes = null;
        [Range(1, 10)]
        [SerializeField] private int m_Difficulty = 1;
        [SerializeField] private EnemyModel m_Model = null;
        [SerializeField] private AttackPattern m_AttackPattern = null;
        
        //attack list for enemy

        #region Properties
        public string EnemyName { get => m_EnemyName.LocalizedString; }
        public EnemyModel Model { get => m_Model; set => m_Model = value; }
        public Attribute[] Attributes { get => m_Attributes; set => m_Attributes = value; }
        public AttackPattern AttackPattern { get => m_AttackPattern; set => m_AttackPattern = value; }
        public TagList TagList { get => m_TagList; set => m_TagList = value; }
        public int Difficulty { get => m_Difficulty; set => m_Difficulty = value; }

        #endregion

        #region Methods
        public override bool ConsistencyCheck()
        {
            base.ConsistencyCheck();
            return m_TagList.ConsistencyCheck();
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
