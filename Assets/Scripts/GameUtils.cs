using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUtils : MonoBehaviourPunCallbacks
{
    public static int SceneMainGame = 0;
    public static int SceneRoundOver = 1;
    public static int SceneGameOver = 2;

    [PunRPC]
    public void AwardSilverCoins(string userId, int coinsToAdd)
    {
        Debug.Log("Potentially awarding some silver coins [" + coinsToAdd + "] to [" + userId + "]");
        Debug.Log("I am user: " + PhotonNetwork.LocalPlayer.UserId);

        if (userId == PhotonNetwork.LocalPlayer.UserId)
        {
            Debug.Log("Awarded points to (me!) user: " + userId);

            // Get the existing player's team
            int newSilverCoins = (int)PhotonNetwork.LocalPlayer.CustomProperties["silverCoins"];
            newSilverCoins += coinsToAdd; // SilverCoinCalculator.CalculateSilverCoinReturn(SilverCoinCalculator.SilverEarners.playerHit);

            PhotonNetwork.LocalPlayer.CustomProperties["silverCoins"] = newSilverCoins;
            TextMeshProUGUI playerSilverCoins = GameObject.Find("PlayerSilverCoins").GetComponent<TextMeshProUGUI>();
            playerSilverCoins.text = newSilverCoins.ToString();
        }
    }
}
