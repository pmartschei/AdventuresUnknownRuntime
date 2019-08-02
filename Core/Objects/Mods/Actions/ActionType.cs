using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/ActionType", fileName = "ActionType.asset")]
    public class ActionType : CoreObject
    {
        [SerializeField] private int m_Value = 0;

        public int Value { get => m_Value; set => m_Value = value; }
    }
}
