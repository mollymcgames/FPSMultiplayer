using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GeneralUtils : MonoBehaviour
{

    //By default the winner is Dark 
    public string theWinner = "Dark";

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
