using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviourPun
{
    public Login Login;

    public void PlayGame()
     {

      PhotonNetwork.LoadLevel("FPSScene");
    }
   
}
