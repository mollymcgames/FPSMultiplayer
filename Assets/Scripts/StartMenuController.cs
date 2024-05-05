using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public AudioClip clipUiLoading;
    public AudioClip clipUiLogout;
    public AudioClip clipUiExit;

    public void StartGame()
    {
        AudioSource mainCameraAudioSource = gameObject.GetComponent<AudioSource>();
        mainCameraAudioSource.clip = clipUiLoading;
        mainCameraAudioSource.Play();
        PhotonNetwork.LoadLevel("FPSScene");
    }

   
    public void Logout()
    {
        AudioSource mainCameraAudioSource = gameObject.GetComponent<AudioSource>();
        mainCameraAudioSource.clip = clipUiLogout;
        mainCameraAudioSource.Play();
        FPSGameManager.Instance.PlayerInfo = null;

        // Stop any menu music now!
        Destroy(GameObject.Find("MainMenuCamera"));
        PhotonNetwork.LoadLevel("Login");
    }


    public void ExitGame()
    {    
        AudioSource mainCameraAudioSource = gameObject.GetComponent<AudioSource>();
        mainCameraAudioSource.clip = clipUiExit;
        mainCameraAudioSource.Play();

        // Stop any menu music now!
        Destroy(GameObject.Find("MainMenuCamera"));
        Application.Quit();       
    }

}
