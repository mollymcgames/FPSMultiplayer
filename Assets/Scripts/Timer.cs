using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviourPunCallbacks, IPunObservable
{
    public float gameRoundDuration = 60 * 10;
    public bool timerIsRunning = false;
    [SerializeField] private float timeRemaining = 0;

    private void Start()
    {
        Debug.Log("Initiating timer...");
        timeRemaining = gameRoundDuration;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(">>> This player just left, handing over timer: " + otherPlayer.NickName);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            timerIsRunning = true;
        }
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
                    PhotonNetwork.LoadLevel(GameUtils.SceneRoundOver);
                }
            }
        }
    }

    [PunRPC]
    public void DisplayTime(float timeToDisplay)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            timeToDisplay += 1;

            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            GameObject timerText = GameObject.Find("TimerText");
            TextMeshProUGUI timeText = null;
            if (timerText != null)
                timeText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
            if (timeText != null)
                timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(timeRemaining);
        }
        else
        {
            timeRemaining = (float)stream.ReceiveNext();
        }
    }
}