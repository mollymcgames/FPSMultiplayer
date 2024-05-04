using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRBackgroundMusicZone : MonoBehaviour
{

    public AudioClip backgroundMusic;

    private string playerTag = "Player";

    private AudioSource mainCameraAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider's game object has the player tag
        if (other.gameObject.CompareTag(playerTag))
        {
            // This means the player has entered the trigger
            Debug.Log("Player entered the trigger deep in the DR!!");

            // You can perform any actions here for when the player enters the trigger

            AudioSource otherAudio = GameObject.Find("LR-SkySphere").GetComponent<AudioSource>();
            Debug.Log("Stopping LR music: " + otherAudio.clip);
            otherAudio.Stop();

            mainCameraAudioSource = gameObject.GetComponent<AudioSource>();
            mainCameraAudioSource.Stop();
            mainCameraAudioSource.clip = backgroundMusic;
            mainCameraAudioSource.loop = true;
            Debug.Log("DR Playing this:"+mainCameraAudioSource.clip.ToString());
            if (mainCameraAudioSource.isPlaying == false)
            {
                mainCameraAudioSource.Play();
            }
        }
    }

}
