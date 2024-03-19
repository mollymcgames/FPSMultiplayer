using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MachineFixer : MonoBehaviourPun
{
    public float interactionDistance = 2f;
    public KeyCode interactKey = KeyCode.E;
    public float fixingTime = 3f;
    public Slider fixingSlider; // Reference to the UI slider

    private bool isFixing = false;
    private float fixTimer = 0f;
    private bool isBroken = true;

    private void Update()
    {
        // if (!photonView.IsMine) // Only execute this code for the local player
        //     return;

        // Check if the player is near the machine and the machine is broken
        if (Input.GetKeyDown(interactKey) && IsPlayerNearMachine() && isBroken)
        {
            // Start fixing if the key is pressed and the machine is broken
            isFixing = true;
            fixingSlider.gameObject.SetActive(true); // Show the fixing slider
        }

        // Check if the key is released and the fixing process is ongoing
        if (Input.GetKeyUp(interactKey) && isFixing)
        {
            // Stop fixing if the key is released
            isFixing = false;
            fixingSlider.gameObject.SetActive(false); // Hide the fixing slider
            fixTimer = 0f; // Reset the fix timer
            return; // Exit the update loop
        }

        if (isFixing)
        {
            fixTimer += Time.deltaTime;

            // Update the value of the fixing slider based on the progress
            fixingSlider.value = fixTimer / fixingTime;

            // Check if fixing time is completed
            if (fixTimer >= fixingTime)
            {
                FinishFixingMachine();
            }
        }
        else
        {
            // Hide the fixing slider when not fixing
            fixingSlider.gameObject.SetActive(false);
        }
    }

    private void FinishFixingMachine()
    {
        // Reset fix timer and flag
        fixTimer = 0f;
        isFixing = false;

        // Set machine as fixed
        isBroken = false;

        // Hide the fixing slider
        fixingSlider.gameObject.SetActive(false);

        // Log the machine is fixed
        Debug.Log("I AM FIXED NOW!");

        // Notify other players using Photon RPC
        photonView.RPC("SetMachineFixed", RpcTarget.All);
    }

    [PunRPC]
    private void SetMachineFixed()
    {
        // You can add visual effects or animations here to indicate the machine is fixed.
        Debug.Log("Machine is fixed for everyone!");
    }

    private bool IsPlayerNearMachine()
    {
        // Calculate distance between player and machine
        float distance = Vector3.Distance(transform.position, transform.position);

        // Check if the player is within interaction distance
        return distance <= interactionDistance;
    }
}
