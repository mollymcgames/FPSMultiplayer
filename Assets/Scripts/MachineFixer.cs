using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

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
    public int partsRequired = 1; // Number of parts required to fix the machine

    public TMP_Text partsRequiredText; // Reference to the TMP text object

    public int teamMachinePartsCount = 0; //variable to store the team machine parts count 

    private void Start()
    {
        // Set the parts required text
        UpdatePartsRequiredText();
    }


    private void Update()
    {
        //Get the team machine parts count from the room properties
        teamMachinePartsCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["teamMachinePartsCount"];

        // Check if the player is in the light realm first before fixing the machine 
        bool isLightRealm = (string)PhotonNetwork.LocalPlayer.CustomProperties["Realm"] == "Light";

        // Check if the player is near the machine, the machine is broken, and the player is colliding with it and if the player is in the light realm
        if (Input.GetKeyDown(interactKey) && isBroken && isColliding && isLightRealm && teamMachinePartsCount > 0)
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

        // partsRequired--; // Decrement the parts required to fix the machine

        photonView.RPC("DecrementMachineParts", RpcTarget.All);
        // TO DO - this will need work when we have multiple machine parts to fix as more machines will be added
        // Assuming you have a reference to the other GameObject find by tag
        GameObject otherGameObject = GameObject.FindWithTag("Player");
        // GameObject otherGameObject = GameObject.Find("CollectibleMachinePart(Clone)");

        // Get the PhotonView component attached to the other GameObject
        PhotonView otherPhotonView = otherGameObject.GetComponent<PhotonView>();

        otherPhotonView.RPC("UpdateTotalMachinesFixedCount", RpcTarget.All);
        photonView.RPC("UpdatePartsRequiredText", RpcTarget.All);
        photonView.RPC("UpdateTeamCollectibleCountText", RpcTarget.All);


        // Update the parts required text
       // UpdatePartsRequiredText();
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

    [PunRPC]
    public void UpdatePartsRequiredText()
    {
        // Update the parts required text
        partsRequiredText.text = "Parts Required: " + partsRequired;
    }

    [PunRPC]
    public void DecrementMachineParts()
    {
        partsRequired--; // Decrement the parts required to fix the machine

        // Decrement the team machine parts count in the room properties for the custom room property 
        PhotonNetwork.CurrentRoom.CustomProperties["teamMachinePartsCount"] = teamMachinePartsCount - 1;


        // Update the parts required text
        // UpdatePartsRequiredText();        
    }

}
