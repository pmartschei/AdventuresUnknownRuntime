using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/AttributesObject", fileName = "AttributesObject.asset")]
    public class AttributesObject : ScriptableObject
    {
        [SerializeField] private Attribute[] m_Attributes = null;
        public Attribute[] Attributes { get => m_Attributes; set => m_Attributes = value; }
    }
}
