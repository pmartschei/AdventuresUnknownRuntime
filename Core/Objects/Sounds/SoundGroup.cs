using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace AdventuresUnknownSDK.Core.Objects.Sounds
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Sounds/SoundGroup", fileName = "SoundGroup.asset")]
    public class SoundGroup : CoreObject
    {
        [SerializeField] private AudioMixerGroup m_AudioMixerGroup = null;
        public AudioMixerGroup AudioMixerGroup { get => m_AudioMixerGroup; set => m_AudioMixerGroup = value; }
    }
}
