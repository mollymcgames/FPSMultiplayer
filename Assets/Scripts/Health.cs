using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;

public class Health : MonoBehaviourPunCallbacks
{
    public int health;

    public bool isLocalPlayer;

    public TextMeshProUGUI healthText;
    public AudioClip hitSound;

    private AudioSource mainCameraAudioSource;

    private void Start()
    {
        mainCameraAudioSource = gameObject.GetComponent<AudioSource>();
    }

    [PunRPC]    
    public void TakeDamage(int damage, string userIdOfAttackingPlayer)
    {
        Debug.Log("Taking damage from: " + userIdOfAttackingPlayer);

        mainCameraAudioSource.clip = hitSound;
        mainCameraAudioSource.loop = false;
        mainCameraAudioSource.Play();

        health -= (int)Math.Floor(damage * HealthVulnerabilityCalculator.CalculateDamageModifier((string)PhotonNetwork.LocalPlayer.CustomProperties["Realm"]));

        healthText.text = health.ToString();

        if (health <= 0)
        {
            if (isLocalPlayer)
            {
                int silverCoinsForTheKill = SilverCoinCalculator.CalculateSilverCoinReturn(SilverCoinCalculator.SilverEarners.playerKilled);
                Debug.Log("silverCoinsForTheKill:" + silverCoinsForTheKill);
                this.GetComponent<PhotonView>().RPC("AwardSilverCoins", RpcTarget.AllBuffered, userIdOfAttackingPlayer, silverCoinsForTheKill);

                NetworkManager.instance.RespawnPlayer();
            }

            if (photonView.IsMine)
            {
                if (gameObject != null && photonView != null)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
        else
        {
            if (isLocalPlayer)
            {
                int silverCoinsForTheHit = SilverCoinCalculator.CalculateSilverCoinReturn(SilverCoinCalculator.SilverEarners.playerHit);
                Debug.Log("silverCoinsForTheHit:" + silverCoinsForTheHit);
                this.GetComponent<PhotonView>().RPC("AwardSilverCoins", RpcTarget.AllBuffered, userIdOfAttackingPlayer, silverCoinsForTheHit);
            }
        }
    }
}
