using Newtonsoft.Json;
using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Web : MonoBehaviour
{
    public String apiUrl;
    public TextMeshProUGUI feedbackText;
    public GameObject errorPanel;

    private void Start()
    {
        FPSGameManager.Instance.apiUrl = apiUrl;
    }
    
    public IEnumerator RefreshUser(int userId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(FPSGameManager.Instance.apiUrl + "/player/" + userId))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                ShowError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string response = www.downloadHandler.text.Trim(); // Trim to remove any extra whitespace
                FPSGameManager.Instance.PlayerInfo = JsonConvert.DeserializeObject<PlayerInfo>(response);
            }
        }
    }

    private void ShowError(string errorMessage)
    {
        feedbackText.text = errorMessage;
        errorPanel.active = true;
    }

    private void HideError()
    {
        feedbackText.text = "";
        errorPanel.active = false;
    }

    public IEnumerator Login(string username, string password)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl+"/playerLogin", "{ \"username\": \""+username+"\", \"password\": \""+password+"\" }", "application/json"))        
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (www.responseCode == 401)
                    ShowError("Your credentials were incorrect, try again!");
                else
                    ShowError(www.error);
            }
            else
            {
                // Handle the response from the server
                string response = www.downloadHandler.text.Trim(); // Trim to remove any extra whitespace
                if (www.responseCode == 200)
                {
                    Debug.Log("Login successful: " + response);
                    HideError();

                    // Store the logged in player info into the Game Manager for use in the next scene.
                    FPSGameManager.Instance.PlayerInfo = JsonConvert.DeserializeObject<PlayerInfo>(response);

                    SceneManager.LoadScene("StartMenu");
                }
                else if (www.responseCode == 401)
                {
                    Debug.Log("Login failed: Wrong credentials.");
                    ShowError("Wrong credentials. Please try again.");
                }
                else
                {
                    Debug.Log("Login failed: Unexpected server response.");
                    ShowError("Login failed. Please try again.");
                }
            }
        }
    }    
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
                Debug.Log(www.error);
                ShowError(www.error);
            }
            else
            {
               Debug.Log(www.downloadHandler.text);
                // Handle the response from the server
                string response = www.downloadHandler.text.Trim(); // Trim to remove any extra whitespace
                if (www.responseCode == 201)
                {
                    Debug.Log("Registration successful");
                    SceneManager.LoadScene("Login");
                }
                else if (response == "Username is already taken.")
                {
                    Debug.Log("Registration failed: Username taken.");
                    ShowError("Username taken");
                }              
                else
                {
                    Debug.Log("Registration failed: Unexpected server response.");
                    ShowError("Registration failed. Please try again.");
                }

            }
            
        }
    }

    public IEnumerator UpdatePlayerInfo(PlayerInfo playerInfo)
    {
        // JUST UPDATES GOLD COINS FOR NOW!
        using (UnityWebRequest www = UnityWebRequest.Post(FPSGameManager.Instance.apiUrl + "/player/" + playerInfo.id, "{ \"goldCoins\": \"" + playerInfo.goldCoins + "\"}", "application/json"))
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
                if (www.responseCode != 200)
                {
                    Debug.Log("Ooops, didn't save: " + response);
                }
            }
        }
    }
}