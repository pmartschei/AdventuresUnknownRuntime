using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AdventuresUnknownSDK.Core.Objects.Tags.WeightedTagList;

namespace AdventuresUnknownSDK.Core.Objects.Tags
{
    [Serializable]
    public class WeightedTagList : ICollection<WeightedTag>
    {
        [Serializable]
        public struct WeightedTag
        {
            public TagIdentifier Tag;
            public int Weight;
        }

        [SerializeField] private List<WeightedTag> m_WeightedTags = new List<WeightedTag>();

        public int Count => m_WeightedTags.Count;

        public bool IsReadOnly => false;

        #region Properties

        #endregion

        #region Methods
        public int GetWeight(string tagIdentifier)
        {
            foreach(WeightedTag wt in m_WeightedTags)
            {
                if (wt.Tag.Identifier.Equals(tagIdentifier))
                {
                    return wt.Weight;
                }
            }
            return 0;
        }

        public int GetWeight(params string[] tagIdentifiers)
        {
            if (tagIdentifiers == null) return -1;
            foreach(WeightedTag wt in m_WeightedTags)
            {
                if (tagIdentifiers.Contains(wt.Tag.Identifier))
                {
                    return wt.Weight;
                }
            }
            return -1;
        }

        public void Add(WeightedTag item)
        {
            m_WeightedTags.Add(item);
        }

        public void Clear()
        {
            m_WeightedTags.Clear();
        }

        public bool Contains(WeightedTag item)
        {
            return m_WeightedTags.Contains(item);
        }

        public void CopyTo(WeightedTag[] array, int arrayIndex)
        {
            m_WeightedTags.CopyTo(array, arrayIndex);
        }

        public bool Remove(WeightedTag item)
        {
            return m_WeightedTags.Remove(item);
        }

        public IEnumerator<WeightedTag> GetEnumerator()
        {
            return m_WeightedTags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_WeightedTags.GetEnumerator();
        }
        #endregion
    }
}
