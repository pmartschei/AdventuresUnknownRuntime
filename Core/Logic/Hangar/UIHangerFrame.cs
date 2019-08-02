using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Logic.Hangar
{
    public class UIHangerFrame : MonoBehaviour
    {
        public enum PositionType
        {
            Left,
            Right,
            Full,
        }

        [SerializeField] private PositionType m_FramePosition = PositionType.Full;

        public PositionType FramePosition { get => m_FramePosition; set => m_FramePosition = value; }
    }
}
