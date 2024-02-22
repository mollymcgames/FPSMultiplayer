using UnityEngine;
using Photon.Pun;
using TMPro;


public class CollectibleCounter: MonoBehaviourPunCallbacks
{
    //Player should be able to pick up a collectible for the team
    //Player should be able to see how many collectibles they have and everyone else in the team should be able to see it also

    //Reference to the TextMeshPro object that will display the collectible count
    public TextMeshProUGUI collectibleCountText;

    //The number of collectibles the player has
    private int teamMachienePartsCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            //Incrememnt the teams machiene parts count
            IncrementTeamMachienePartsCount();

            //Get the collectible PhotonView
            PhotonView collectiblePhotonView = other.GetComponent<PhotonView>();

            //Check if the collectible is owned by the player
            if (collectiblePhotonView.IsMine)
            {
                //Destroy the collectible object
                PhotonNetwork.Destroy(other.gameObject);
            }
        }
    }

    //Method to increment the teams machiene parts count
    private void IncrementTeamMachienePartsCount()
    {
        //Increment the teams machiene parts count
        teamMachienePartsCount++;

        //Update the collectible count text
        UpdateCollectibleCountText();

        //Synchronize the collectible count with the other players in the team
        photonView.RPC("SyncTeamMachienePartsCount", RpcTarget.OthersBuffered, teamMachienePartsCount);
    }

    //Method to update the collectible count text
    private void UpdateCollectibleCountText()
    {
        //Update the collectible count text
        collectibleCountText.text = "Machiene Parts: " + teamMachienePartsCount;
    }

    //RPC method to synchronize the collectible count with the other players in the team
    [PunRPC]
    private void SyncTeamMachienePartsCount(int count)
    {
        //Set the teams machiene parts count to the new value
        teamMachienePartsCount = count;

        //Update the collectible count text
        UpdateCollectibleCountText();
    }
}