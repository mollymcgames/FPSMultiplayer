using Photon.Pun;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    public AudioClip clipUiLoading;
    public AudioClip clipUiLogout;
    public AudioClip clipUiExit;
    public AudioClip clipMenuMusic;

    private AudioSource audioSource;

    private void Start()
    {
        AudioSource mainCameraAudioSource = gameObject.GetComponent<AudioSource>();
        if ( gameObject.GetComponent<AudioListener>() == null)
            gameObject.AddComponent<AudioListener>();
        mainCameraAudioSource.clip = clipMenuMusic;
        mainCameraAudioSource.loop = true;
        mainCameraAudioSource.Play();
        /*        if (FPSGameManager.Instance.PlayerInfo.reloadRequired == false)
                {
                    GameObject temp = GameObject.Find("MenuMusic");
                    if (temp != null)
                        audioSource = GetComponent<AudioSource>();            
                    if (audioSource != null && !audioSource.isPlaying)
                    {
                        Debug.Log("Making another new thing");

                        DontDestroyOnLoad(mainCameraAudioSource);
                    }
                }*/
    }

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
        mainCameraAudioSource.loop = false;
        mainCameraAudioSource.Play();
        if (GameObject.Find("FPSGameManager"))
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
