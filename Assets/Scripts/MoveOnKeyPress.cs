using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveOnKeyPress : MonoBehaviourPun
{
    public float lrOffset = 52f;
    public float drOffset = 50f;

    void Update()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("CurrentRealm", out object _currentRealm);

        bool isLightRealm = ((string)_currentRealm == "Light");

        // If we are in the second realm, we can't go back to the first realm
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                // @TODO This is where a cool off period can be implemented.  Might want to flash a message on screen if attempting
                // to switch realms when WITHIN the cool off period.
                if (isLightRealm)
                {
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "CurrentRealm", "Dark" } });
                    photonView.RPC("MoveToDR", RpcTarget.AllBuffered, lrOffset);
                    CameraLayersController.switchToDR();
                }
                else
                {
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "CurrentRealm", "Light" } });
                    photonView.RPC("MoveToLR", RpcTarget.AllBuffered, drOffset);
                    CameraLayersController.switchToLR();
                }
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                NetworkManager.SpawnMachineParts();
            }
        }
    }

    [PunRPC]
    void MoveToDR(float yoffset)
    {
        Vector3 currentPos = transform.position;
        currentPos.y += yoffset;
        transform.position = currentPos;
    }

    [PunRPC]
    void MoveToLR(float doffset)
    {
        Vector3 currentPos = transform.position;
        currentPos.y -= doffset;
        transform.position = currentPos;
    }
}
