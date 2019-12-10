using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundSource : MonoBehaviour
    {
        [SerializeField] private SoundGroupIdentifier m_SoundGroup;

        private AudioSource m_AudioSource = null;
        private void OnEnable()
        {
            m_AudioSource = GetComponent<AudioSource>();

            if (!m_SoundGroup.ConsistencyCheck()) return;

            m_AudioSource.outputAudioMixerGroup = m_SoundGroup.Object.AudioMixerGroup;
        }

    }
}
