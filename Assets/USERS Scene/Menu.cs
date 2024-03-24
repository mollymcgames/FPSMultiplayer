using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Login Login;
    public void PlayGame()
     {

      SceneManager.LoadSceneAsync(1);
    
    }
   
}
