using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Logic.Attacks
{
    public abstract class GenericProjectileAttack : GenericAttack
    {
        #region Properties

        #endregion

        #region Methods

        protected override void AttackFixedUpdate()
        {
            Move();
            Stat speed = Entity.GetStat("core.modtypes.skills.projectilespeed");
            //Stat despawnOnDestination = Entity.GetStat("core.modtypes.skills.projectiledespawndestination");
            //if (despawnOnDestination.Calculated >= 1.0f && this.transform.position == OriginalDestination)
            //{
            //    DestroyAttack();
            //    return;
            //}
            if (Rigidbody.velocity.magnitude > speed.Calculated + StartVelocity)
            {
                Rigidbody.velocity = Rigidbody.velocity.normalized * (speed.Calculated + StartVelocity);
            }
            speed.Current = Rigidbody.velocity.magnitude;
            Origin = this.Head.position;
            Destination = Direction * speed.Calculated + Origin;
        }
        protected virtual void Move()
        {
            Stat speed = Entity.GetStat("core.modtypes.skills.projectilespeed");
            Rigidbody.AddForce(Direction.normalized * (speed.Calculated + StartVelocity) - Rigidbody.velocity, ForceMode.VelocityChange);
        }

        protected override void OnAttackHit(HitContext hitContext)
        {
            base.OnAttackHit(hitContext);
            Stat pierce = Entity.GetStat("core.modtypes.skills.pierce");
            pierce.Current--;
            if (pierce.Current < 0.0f)
            {
                DestroyAttack();
            }
        }

        public static Vector3[] GenerateCircularPositions(Vector3 direction, Vector3 origin, int projectileCount,float startingAngle,float angle)
        {
            Vector3[] positions = new Vector3[projectileCount];

            if (projectileCount <= 0)
            {
                return positions;
            }
            if (projectileCount == 1)
            {
                positions[0] = direction + origin;
                return positions;
            }
            float anglePerProjectile = angle / (float)(projectileCount);
            for(int i = 0; i < positions.Length; i++)
            {
                positions[i] = (Quaternion.Euler(new Vector3(0, anglePerProjectile * i + startingAngle, 0 )) * direction) + origin;

            }
            return positions;
        }

        public static Vector3[] GenerateRandomCircularPositions(Vector3 direction, Vector3 origin, int projectileCount, float startingAngle, float maxAngle)
        {
            Vector3[] positions = new Vector3[projectileCount];

            for (int i = 0; i < positions.Length; i++)
            {
                float randomAngle = UnityEngine.Random.Range(0, maxAngle);
                positions[i] = (Quaternion.Euler(new Vector3(0, randomAngle + startingAngle,0)) * direction) + origin;
            }
            return positions;
        }

        public static Vector3[] GenerateVolleyPositions(Vector3 destination, Vector3 origin, int projectileCount, float targetLength, bool useOrigin)
        {

            Vector3[] positions = new Vector3[projectileCount];

            Vector3 line = Vector3.Cross((destination-origin).normalized, new Vector3(0, targetLength / (float)projectileCount,0));
            float start = -(projectileCount - 1) / 2.0f;
            for (int i = 0; i < positions.Length; i++)
            {
                if (useOrigin)
                {
                    positions[i] = origin + line * (start + i);
                }
                else
                {
                    positions[i] = destination + line * (start + i);
                }
            }

            return positions;
        }
        public override float GetMaxSkillDistance(Entity stats)
        {
            Stat projectileSpeed = stats.GetStat("core.modtypes.skills.projectilespeed");
            Stat duration = stats.GetStat("core.modtypes.skills.duration");

            float travelDistance = duration.Calculated * projectileSpeed.Calculated;

            return base.GetMaxSkillDistance(stats) + travelDistance;
        }
        #endregion
    }
}
