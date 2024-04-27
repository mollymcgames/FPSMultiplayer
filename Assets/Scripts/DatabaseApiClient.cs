using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseApiClient : MonoBehaviourPunCallbacks
{
    private readonly ISerializationOption _serializationOption;
    private string apiUrl;
    private static DatabaseApiClient _client = null;

    public static DatabaseApiClient GetDatabaseApiClient(string apiUrl, ISerializationOption serializationOption)
    {
        if (_client == null)
        {
            _client = new DatabaseApiClient(apiUrl, new JsonSerializationOption());
        }
        return _client;
    }

    private DatabaseApiClient(string apiUrl, ISerializationOption serializationOption)
    {
        _serializationOption = serializationOption;
        this.apiUrl = apiUrl;
    }

    public async Task<TResultType> GetPlayer<TResultType>(int playerId)
    {
        try
        {
            string url = apiUrl + "/player/" + playerId;
            Debug.Log("Hitting API: " + url);
            using var apiRequest = UnityWebRequest.Get(url);

            apiRequest.SetRequestHeader("Content-Type", _serializationOption.ContentType);

            var operation = apiRequest.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (apiRequest.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {apiRequest.error}");

            var result = _serializationOption.Deserialize<TResultType>(apiRequest.downloadHandler.text);

            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{nameof(GetPlayer)} failed: {ex.Message}");
            return default;
        }
    }
}