using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviourPunCallbacks
{
    public void Start()
    {
        GameOverKill();
    }

    public void GameOverKill()
    {
        Debug.Log("Killing player!");
        PhotonNetwork.Disconnect();
    }
}
