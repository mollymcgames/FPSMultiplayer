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

    private List<PlayerInfo> playersAtTheEnd;
    public TMP_Text leaderListNames;
    public TMP_Text leaderListScores;

    void Start()
    {
        WakeMouse();
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

        PlayerInfo nextListPlayer;
        playersAtTheEnd = new List<PlayerInfo>();
        Dictionary<int, Player> playerList = PhotonNetwork.CurrentRoom.Players;
        foreach (var player in playerList)
        {
            Player nextPlayer = player.Value;
            Debug.Log("Player " + nextPlayer.NickName + "'s realm is: " + nextPlayer.CustomProperties["Realm"]);

            nextListPlayer = new PlayerInfo();
            // Random coin amount for now. Ultimately the correct amount needs to be used from across the PUN network
            // nextListPlayer.silverCoins = (int)nextPlayer.CustomProperties["silverCoins"];
            nextListPlayer.silverCoins = UnityEngine.Random.Range(0, 56);
            nextListPlayer.goldCoins = (int)nextPlayer.CustomProperties["goldCoins"];
            nextListPlayer.username = nextPlayer.NickName;

            playersAtTheEnd.Add(nextListPlayer);

            if ((string)nextPlayer.CustomProperties["Realm"] == theWinner)
            {
                int playerInfoId = (int)nextPlayer.CustomProperties["PlayerInfoId"];
                int playerInfoGoldCoins = (int)nextPlayer.CustomProperties["goldCoins"];
                playerInfoGoldCoins += GoldCoinCalculator.CalculateGoldCoinReturn(GoldCoinCalculator.GoldEarners.teamWon);
                PlayerInfo updatedPlayerInfo = new PlayerInfo();
                updatedPlayerInfo.id = playerInfoId;
                updatedPlayerInfo.goldCoins = playerInfoGoldCoins;
                Debug.Log("Updating player with id: " + updatedPlayerInfo.id);
                StartCoroutine(Main.instance.Web.UpdatePlayerInfo(updatedPlayerInfo));
            }
        }

        FPSGameManager.Instance.PlayerInfo.reloadRequired = true;

        playersAtTheEnd.Sort((a, b) => a.silverCoins.CompareTo(b.silverCoins));
        foreach (PlayerInfo p in playersAtTheEnd)
        {
            leaderListNames.text += string.Format("{0}\n", p.username);
            leaderListScores.text += string.Format("{0}\n", p.silverCoins);
        }

        DisconnectFromGame();
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
