using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Health : MonoBehaviourPunCallbacks
{
    public int health;

    public bool isLocalPlayer;

    public TextMeshProUGUI healthText;


    [PunRPC]
    public void TakeDamage(int damage)
    {
        health -= damage;

        healthText.text = health.ToString();
        
        if (health <= 0)
        {
            if (isLocalPlayer)
            {
                NetworkManager.instance.RespawnPlayer();
            }

            if (photonView.IsMine)
            {
                if (gameObject != null && photonView != null)
                    PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Not mine");
            }
        }
    }
}
