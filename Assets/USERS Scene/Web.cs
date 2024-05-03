using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Web : MonoBehaviour
{
    public String apiUrl;

    IEnumerator GetUser(string username, string password)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl+"/player", "{ \"username\": \""+username+"\", \"password\": \""+password+"\" }", "application/json"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                byte[] results = www.downloadHandler.data;
            }
        }
    }
    
    public Text feedbackText;
    public IEnumerator Login(string username, string password)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl+"/playerLogin", "{ \"username\": \""+username+"\", \"password\": \""+password+"\" }", "application/json"))        
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                // Handle the response from the server
                string response = www.downloadHandler.text.Trim(); // Trim to remove any extra whitespace
                if (www.responseCode == 200)
                {
                    Debug.Log("Login successful: " + response);
                    feedbackText.text = ""; // Optionally clear or set a success message

                    // Store the logged in player info into the Game Manager for use in the next scene.
                    FPSGameManager.Instance.PlayerInfo = JsonConvert.DeserializeObject<PlayerInfo>(response);
                    

                    SceneManager.LoadSceneAsync(1);
                }
                else if (www.responseCode == 401)
                {
                    Debug.LogError("Login failed: Wrong credentials.");
                    feedbackText.text = "Wrong credentials. Please try again.";
                }
                else
                {
                    Debug.LogError("Login failed: Unexpected server response.");
                    feedbackText.text = "Login failed. Please try again.";
                }

            }
        }
    }

    public Text feedbackText2;
    public IEnumerator RegisterUser(string username, string password)
    {
        Debug.Log("Creating user at this API:"+apiUrl+"/player");
        string jsonRequest = "{ \"name\": \"testName\", \"punPlayerId\": \"testPunId\", \"goldCoins\": 0, \"username\": \""+username+"\", \"password\": \""+password +"\"}";
        Debug.Log("Creating user here:"+jsonRequest);

        using (UnityWebRequest www = UnityWebRequest.Put(apiUrl+"/player", jsonRequest))
        {
            www.SetRequestHeader ("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
               Debug.Log(www.downloadHandler.text);
                // Handle the response from the server
                string response = www.downloadHandler.text.Trim(); // Trim to remove any extra whitespace
                if (www.responseCode == 201)
                {
                    Debug.Log("Registration successful");                   
                    SceneManager.LoadSceneAsync(0);
                }
                else if (response == "Username is already taken.")
                {
                    Debug.LogError("Registration failed: Username taken.");
                    feedbackText2.text = "Username taken";
                }              
                else
                {
                    Debug.LogError("Registration failed: Unexpected server response.");
                    feedbackText2.text = "Registration failed. Please try again.";
                }

            }
            
        }
    }
}