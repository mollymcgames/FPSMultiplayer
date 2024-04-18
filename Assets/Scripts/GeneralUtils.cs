using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUtils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WakeMouse();
        DisconnectFromGame();
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
}
