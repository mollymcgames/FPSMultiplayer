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
    private bool isColliding = false;

    private void Update()
    {
        // Check if the player is in the light realm first before fixing the machine 
        bool isLightRealm = (string)PhotonNetwork.LocalPlayer.CustomProperties["Realm"] == "Light";

        // Check if the player is near the machine, the machine is broken, and the player is colliding with it and if the player is in the light realm
        if (Input.GetKeyDown(interactKey) && isBroken && isColliding && isLightRealm)
        {
            Debug.Log("Start fixing the machine.");
            // Start fixing if the key is pressed, the machine is broken, and the player is colliding with it
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
        // Set machine as fixed
        isBroken = false;        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a player
        if (other.CompareTag("Player"))
        {
            // Set the flag to true when the player collides with the machine
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collider belongs to a player
        if (other.CompareTag("Player"))
        {
            // Set the flag to false when the player exits the collision with the machine
            isColliding = false;
        }
    }
}
