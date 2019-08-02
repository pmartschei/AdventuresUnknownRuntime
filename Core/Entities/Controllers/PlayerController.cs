using AdventuresUnknownSDK.Core.Attributes;
using AdventuresUnknownSDK.Core.Entities;
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
    public class PlayerController : EntityController
    {
        [SerializeField] private Rigidbody m_RigidBody = null;
        [SerializeField] private GenericActiveGemContainer m_PlayerActiveGemContainer = null;

        #region Properties
        public GenericActiveGemContainer PlayerActiveGemContainer { get => m_PlayerActiveGemContainer; set => m_PlayerActiveGemContainer = value; }
        #endregion

        #region Methods
        public override void OnStart()
        {
            base.OnStart();
            SpaceShip = PlayerManager.SpaceShip;
            SpaceShip.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!CanReceiveUpdate()) return;
            if (Camera.main)
            {
                LookingDestination = Camera.main.ScreenToWorldPoint(new Vector3(
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                    transform.position.z - Camera.main.transform.position.z
                    ));
                LookAt(LookingDestination);
            }
            UpdateMovement();
            UpdateSkillActivations();
            if (SpaceShip.Entity.IsDead)
            {
                SpaceShip.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
        }

        private void LookAt(Vector3 position)
        {
            if (Time.timeScale == 0.0f) return;
            float z = Mathf.Atan2((position.y - transform.position.y),
                (position.x - transform.position.x)) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.RotateTowards(
                this.transform.rotation,
                Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, z - 90.0f),
                Time.deltaTime * 360.0f);
        }

        private void FixedUpdate()
        {
            m_RigidBody.drag = SpaceShip.Entity.GetStat("core.modtypes.ship.movementresistance").Calculated;
            Stat speed = SpaceShip.Entity.GetStat("core.modtypes.ship.movementspeed");
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
                m_RigidBody.AddForce(Vector3.up * acceleration.Calculated);
            }
            if (Input.GetKey(KeyCode.S))
            {
                m_RigidBody.AddForce(-Vector3.up * acceleration.Calculated);
            }
            if (Input.GetKey(KeyCode.D))
            {
                m_RigidBody.AddForce(Vector3.right * acceleration.Calculated);
                //Quaternion deltaRotation = Quaternion.Euler(new Vector3(0,0,-250) * Time.deltaTime);
                //rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
            }
            if (Input.GetKey(KeyCode.A))
            {
                m_RigidBody.AddForce(-Vector3.right * acceleration.Calculated);
                //Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, 0, 250) * Time.deltaTime);
                //rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
            }
        }

        private void UpdateSkillActivations()
        {
            if (!m_PlayerActiveGemContainer) return;
            if (Input.GetKey(KeyCode.E))
            {
                m_PlayerActiveGemContainer.Spawn(0, this.transform.position, LookingDestination);
            }
        }

        private bool CanReceiveUpdate()
        {
            if (!m_RigidBody) return false;
            return true;
        }
        #endregion
    }
}
