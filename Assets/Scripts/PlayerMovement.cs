using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float walkSpeed = 4f;
    public float maxVelocityChange = 10f;

    private Vector2 input;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();
    }

    void FixedUpdate()
    {
        rb.AddForce(CalculateMovement(walkSpeed), ForceMode.VelocityChange);
    }

    Vector3 CalculateMovement(float speed)
    {
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity); //rotate the target velocity to the forward direction of the player
        targetVelocity *= speed;

        Vector3 velocity = rb.velocity;

        if (input.magnitude > 0.5f)
        {
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;

            return (velocityChange);
        }
        else
        {
            return Vector3.zero;
        }
    }
}
