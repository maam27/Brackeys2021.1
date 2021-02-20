using System;
using Ship.Weapons;
using Ship.Weapons.Weapon_Fire;
using UnityEngine;
using Utility.Attributes;

namespace Level.Enemies
{
    public class Fighter : BaseEnemy
    {
        public float attackRange;
        [Space] [Expose] public Weapon currentWeapon;
        public Transform weaponBarrel;

        bool m_IsWithinRange = true;

        protected void Update()
        {
            Transform transform1;
            (transform1 = transform).rotation = Quaternion.LookRotation(DirectionToPlayer, Vector3.up);
            m_IsWithinRange = DistanceBetweenPlayerAndThis <= attackRange;

            if (m_IsWithinRange)
            {
                if (currentWeapon)
                    currentWeapon.Fire(true, weaponBarrel, transform1);
            }

            transform1.position = new Vector3(transform1.position.x, 0, transform1.position.z);
        }

        private void FixedUpdate()
        {
            OnFalseConditionMoveTowardsPlayer(m_IsWithinRange);
        }


        #region Reddit

        // public Vector2 Velocity = new Vector2(1, 0);
        //
        // [Range(0, 5)] 
        // public float RotateSpeed = 1f;
        // [Range(0, 5)]
        // public float RotateRadiusX = 1f;
        // [Range(0, 5)]
        // public float RotateRadiusY = 1f;
        //
        // public bool Clockwise = true;
        //
        // private Vector2 _centre;
        // private float _angle;
        //
        // private void Start()
        // {
        //     _centre = transform.position;
        // }
        //
        // private void Update()
        // {
        //     _centre += Velocity * Time.deltaTime;
        //
        //     _angle += (Clockwise ? RotateSpeed : -RotateSpeed) * Time.deltaTime;
        //
        //      The below apparently makes things rotate around the point.
        //     var x = Mathf.Sin(_angle) * RotateRadiusX;
        //     var y = Mathf.Cos(_angle) * RotateRadiusY;
        //
        //     transform.position = _centre + new Vector2(x, y);
        // }
        //
        // void OnDrawGizmos()
        // {
        //     Gizmos.DrawSphere(_centre, 0.1f);
        //     Gizmos.DrawLine(_centre, transform.position);
        // }

        #endregion
    }
}