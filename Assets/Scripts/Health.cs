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


    [PunRPC]
    [Obsolete]
    public void TakeDamage(int damage, string userIdOfAttackingPlayer)
    {
        Debug.Log("Taking damage from: " + userIdOfAttackingPlayer);

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
