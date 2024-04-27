using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MoveOnKeyPress : MonoBehaviourPun
{
    private float lrOffset = 317f;
    private float drOffset = 317f;

    public TextMeshProUGUI cooldownText;
    public GameObject realmSwitchVFXPrefab; // Reference to the VFX Prefab

    private bool canSwitchRealm = true;    

    void Start()
    {
        cooldownText.enabled = false; // Disable the cooldown message initially
    }    

    void Update()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("CurrentRealm", out object _currentRealm);

        bool isLightRealm = ((string)_currentRealm == "Light");

        // If we are in the second realm, we can't go back to the first realm
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.V) && canSwitchRealm)
            {
                if (isLightRealm)
                {
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "CurrentRealm", "Dark" } });
                    photonView.RPC("MoveToDR", RpcTarget.AllBuffered, lrOffset);
                    CameraLayersController.switchToDR();
                    PlayRealmSwitchVFX(transform.position); // Play the VFX when switching to Dark realm
                }
                else
                {
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "CurrentRealm", "Light" } });
                    photonView.RPC("MoveToLR", RpcTarget.AllBuffered, drOffset);
                    CameraLayersController.switchToLR();
                    PlayRealmSwitchVFX(transform.position); // Play the VFX when switching to Dark realm
                }
                //Start the cooldown co routine
                StartCoroutine(StartCooldown());
            }
            else if (Input.GetKeyDown(KeyCode.V) && !canSwitchRealm)
            {
                Debug.Log("Can't switch realms yet");
                cooldownText.enabled = true; //Show the cooldown text
                cooldownText.text = "Cannot switch realms yet. Cooldown is active.";
                StartCoroutine(HideCooldownMessage());
                
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            NetworkManager.SpawnMachineParts();
        }
    }

    [PunRPC]
    IEnumerator StartCooldown()
    {
        canSwitchRealm = false;
        yield return new WaitForSeconds(5);
        canSwitchRealm = true;
    }

    [PunRPC]
    IEnumerator HideCooldownMessage()
    {
        yield return new WaitForSeconds(2f); //Hide the cooldown message after 2 seconds
        cooldownText.enabled = false;
    }

    [PunRPC]
    void MoveToDR(float yOffset)
    {
        Debug.Log("Moving to DR, offset: " + lrOffset);
        Vector3 currentPos = transform.position;
        currentPos.y += lrOffset;
        transform.position = currentPos;
    }

    [PunRPC]
    void MoveToLR(float dOffset)
    {
        Debug.Log("Moving to LR, offset: " + drOffset);
        Vector3 currentPos = transform.position;
        currentPos.y -= drOffset;
        transform.position = currentPos;
    }

    [PunRPC]
    void PlayRealmSwitchVFX(Vector3 position)
    {
        // Add a downward offset (adjust the value as needed)
        position -= Vector3.up * 1.1f; //Todo - this could be done in a nicer way taking the location of the ground and player into account           

        // Instantiate the VFX Prefab at the specified position and play it
        GameObject vfxObject = PhotonNetwork.Instantiate(realmSwitchVFXPrefab.name, position, Quaternion.identity);
        if (vfxObject == null)
        {
            Debug.LogError("Failed to instantiate VFX prefab!");
            return;
        }        
        // Instantiate the VFX Prefab at the specified position and play it
        vfxObject.GetComponent<ParticleSystem>().Play();

        // Get the duration of the particle system
        ParticleSystem particleSystem = vfxObject.GetComponent<ParticleSystem>();
        float duration = particleSystem.main.duration + particleSystem.main.startLifetime.constantMax;

        // Destroy the VFX object after the duration for all the clients in game
        StartCoroutine(DestroyVFXObject(vfxObject, duration));
    }
    IEnumerator DestroyVFXObject(GameObject vfxObject, float duration)
    {
        yield return new WaitForSeconds(duration);
        PhotonNetwork.Destroy(vfxObject);
    }    
}
