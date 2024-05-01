using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class VideoEndDetector : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject rawImageGameObject; // Drag the GameObject with the RawImage in the Unity Editor

    void Start()
    {
        videoPlayer.isLooping = false;
        videoPlayer.Play();
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        Destroy(rawImageGameObject);  // Destroy only the GameObject with the RawImage
    }
}
