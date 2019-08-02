using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/ProtectionCause", fileName = "ProtectionCause.asset")]
    public class ProtectionCause : CoreObject
    {
        [SerializeField] private int m_Value = 0;

        public int Value { get => m_Value; set => m_Value = value; }
    }
}