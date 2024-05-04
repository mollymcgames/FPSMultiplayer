using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSGameManager : MonoBehaviour
{
    public static FPSGameManager Instance;

    public string apiUrl;

    public PlayerInfo PlayerInfo;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
