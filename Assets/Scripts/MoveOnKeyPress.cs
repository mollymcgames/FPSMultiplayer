using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveOnKeyPress : MonoBehaviourPun
{
    public float yoffset = 52f;

    public float doffset = 50f;
    private bool inFirstRealm = true; //Have a bool to check if we are in the second realm or not

    // Update is called once per frame
    void Update()
    {
        //If we are in the second realm, we can't go back to the first realm
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (inFirstRealm)
                {
                    photonView.RPC("Move", RpcTarget.AllBuffered, yoffset);
                }
                else
                {
                    photonView.RPC("MoveDown", RpcTarget.AllBuffered, doffset);
                }
            }
        }

        // if (photonView.IsMine && isInSecondRealm)
        // {
        //     if (Input.GetKeyDown(KeyCode.V))
        //     {
        //         photonView.RPC("MoveDown", RpcTarget.AllBuffered, yoffset);
        //         isInSecondRealm = false; //If we are in the first realm, set the bool to false
        //     }
        // }

        // if (photonView.IsMine && isInSecondRealm)
        // {
        //     if (Input.GetKeyDown(KeyCode.V))
        //     {
        //         photonView.RPC("MoveDown", RpcTarget.AllBuffered, yoffset);
        //         isInSecondRealm = false; //If we are in the first realm, set the bool to false
        //     }
        // }
        
    }

    [PunRPC]

    void Move(float yoffset)
    {
        Vector3 curerntPos = transform.position;
        curerntPos.y += yoffset;
        transform.position = curerntPos;
        inFirstRealm = false; //If we are in the second realm, set the bool to true
    }



    [PunRPC]
    void MoveDown(float doffset)
    {
        Vector3 curerntPos = transform.position;
        curerntPos.y -= doffset;
        transform.position = curerntPos;
        inFirstRealm = true; //If we are in the first realm, set the bool to false
    }
}
