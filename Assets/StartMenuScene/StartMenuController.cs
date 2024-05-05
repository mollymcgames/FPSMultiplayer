using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
   
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("FPSScene");
    }

   
    public void Logout()
    {
        PhotonNetwork.LoadLevel("Login");

    }


    public void ExitGame()
    {
        Application.Quit();       
    }

}
