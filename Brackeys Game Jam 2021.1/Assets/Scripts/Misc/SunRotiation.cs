using UnityEngine;

namespace Misc
{
    public class SunRotiation : MonoBehaviour
    {
        public float rotationSpeed;

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}