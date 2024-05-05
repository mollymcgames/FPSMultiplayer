using Photon.Pun;
using System.Collections;
using TMPro;
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

        if (FPSGameManager.Instance != null && FPSGameManager.Instance.PlayerInfo.reloadRequired == true)
        {
            StartCoroutine(Main.instance.Web.RefreshUser(FPSGameManager.Instance.PlayerInfo.id));
        }
        FPSGameManager.Instance.PlayerInfo.reloadRequired = false;
        
        TextMeshProUGUI goldCoinsText = GameObject.Find("CoinsCount").GetComponent<TextMeshProUGUI>();
        goldCoinsText.text = FPSGameManager.Instance.PlayerInfo.goldCoins.ToString();
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

        StartCoroutine(WaitThenLogout(2));
    }

    IEnumerator WaitThenLogout(int duration)
    {
        yield return new WaitForSeconds(duration);
        // Stop any menu music now!
        Destroy(GameObject.Find("MainMenuCamera"));
        PhotonNetwork.LoadLevel("Login");
    }

    public void ExitGame()
    {    
        AudioSource mainCameraAudioSource = gameObject.GetComponent<AudioSource>();
        mainCameraAudioSource.clip = clipUiExit;
        mainCameraAudioSource.loop = false;
        mainCameraAudioSource.Play();

        StartCoroutine(WaitThenExit(2));
    }

    IEnumerator WaitThenExit(int duration)
    {
        yield return new WaitForSeconds(duration);
        // Stop any menu music now!
        Destroy(GameObject.Find("MainMenuCamera"));
        Application.Quit();
    }

}
