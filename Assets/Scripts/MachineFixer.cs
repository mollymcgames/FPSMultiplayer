using UnityEngine;
using Photon.Pun;

public class MachineFixer : MonoBehaviourPun
{
    public float interactionDistance = 2f;
    public KeyCode interactKey = KeyCode.E;
    public float fixingTime = 3f;

    private bool isFixing = false;
    private float fixTimer = 0f;
    private bool isBroken = true;

    private void Update()
    {
        // add in a null check to check the playuer is in the scene 
        if (PhotonNetwork.LocalPlayer != null)
        {
            if (!photonView.IsMine)
                return;

            if (Input.GetKeyDown(interactKey) && IsPlayerNearMachine() && isBroken)
            {
                // Start fixing if the machine is broken and the player is nearby
                isFixing = true;
            }

            if (isFixing)
            {
                fixTimer += Time.deltaTime;

                // Check if fixing time is completed
                if (fixTimer >= fixingTime)
                {
                    FinishFixingMachine();
                }
            }
        }
    }

    private void FinishFixingMachine()
    {
        // Reset fix timer and flag
        fixTimer = 0f;
        isFixing = false;

        // Set machine as fixed
        isBroken = false;

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
