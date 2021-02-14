using Mirror;
using UnityEngine;

namespace Networking
{
    public class TestController : NetworkBehaviour
    {
        // Start is called before the first frame update
        public float movementSpeed;

        // Update is called once per frame
        void Update()
        {
            if (isLocalPlayer)
                MovePlayer(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * movementSpeed);
        }

        private void MovePlayer(Vector2 input)
        {
            transform.position += new Vector3(input.x, 0, input.y) * Time.deltaTime;
        }
    }
}