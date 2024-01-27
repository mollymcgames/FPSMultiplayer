using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject camera;

    public void LocalPlayer()
    {
        playerMovement.enabled = true;
        camera.SetActive(true);
    }

}
