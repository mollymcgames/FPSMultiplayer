using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject camera;

    public string nickName;

    public TextMeshPro nicknameText;

    public void LocalPlayer()
    {
        playerMovement.enabled = true;
        camera.SetActive(true);
    }

    [PunRPC]
    public void SetName(string name)
    {
        nickName = name;
        nicknameText.text = nickName;
    }
}
