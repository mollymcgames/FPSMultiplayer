using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class GetApiData : MonoBehaviourPunCallbacks
{
    public string URL;

    public void FetchPlayerDataByPunPlayerId(int playerId)
    {
        StartCoroutine(FetchPlayerData(playerId));
    }

    public IEnumerator FetchPlayerData(int playerId)
    {
        Debug.Log("Dude");

        using (UnityWebRequest request = UnityWebRequest.Get(URL + "/player/" + playerId))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                PlayerInfo playerInfo = new PlayerInfo();
                //Debug.Log("Received: " + request.downloadHandler.text);
                playerInfo = JsonUtility.FromJson<PlayerInfo>(request.downloadHandler.text);
                Debug.Log("Player name from API: " + playerInfo.name);
            }
        }
    }
}