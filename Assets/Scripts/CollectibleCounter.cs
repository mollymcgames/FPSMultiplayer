using UnityEngine;
using Photon.Pun;
using TMPro;
using static Photon.Pun.UtilityScripts.PunTeams;
// using Photon.Pun.UtilityScripts;


public class CollectibleCounter: MonoBehaviourPunCallbacks, IPunObservable
{
    //Player should be able to pick up a collectible for the team
    //Player should be able to see how many collectibles they have and everyone else in the team should be able to see it also

    // The number of collectibles the team has
    private int teamMachinePartsCount = 0;

    // The number of collectibles the player has
    private int playerMachinePartsCount = 0;

    private float lastEnteredTime = 0;

    void Awake()
    {
        lastEnteredTime = Time.fixedTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        teamMachinePartsCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["teamMachinePartsCount"];
        playerMachinePartsCount = (int)PhotonNetwork.LocalPlayer.CustomProperties["MachinePartsCount"];

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log("[" + actorNumber + "] OnTriggerEnter() Enter Collided with: " + other.name);
        Debug.Log("[" + actorNumber + "] OnTriggerEnter() Enter current team machine count: " + teamMachinePartsCount);
        Debug.Log("[" + actorNumber + "] OnTriggerEnter() Enter current player machine count: " + playerMachinePartsCount);

        //Get the collectible PhotonView
        Debug.Log("[" + actorNumber + "] OnTriggerEnter() for player");

//        bool thisIsMine = false;
        PhotonView collectiblePhotonView = other.GetComponent<PhotonView>();
//        if (collectiblePhotonView != null)
//            thisIsMine = collectiblePhotonView.IsMine;
        
        if (other.CompareTag("Collectible") && Time.fixedTime - lastEnteredTime > 1)
        {
            IncrementTeamMachinePartsCount();
            IncrementPlayerMachinePartsCount();

//            string realm = (string)PhotonNetwork.LocalPlayer.CustomProperties["Realm"];

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
        Debug.Log("[" + actorNumber + "] IncrementTeamMachinePartsCount()Incrementing team machine count:" + teamMachinePartsCount);

        // Increment the team's machine parts count
        teamMachinePartsCount++;
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "teamMachinePartsCount", teamMachinePartsCount } });
        Debug.Log("[" + actorNumber + "] IncrementTeamMachinePartsCount() Incremented team machine count:" + teamMachinePartsCount);

        UpdateTeamCollectibleCountText();
    }

    //Method to increment the teams machine parts count
    private void IncrementPlayerMachinePartsCount()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log("[" + actorNumber + "] IncrementPlayerMachinePartsCount() for player");
        Debug.Log("[" + actorNumber + "] IncrementPlayerMachinePartsCount() IsMine value:" + photonView.IsMine);
        Debug.Log("[" + actorNumber + "] IncrementPlayerMachinePartsCount() Incrementing player machine count:" + playerMachinePartsCount);

        // Increment the player's machine parts count
        playerMachinePartsCount++;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "MachinePartsCount", playerMachinePartsCount } });
        Debug.Log("[" + actorNumber + "] IncrementPlayerMachinePartsCount() Incremented player machine count:" + playerMachinePartsCount);

        UpdatePlayerCollectibleCountText();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("something room  based just got updated..."+propertiesThatChanged);
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("teamMachinePartsCount", out object _teamMachinePartsCount))
        {
            teamMachinePartsCount = (int)_teamMachinePartsCount;
            Debug.Log("teamMachinePartsCount just got updated: " + teamMachinePartsCount);

            UpdateTeamCollectibleCountText();
        }
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("MachinePartsCount", out object _playerMachinePartsCount))
        {
            playerMachinePartsCount = (int)_playerMachinePartsCount;
            Debug.Log("playerMachinePartsCount just got updated: " + teamMachinePartsCount);

            UpdateTeamCollectibleCountText();
        }
    }

    //Method to update the team collectible count text
    [PunRPC]
    private void UpdateTeamCollectibleCountText()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        string realm = (string)PhotonNetwork.LocalPlayer.CustomProperties["Realm"];
        Debug.Log("[" + actorNumber + "] RPC UpdateTeamCollectibleCountText() Updating text for realm: " + realm);

        UpdateTeamCollectibleCountText(realm);
    }

    [PunRPC]
    private void UpdateTeamCollectibleCountText(string realm)
    {
        teamMachinePartsCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["teamMachinePartsCount"];

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        Debug.Log("[" + actorNumber + "] UpdateTeamCollectibleCountText(realm) invoked! realm:" + realm);
        Debug.Log("[" + actorNumber + "] UpdateTeamCollectibleCountText(realm) invoked! team machine parts count:" + teamMachinePartsCount);

        if ("Light" == realm)
        {
            // Reference to the TextMeshPro object that will display the team collectible count
            TextMeshProUGUI teamCollectibleCountText = GameObject.Find("MachinePartsTeam").GetComponent<TextMeshProUGUI>();

            Debug.Log("[" + actorNumber + "] UpdateTeamCollectibleCountText(realm) Updating team machine count text:" + teamMachinePartsCount);

            // Update the Team collectible count text
            teamCollectibleCountText.text = "Team Machine Parts: " + teamMachinePartsCount;
        }
    }

    //Method to update the player collectible count text
    [PunRPC]
    private void UpdatePlayerCollectibleCountText()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        string realm = (string)PhotonNetwork.LocalPlayer.CustomProperties["Realm"];
        Debug.Log("[" + actorNumber + "] RPC UpdatePlayerCollectibleCountText() Updating text for realm: " + realm);

        UpdatePlayerCollectibleCountText(realm);
    }

    private void UpdatePlayerCollectibleCountText(string realm)
    {
        //playerMachinePartsCount = (int)PhotonNetwork.LocalPlayer.CustomProperties["MachinePartsCount"];

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        Debug.Log("[" + actorNumber + "] UpdateCollectibleCountText(realm) invoked! realm:" + realm);
        Debug.Log("[" + actorNumber + "] UpdateCollectibleCountText(realm) invoked! player machine parts count:" + playerMachinePartsCount);

        if ("Light" == realm)
        {
            // Reference to the TextMeshPro object that will display the team collectible count
            TextMeshProUGUI playerCollectibleCountText = GameObject.Find("MachinePartsPlayer").GetComponent<TextMeshProUGUI>();

            Debug.Log("[" + actorNumber + "] UpdateCollectibleCountText(realm) Updating player machine count text:" + playerMachinePartsCount);

            // Update the Player collectible count text
            playerCollectibleCountText.text = "Player Machine Parts: " + playerMachinePartsCount;
        }
    }

    // Seems this has to be implemented!!
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {}
}