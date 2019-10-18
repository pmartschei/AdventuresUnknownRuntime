using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.Weapons
{
    public class Muzzle : MonoBehaviour
    {
        [SerializeField] private Transform m_MuzzleTransform = null;
        [SerializeField] private string m_MuzzleName = "";
        [SerializeField] private bool m_CanRotate = false;
        [SerializeField] private float m_DefaultRotation = 0.0f;
        [SerializeField] private float m_MaxRotation = 180.0f;
        #region Properties
        public Transform MuzzleTransform { get => m_MuzzleTransform; }
        public string MuzzleName { get => m_MuzzleName; set => m_MuzzleName = value; }
        public bool CanRotate { get => m_CanRotate; set => m_CanRotate = value; }
        public float DefaultRotation { get => m_DefaultRotation; set => m_DefaultRotation = value; }
        public float MaxRotation { get => m_MaxRotation; set => m_MaxRotation = value; }
        #endregion

        #region Methods

        #endregion
    }
}
