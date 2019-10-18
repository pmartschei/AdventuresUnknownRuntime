using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Experience
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/ExperienceController/ExponentExperienceController", fileName = "ExponentExperienceController.asset")]
    public class ExponentExperienceController : ExperienceController
    {
        [SerializeField] private float m_BaseExperience = 50.0f;
        [SerializeField] private float m_Exponent = 1.1f;
        public override void Initialize()
        {
            base.Initialize();
            m_RequiredExperience = new int[m_MaxLevel];
            for(int i=0;i < m_MaxLevel; i++)
            {
                if (i == 0 || i == 1)
                {
                    m_RequiredExperience[i] = 0;
                }
                else if (i == 2)
                {
                    m_RequiredExperience[i] = Mathf.FloorToInt(m_BaseExperience);
                }
                else
                {
                    m_RequiredExperience[i] = Mathf.FloorToInt(m_RequiredExperience[i - 1] * m_Exponent);
                }
            }
        }
    }
}
