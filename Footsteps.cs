using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
	[SerializeField] private AudioSource walkSound;
	[SerializeField] private float footstepInterval = 0.5f;

	private Rigidbody rb;
	private bool grounded = false;
	private float timeSinceLastFootstep = 0f;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Check if grounded
	private void OnTriggerStay(Collider other)
	{
		grounded = true;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (grounded && rb.velocity.magnitude > 0.1f)
		{
			// Check if enough time has passed since the last footstep sound
			if (Time.time - timeSinceLastFootstep > footstepInterval)
			{
				walkSound.Play();
				timeSinceLastFootstep = Time.time;
			}
		}

		grounded = false;
	}
}