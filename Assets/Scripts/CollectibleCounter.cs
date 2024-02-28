using UnityEngine;
using Photon.Pun;
using TMPro;
using static Photon.Pun.UtilityScripts.PunTeams;
// using Photon.Pun.UtilityScripts;


public class CollectibleCounter: MonoBehaviourPunCallbacks, IPunObservable
{
    //Player should be able to pick up a collectible for the team
    //Player should be able to see how many collectibles they have and everyone else in the team should be able to see it also

    //The number of collectibles the player has
    private int teamMachinePartsCount = 0;
    //private int hasCollidedWithMachine = 0;
    private float lastEnteredTime = 0;

    void Awake()
    {
        lastEnteredTime = Time.fixedTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        teamMachinePartsCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["teamMachinePartsCount"];

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log("[" + actorNumber + "] OnTriggerEnter() Enter Collided with: " + other.name);
        Debug.Log("[" + actorNumber + "] OnTriggerEnter() Enter current machine count: " + teamMachinePartsCount);

        //Get the collectible PhotonView
        Debug.Log("[" + actorNumber + "] OnTriggerEnter() for player");

        bool thisIsMine = false;
        PhotonView collectiblePhotonView = other.GetComponent<PhotonView>();
        if (collectiblePhotonView != null)
            thisIsMine = collectiblePhotonView.IsMine;

        Debug.Log("[" + actorNumber + "] OnTriggerEnter() This is mine: " + thisIsMine);

        if (other.CompareTag("Collectible") && Time.fixedTime - lastEnteredTime > 1)
        {
            Debug.Log("[" + actorNumber + "] OnTriggerEnter() Doing technical things thanks to: " + other.name);

            IncrementTeamMachinePartsCount();

            string realm = (string)PhotonNetwork.LocalPlayer.CustomProperties["Realm"];

            // Only the Master can remove the machine parts from the game.  
            PhotonView photonView2 = other.gameObject.GetComponent<PhotonView>();
            photonView.RPC("DestroyMachinePart", RpcTarget.MasterClient, photonView2.ViewID);

        }
        else
        {
            // This ELSE block can be removed when it's clear double triggering no-longer happens.
            Debug.Log("Triggered too quickly: " + (Time.fixedTime - lastEnteredTime).ToString());
        }
        lastEnteredTime = Time.fixedTime;
    }

    [PunRPC]
    private void DestroyMachinePart(int viewId)
    {
        Debug.Log(" DestroyMachinePart() destroying machine part with id:" + viewId);

        PhotonView pv = PhotonView.Find(viewId);

        if (pv != null)
        {
            Debug.Log(" DestroyMachinePart() destroying machine part:" + pv.name);

            //Destroy the collectible object
            PhotonNetwork.Destroy(PhotonView.Find(viewId).gameObject);
        }
    }

    //Method to increment the teams machine parts count
    private void IncrementTeamMachinePartsCount()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log("[" + actorNumber + "] IncrementTeamMachinePartsCount() for player");
        Debug.Log("[" + actorNumber + "] IncrementTeamMachinePartsCount() IsMine value:" + photonView.IsMine);
        Debug.Log("[" + actorNumber + "] Incrementing machine count:" + teamMachinePartsCount);

        //Increment the teams machine parts count
        teamMachinePartsCount++;
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "teamMachinePartsCount", teamMachinePartsCount } });
        Debug.Log("[" + actorNumber + "] Incremented machine count:" + teamMachinePartsCount);

        UpdateCollectibleCountText();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("something room  based just got updated..."+propertiesThatChanged);
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("teamMachinePartsCount", out object _teamMachinePartsCount))
        {
            teamMachinePartsCount = (int)_teamMachinePartsCount;
            Debug.Log("teamMachinePartsCount just got updated: " + teamMachinePartsCount);

            UpdateCollectibleCountText();
        }
    }

    //Method to update the collectible count text
    [PunRPC]
    private void UpdateCollectibleCountText()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        string realm = (string)PhotonNetwork.LocalPlayer.CustomProperties["Realm"];
        Debug.Log("[" + actorNumber + "] Updating text for realm: " + realm);

        UpdateCollectibleCountText(realm);
    }

    [PunRPC]
    private void UpdateCollectibleCountText(string realm)
    {
        teamMachinePartsCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["teamMachinePartsCount"];

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        Debug.Log("[" + actorNumber + "] RPC UpdateCollectibleCountText invoked! realm:" + realm);
        Debug.Log("[" + actorNumber + "] RPC UpdateCollectibleCountText invoked! count:" + teamMachinePartsCount);

        if ("Light" == realm)
        {
            //Reference to the TextMeshPro object that will display the collectible count
            TextMeshProUGUI collectibleCountText = GameObject.Find("MachineParts").GetComponent<TextMeshProUGUI>();

            Debug.Log("[" + actorNumber + "] Updating machine count text:" + teamMachinePartsCount);

            //Update the collectible count text
            collectibleCountText.text = "Machine Parts: " + teamMachinePartsCount;
        }
    }

    // Seems this has to be implemented!!
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {}
}