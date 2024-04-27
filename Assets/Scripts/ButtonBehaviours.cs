using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviours : MonoBehaviour
{
    public void OnBackToLobbyButtonPress()
    {
        Debug.Log("I'm going back to the Lobby! : " + PhotonNetwork.LocalPlayer.NickName);
        PhotonNetwork.LoadLevel(GameUtils.SceneMainGame);
    }
}
