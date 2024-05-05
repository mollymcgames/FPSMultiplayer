using Photon.Pun;
using UnityEngine;

public class Menu : MonoBehaviourPun
{
    public Login Login;
    public AudioClip clipMenuMusic;
    private AudioSource audioSource;

    private void Start()
    {
        GameObject menuBase = GameObject.Find("MenuMusic");
        if (menuBase == null)
        {
            audioSource = GameObject.Find("MainMenuCamera").GetComponent<AudioSource>();
            gameObject.name = "MenuMusic";
            AudioSource mainCameraAudioSource = gameObject.GetComponent<AudioSource>();
            mainCameraAudioSource.clip = clipMenuMusic;
            mainCameraAudioSource.loop = true;
            mainCameraAudioSource.Play();
            DontDestroyOnLoad(mainCameraAudioSource);
        }
    }

    public void PlayGame()
    {
        PhotonNetwork.LoadLevel("FPSScene");
    }
   
}
