using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    private Vector3 trueInputVector;
    private Vector3 currentEulerAngle;

    public float RotationSpeed;
    public float AccelerationSpeed;
    public float MovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(trueInputVector * Time.fixedDeltaTime * AccelerationSpeed * 100);
        float velocityY = rb.velocity.y;
        Vector3 clampedVelocity = Vector3.ClampMagnitude(rb.velocity, MovementSpeed);
        clampedVelocity.y = velocityY;
        rb.velocity = clampedVelocity;


    }

    private void Update()
    {
        //movement
        float sidewaysMovement = UnityEngine.Input.GetAxis("Horizontal");
        float forwardMovement = UnityEngine.Input.GetAxis("Vertical");
        trueInputVector = transform.forward * forwardMovement + transform.right * sidewaysMovement;

        ////rotation
        //if (sidewaysMovement != 0)
        //    currentEulerAngle += new Vector3(0f, sidewaysMovement * RotationSpeed, 0f);
        //else
        //    currentEulerAngle = Vector3.Lerp(currentEulerAngle, Vector3.zero, 0.7f);
        //currentEulerAngle = Vector3.ClampMagnitude(currentEulerAngle, RotationSpeed);
        //transform.Rotate(currentEulerAngle);
    }
}
