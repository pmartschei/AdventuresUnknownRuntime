using AdventuresUnknownSDK.Core.Attributes;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers.Interfaces;
using AdventuresUnknownSDK.Core.Entities.Weapons;
using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Logic.ActiveGemContainers;
using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.Controllers
{
    public class PlayerController : EntityController,IMuzzleComponentController
    {
        [SerializeField] private Rigidbody m_RigidBody = null;
        [SerializeField] private Muzzle[] m_Muzzles = null;

        private Dictionary<string, Muzzle> m_MuzzleDictionary = new Dictionary<string, Muzzle>();


        #region Properties
        public Muzzle[] Muzzles { get => m_Muzzles; set => m_Muzzles = value; }
        #endregion

        #region Methods
        public override void OnStart()
        {
            base.OnStart();

            foreach (Muzzle muzzle in m_Muzzles)
            {
                if (muzzle == null) continue;
                if (m_MuzzleDictionary.ContainsKey(muzzle.MuzzleName))
                {
                    GameConsole.LogErrorFormat("Skipped duplicate muzzle -> {0}, {1}", muzzle.MuzzleName, this.name);
                    continue;
                }
                m_MuzzleDictionary.Add(muzzle.MuzzleName, muzzle);
            }

            SpaceShip = PlayerManager.SpaceShip;
            SpaceShip.gameObject.SetActive(true);
            Entity = SpaceShip.Entity;
        }

        private void Update()
        {
            if (!CanReceiveUpdate()) return;
            if (Camera.main)
            {
                LookingDestination = Camera.main.ScreenToWorldPoint(new Vector3(
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                    Camera.main.transform.position.y - (Head.position.y)
                    ));
                LookAt(LookingDestination);
            }
            if (SpaceShip.Entity.IsDead)
            {
                SpaceShip.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
        }

        private void LookAt(Vector3 position)
        {
            if (m_RigidBody.velocity != Vector3.zero)
                Head.rotation = Quaternion.LookRotation(m_RigidBody.velocity.normalized);
            foreach(Muzzle muzzle in Muzzles)
            {
                if (!muzzle || muzzle.CanRotate) continue;
                muzzle.transform.rotation = Quaternion.LookRotation(position - muzzle.transform.position);
            }
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(position),Time.deltaTime*360.0f);
            //float y = Mathf.Atan2((position.z - transform.position.z),
            //    (position.x - transform.position.x)) * Mathf.Rad2Deg;
            //this.transform.rotation = Quaternion.RotateTowards(
            //    this.transform.rotation,
            //    Quaternion.Euler(this.transform.rotation.eulerAngles.x, y + 90.0f,  this.transform.rotation.eulerAngles.z),
            //    Time.deltaTime * 360.0f);
        }

        private void FixedUpdate()
        {
            //m_RigidBody.drag = SpaceShip.Entity.GetStat("core.modtypes.ship.movementresistance").Calculated;
            Stat speed = SpaceShip.Entity.GetStat("core.modtypes.ship.movementspeed");
            UpdateMovement();
            if (m_RigidBody.velocity.magnitude > speed.Calculated)
            {
                m_RigidBody.velocity = m_RigidBody.velocity.normalized * speed.Calculated;
            }
            speed.Current = m_RigidBody.velocity.magnitude;
        }
        private void UpdateMovement()
        {
            Stat acceleration = SpaceShip.Entity.GetStat("core.modtypes.ship.acceleration");
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 force = (Vector3.forward * acceleration.Calculated);
                m_RigidBody.AddForce(force);
            }
            if (Input.GetKey(KeyCode.S))
            {
                Vector3 force = (-Vector3.forward * acceleration.Calculated);
                m_RigidBody.AddForce(force, ForceMode.Acceleration);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Vector3 force = (Vector3.right * acceleration.Calculated);
                m_RigidBody.AddForce(force, ForceMode.Acceleration);
            }
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 force = (-Vector3.right * acceleration.Calculated);
                m_RigidBody.AddForce(force, ForceMode.Acceleration);
            }
        }

        private bool CanReceiveUpdate()
        {
            if (!m_RigidBody) return false;
            return true;
        }

        public Muzzle FindMuzzle(string name)
        {
            Muzzle muzzle;
            m_MuzzleDictionary.TryGetValue(name, out muzzle);
            return muzzle;
        }

        public Muzzle[] FindMuzzles(params string[] names)
        {
            List<Muzzle> muzzles = new List<Muzzle>();
            if (names != null)
            {
                foreach (string name in names)
                {
                    Muzzle muzzle = FindMuzzle(name);
                    if (!muzzle) continue;
                    muzzles.Add(muzzle);
                }
            }

            return muzzles.ToArray();
        }
        #endregion
    }
}
