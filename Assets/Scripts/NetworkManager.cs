using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;
using System;
using UnityEngine.UI;
using Photon.Pun.Demo.PunBasics;
using System.Net.Http;
//'PunTeams' is obsolete: 'do not use this or add it to the scene. use PhotonTeamsManager instead'CS0618
// using Photon.Pun.UtilityScripts;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private Timer timer;
    public string playerApiUrl;

    public static NetworkManager instance;
    public GameObject lightRealmPlayerPrefab; //prefab for light realm player

    public GameObject darkRealmPlayerPrefab; //prefab for dark realm player
    public Transform[] spawnPointsLR;
    public Transform[] spawnPointsDR;

    public GameObject roomCamera;

    [Space]
    public GameObject nameUI;
    public GameObject playerUIImage;
    
    public GameObject connectUI;

    private string nickname = "unnamed";
    private string realm = "";

    private bool spawnlightRealmPlayer = true; //flag to determine which team to spawn next

    public void Start()
    {
        Debug.Log("Starting <HI>");
        timer = GetComponent<Timer>();
    }

    void Awake()
    {
        instance = this;
        //set the playerUI alpha to 0
        SetImageAlpha(playerUIImage, 0f);
    }

    public void ChangeNickname(string name)
    {
        nickname = name;
    }

    public void JoinRoomButtonPressed()
    {
        Debug.Log("Connecting to server...");

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        nameUI.SetActive(false);
        connectUI.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected to server!");

        PhotonNetwork.JoinLobby();
    }

    public override async void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        var options = new RoomOptions() { };
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        options.CustomRoomProperties.Add("teamMachinePartsCount", 0);

        PhotonNetwork.JoinOrCreateRoom("test", roomOptions:options, null);

        PlayerInfo pf = null;
        var apiClient = new DatabaseApiClient(playerApiUrl, new JsonSerializationOption());
        pf = await apiClient.GetPlayer<PlayerInfo>(1);

        // @TODO Temporary usage of the PlayerInfo object. Really would need to use it to help setup the game!
        TextMeshProUGUI playerGoldCoins = GameObject.Find("PlayerGoldCoins").GetComponent<TextMeshProUGUI>();
        playerGoldCoins.text = pf.goldCoins.ToString();

        Debug.Log("Joined lobby!");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room!");

        roomCamera.SetActive(false);

        SpawnNewPlayer();
        timer.timerIsRunning = true;
        Room currentRoom = PhotonNetwork.CurrentRoom;
        Debug.Log("current room properties: " + currentRoom.CustomProperties);    
        // When the room is joined, make the image visible by setting alpha to 1
        SetImageAlpha(playerUIImage, 1f);            
    }    

    public void RespawnPlayer()
    {
        Debug.Log("Respawning player: " + nickname);

        // Get the existing player's ID
        int localPlayerID = PhotonNetwork.LocalPlayer.ActorNumber;

        // Find the existing player object
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView != null && photonView.Owner.ActorNumber == localPlayerID)
            {
                // Move the existing player object to a hidden area (e.g., move it far away)
                player.transform.position = new Vector3(9999f, 9999f, 9999f);
                break;
            }
        }

        // Get the existing player's team
        bool isLightRealm = ((string)PhotonNetwork.LocalPlayer.CustomProperties["Realm"] == "Light");

        // Assign the player's prefab and spawn point based on the existing team
        GameObject playerPrefab = DeterminePlayerPrefab(isLightRealm);
        Transform spawnPoint = DetermineSpawnPoint(isLightRealm);

        // Reset their MachinePartsCount to zero
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "MachinePartsCount", 0 } });
        // And what realm they're currently in
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "CurrentRealm", isLightRealm ? "Light" : "Dark"} });

        // Instantiate the player at the spawn point
        GameObject _player = deployPlayer(playerPrefab, spawnPoint);

        setOnScreenPlayerStatsAndVisibility(_player);
    }

    void SpawnNewPlayer()
    {
        Debug.Log("*** NEW PLAYER JOINING ***");

        // Set the realm of the player based on the number of players in the room
        bool isLightRealm = PhotonNetwork.PlayerList.Length % 2 == 1;  // Check if the number of players in the room is even
        // bool isLightRealm = PhotonNetwork.PlayerList.Length < 3;

        realm = isLightRealm ? "Light" : "Dark";
        Debug.Log("Player [" + nickname + "] assigned to realm [" + realm + "]");

        // Assign the player's prefab and spawn point
        GameObject playerPrefab = DeterminePlayerPrefab(isLightRealm);
        Transform spawnPoint = DetermineSpawnPoint(isLightRealm);

        // Initiliase the player's machine parts count.
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Realm", realm }, { "MachinePartsCount", 0 } });

        // And what realm they're currently in
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "CurrentRealm", isLightRealm ? "Light" : "Dark" } });

        // Instantiate the player at the spawn point
        GameObject _player = deployPlayer(playerPrefab, spawnPoint);

        SpawnMachineParts();

        setOnScreenPlayerStatsAndVisibility(_player);

        //Spawn the machine to fix
        SpawnMachineToFix();
    }

    public static void SpawnMachineParts()
    {
        Debug.Log("Spawning machine parts...");

        // Instantiate the Machine Parts too (this has to be done by Photon, so that Photon can correctly manage their lifecycle!
        GameObject machine1Prefab = (GameObject)Resources.Load("Machine", typeof(GameObject));
        GameObject machine2Prefab = (GameObject)Resources.Load("Machine2", typeof(GameObject));

        PhotonNetwork.InstantiateRoomObject(machine1Prefab.name, machine1Prefab.transform.position, Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(machine2Prefab.name, machine2Prefab.transform.position, Quaternion.identity);
    }

    public static void SpawnMachineToFix()
    {
        Debug.Log("Spawning machine to fix...");

        // Instantiate the Machine to fix (this has to be done by Photon, so that Photon can correctly manage their lifecycle!
        GameObject machineToFixPrefab = (GameObject)Resources.Load("MachineToFix", typeof(GameObject));

        PhotonNetwork.InstantiateRoomObject(machineToFixPrefab.name, machineToFixPrefab.transform.position, machineToFixPrefab.transform.rotation);
        
    }

    private GameObject DeterminePlayerPrefab(bool isLightRealm)
    {
        return isLightRealm ? lightRealmPlayerPrefab : darkRealmPlayerPrefab;
    }

    private Transform DetermineSpawnPoint(bool isLightRealm)
    {
        return isLightRealm ? spawnPointsLR[UnityEngine.Random.Range(0, spawnPointsLR.Length)] : spawnPointsDR[UnityEngine.Random.Range(0, spawnPointsDR.Length)];
    }

    private GameObject deployPlayer(GameObject playerPrefab, Transform spawnPoint)
    {
        GameObject _player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().LocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;
        _player.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, nickname);
        return _player;
    }

    private void setOnScreenPlayerStatsAndVisibility(GameObject _player)
    {
        // Put name on screen
        TextMeshProUGUI nicknameText = GameObject.Find("Nickname").GetComponent<TextMeshProUGUI>();
        nicknameText.text = nickname;
        playerUIImage.SetActive(true);

        // Put realm on screen
        // TextMeshProUGUI realmText = GameObject.Find("Realm").GetComponent<TextMeshProUGUI>();
        // realmText.text = "Realm: " + realm;

        if ("Light" == realm)
        {
            CameraLayersController.switchToLR();
            _player.GetComponent<PhotonView>().RPC("UpdateTeamCollectibleCountText", RpcTarget.AllBufferedViaServer);
            _player.GetComponent<PhotonView>().RPC("UpdatePlayerCollectibleCountText", RpcTarget.AllBufferedViaServer);
            _player.GetComponent<PhotonView>().RPC("UpdateMachinesFixedCountText", RpcTarget.AllBufferedViaServer);
        }
        else
        {
            CameraLayersController.switchToDR();
        }
    }

    void SetImageAlpha(GameObject obj, float alpha)
    {
        // Get the Image component of the GameObject
        Image image = obj.GetComponent<Image>();

        // If the Image component is found
        if (image != null)
        {
            // Get the current color of the image
            Color currentColor = image.color;

            // Set the alpha value
            currentColor.a = alpha;

            // Apply the new color to the image
            image.color = currentColor;
        }
        else
        {
            Debug.LogWarning("No Image component found on the GameObject: " + obj.name);
        }
    }    
}