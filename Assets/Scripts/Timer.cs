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
    public float timeRemaining = 60 * 10;
    public bool timerIsRunning = false;

    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
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
                    SceneManager.LoadScene(1);
                    timeRemaining = 0;
                    timerIsRunning = false;
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