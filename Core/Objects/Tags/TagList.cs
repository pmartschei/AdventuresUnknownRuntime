using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Tags
{
    [Serializable]
    public class TagList: ICollection<Tag>
    {
        [SerializeField] private List<TagIdentifier> m_TagIdentifiers = new List<TagIdentifier>();

        private List<Tag> m_ConsistentTags = new List<Tag>();

        #region Properties
        public int Count => m_ConsistentTags.Count;

        public bool IsReadOnly => false;
        #endregion

        #region Methods
        public bool HasTag(string identifier)
        {
            foreach(Tag tag in m_ConsistentTags)
            {
                if (tag.Identifier.Equals(identifier)) return true;
            }
            return false;
        }
        public bool ConsistencyCheck()
        {
            m_ConsistentTags.Clear();
            for (int i = 0; i < m_TagIdentifiers.Count; i++)
            {
                if (m_TagIdentifiers[i].ConsistencyCheck())
                {
                    m_ConsistentTags.Add(m_TagIdentifiers[i].Object);
                }
            }
            return m_ConsistentTags.Count != 0;
        }

        public void Add(Tag item)
        {
            if (item == null) return;
            m_ConsistentTags.Add(item);
        }

        public void Clear()
        {
            m_ConsistentTags.Clear();
        }

        public bool Contains(Tag item)
        {
            if (item == null) return false;
            return m_ConsistentTags.Contains(item);
        }

        public void CopyTo(Tag[] array, int arrayIndex)
        {
            m_ConsistentTags.CopyTo(array, arrayIndex);
        }

        public bool Remove(Tag item)
        {
            return m_ConsistentTags.Remove(item);
        }

        public IEnumerator<Tag> GetEnumerator()
        {
            return m_ConsistentTags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_ConsistentTags.GetEnumerator();
        }

        public Tag this[int index]
        {
            get
            {
                return m_ConsistentTags[index];
            }
            set
            {
                m_ConsistentTags[index] = value;
            }
        }
        #endregion
    }
}
