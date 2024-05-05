using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviourPun
{
    public Login Login;
    public AudioClip clipMenuMusic;

    private void Start()
    {
        AudioSource mainCameraAudioSource = gameObject.GetComponent<AudioSource>();
        mainCameraAudioSource.clip = clipMenuMusic;
        mainCameraAudioSource.loop = true;
        mainCameraAudioSource.Play();
        DontDestroyOnLoad(mainCameraAudioSource);
    }

    public void PlayGame()
    {
        PhotonNetwork.LoadLevel("FPSScene");
    }
   
}
