using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviours : MonoBehaviour
{
    public void OnBackToLobbyButtonPress()
    {
        FPSGameManager.Instance.PlayerInfo.reloadRequired = true;
        SceneManager.LoadScene("StartMenu");
    }
}
