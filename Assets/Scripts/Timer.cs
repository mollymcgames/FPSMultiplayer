using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviourPun
{
    public float gameRoundDuration = 60 * 10;
    public bool timerIsRunning = false;
    private float timeRemaining = 0;

    private void Start()
    {
        timeRemaining = gameRoundDuration;
        // Starts the timer automatically
        timerIsRunning = false;
    }

    void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (timerIsRunning)
            {                
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;                    
                    photonView.RPC("DisplayTime", RpcTarget.AllBuffered, timeRemaining);
                }
                else
                {
                    Debug.Log("Time has run out!");
                    timeRemaining = gameRoundDuration;
                    timerIsRunning = false;
                    PhotonNetwork.LoadLevel("GameOverScene");
                }
            }
        }
    }

    [PunRPC]
    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        TextMeshProUGUI timeText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}