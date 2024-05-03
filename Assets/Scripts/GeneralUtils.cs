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
