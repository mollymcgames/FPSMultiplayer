using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Photon.Realtime;
using UnityEngine.Networking;

public class GeneralUtils : MonoBehaviour
{

    //By default the winner is Dark 
    public string theWinner = "Dark";

    public AudioClip theWinnerAudio;
    public AudioClip theLoserAudio;

    void Start()
    {
        WakeMouse();
        DisconnectFromGame();
        // Set the text value of "RoundWonByText" to theWinner
        UpdateRoundWonByText();

        //check if the winner has been set in the room proerties
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Winner"))
        {
            theWinner = (string)PhotonNetwork.CurrentRoom.CustomProperties["Winner"];
            Debug.Log("The winner in general utils is: " + PhotonNetwork.CurrentRoom.CustomProperties["Winner"]);
            UpdateRoundWonByText();
        }

        AudioSource mainCameraAudioSource = gameObject.GetComponent<AudioSource>();
        if ((string)PhotonNetwork.LocalPlayer.CustomProperties["Realm"] == theWinner)
        {            
            mainCameraAudioSource.clip = theWinnerAudio;
        }
        else
        {
            mainCameraAudioSource.clip = theLoserAudio;
        }
        mainCameraAudioSource.loop = true;
        mainCameraAudioSource.Play();

        Dictionary<int, Player> playerList = PhotonNetwork.CurrentRoom.Players;
        foreach (var player in playerList)
        {
            Player nextPlayer = player.Value;
            Debug.Log("Player "+nextPlayer.NickName+"'s realm is: " + nextPlayer.CustomProperties["Realm"]);
            if ((string)nextPlayer.CustomProperties["Realm"] == theWinner)
            {
                int playerInfoId = (int)nextPlayer.CustomProperties["PlayerInfoId"];
                int playerInfoGoldCoins = (int)nextPlayer.CustomProperties["goldCoins"];
                playerInfoGoldCoins += GoldCoinCalculator.CalculateGoldCoinReturn(GoldCoinCalculator.GoldEarners.teamWon);
                PlayerInfo updatedPlayerInfo = new PlayerInfo();
                updatedPlayerInfo.id = playerInfoId;
                updatedPlayerInfo.goldCoins = playerInfoGoldCoins;
                Debug.Log("Updating player with id: " + updatedPlayerInfo.id);
                StartCoroutine (Main.instance.Web.UpdatePlayerInfo(updatedPlayerInfo));
            }
        }

        FPSGameManager.Instance.PlayerInfo.reloadRequired = true;
    }
    void WakeMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void DisconnectFromGame()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }        
    }

    void UpdateRoundWonByText()
    {
        TextMeshProUGUI roundWonByText = GameObject.Find("RoundWonByText").GetComponent<TextMeshProUGUI>();
        roundWonByText.text = theWinner;
    }

    public void UpdateRoundWonBy(string winner)
    {
        theWinner = winner;
        UpdateRoundWonByText();
    }
}
