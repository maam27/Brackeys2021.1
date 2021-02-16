using System;
using UnityEngine;

namespace Ship
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShipController : MonoBehaviour
    {
        private Rigidbody m_Rb;
        private Camera m_MainCam;
        public float shipSpeed;

        // Start is called before the first frame update
        void Awake()
        {
            m_Rb = GetComponent<Rigidbody>();
            m_MainCam = Camera.main;
        }


        private Vector2 m_Input;
        private Vector3 m_MousePos;

        // Update is called once per frame
        void Update()
        {
            m_Input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            m_MousePos = GetMousePositionInWorldCoordinates(Input.mousePosition);
            RotateShipTowards(m_MousePos);
        }

        private Vector3 GetMousePositionInWorldCoordinates(Vector3 mousePosInPixelCoordinates)
        {
            Vector3 result = Vector3.zero;
            Plane plane = new Plane();
            plane.SetNormalAndPosition(Vector3.up, transform.position);
            Ray ray = m_MainCam.ScreenPointToRay(mousePosInPixelCoordinates);
            if (plane.Raycast(ray, out var dist))
            {
                result = ray.GetPoint(dist);
            }

            return result;
        }

        private void FixedUpdate()
        {
            MoveShip(m_Input, shipSpeed);
        }


        public void RotateShipTowards(Vector3 target)
        {
            transform.rotation = Quaternion.LookRotation((target - transform.position).normalized, Vector3.up);
        }

        public void MoveShip(Vector2 direction, float speed)
        {
            if (direction != Vector2.zero)
                m_Rb.velocity = Vector3.Lerp(m_Rb.velocity, new Vector3(
                    direction.x * (speed * 100f) * Time.fixedDeltaTime,
                    m_Rb.velocity.y,
                    direction.y * (speed * 100f) * Time.fixedDeltaTime), 0.75f);

            else
                m_Rb.velocity = Vector3.Lerp(m_Rb.velocity, Vector3.zero, 0.05f);
        }
    }
}