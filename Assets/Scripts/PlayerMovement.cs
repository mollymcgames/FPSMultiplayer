using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float walkSpeed = 8f;
    public float runSpeed = 14f;
    public float maxVelocityChange = 10f;

    public float airControl = 0.5f;

    public float jumpHeight = 5f;

    private Vector2 input;
    private Rigidbody rb;

    private bool running;
    private bool jumping;
    private bool grounded = false;

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

        running = Input.GetButton("Run");
        jumping = Input.GetButton("Jump");
    }

    //check were grounded
    private void OnTriggerStay(Collider other)
    {
        grounded = true;
    }

    void FixedUpdate()
    {
        //jumping
        if (grounded)
        {
            if (jumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
            }
            else if(input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(running ? runSpeed : walkSpeed), ForceMode.VelocityChange);
            }
            else
            {
                Vector3 velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f, velocity1.y, velocity1.z * 0.2f);
                rb.velocity = Vector3.Lerp(rb.velocity, velocity1, 0.2f);
            }
        }
        else
        {
            if(input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(running ? runSpeed * airControl : walkSpeed * airControl), ForceMode.VelocityChange);
            }
            else
            {
                Vector3 velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f, velocity1.y, velocity1.z * 0.2f);
                rb.velocity = Vector3.Lerp(rb.velocity, velocity1, 0.2f);
            }
        }

        grounded = false;
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
