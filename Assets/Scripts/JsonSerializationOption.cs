using Newtonsoft.Json;
using System;
using UnityEngine;

public class JsonSerializationOption : ISerializationOption
{
    public string ContentType => "application/json";

    public T Deserialize<T>(string playerInfo)
    {
        try
        {
            var playerInfoRet = JsonConvert.DeserializeObject<T>(playerInfo);
            Debug.Log($"Success: {playerInfoRet}");
            return playerInfoRet;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Could not parse response {playerInfo}. {ex.Message}");
            return default;
        }
    }
}