using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Experience
{
    public abstract class ExperienceController : CoreObject
    {
        [SerializeField] protected int m_MaxLevel = 100;
        [SerializeField] protected int[] m_RequiredExperience = null;
        public int MaxLevel { get => m_MaxLevel; }
        public int GetExperienceForLevel(int level)
        {
            if (level < m_MaxLevel)
            {
                return m_RequiredExperience[level];
            }
            return -1;
        }
    }
}
