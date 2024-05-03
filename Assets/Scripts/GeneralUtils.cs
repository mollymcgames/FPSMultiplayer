using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GeneralUtils : MonoBehaviour
{

    // public TextMeshProUGUI roundWonByText;
    //define the winner string to say "dark" or "light" won the game
    public string theWinner = "Dark";
    // Start is called before the first frame update
    void Start()
    {
        WakeMouse();
        DisconnectFromGame();
        // Set the text value of "RoundWonByText" to theWinner
        UpdateRoundWonByText();

        // Debug log to print out the entire custom properties
        Debug.Log("Custom Properties in general urils is: " + PhotonNetwork.CurrentRoom.CustomProperties);


        //check if the winner has been set in the room proerties
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Winner"))
        {
            theWinner = (string)PhotonNetwork.CurrentRoom.CustomProperties["Winner"];
            Debug.Log("The winner in general utils is: " + PhotonNetwork.CurrentRoom.CustomProperties["Winner"]);
            UpdateRoundWonByText();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
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
