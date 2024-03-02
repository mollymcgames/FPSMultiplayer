using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;
using static Photon.Pun.UtilityScripts.PunTeams;
//'PunTeams' is obsolete: 'do not use this or add it to the scene. use PhotonTeamsManager instead'CS0618
// using Photon.Pun.UtilityScripts;

// IPunObservable
public class NetworkManager : MonoBehaviourPunCallbacks
{

    public static NetworkManager instance;
    public GameObject lightRealmPlayerPrefab; //prefab for light realm player

    public GameObject darkRealmPlayerPrefab; //prefab for dark realm player
    public Transform[] spawnPoints;

    public GameObject roomCamera;

    [Space]
    public GameObject nameUI;
    public GameObject connectUI;

    private string nickname = "unnamed";

    private bool spawnlightRealmPlayer = true; //flag to determine which team to spawn next

    void Awake()
    {
        instance = this;
    }

    public void ChangeNickname(string name)
    {
        nickname = name;
    }

    public void JoinRoomButtonPressed()
    {
        Debug.Log("Connecting to server...");

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

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        var options = new RoomOptions() { };
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        options.CustomRoomProperties.Add("teamMachinePartsCount", 0);

        PhotonNetwork.JoinOrCreateRoom("test", roomOptions:options, null);

        Debug.Log("Joined lobby!");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room!");

        roomCamera.SetActive(false);

        AssignPlayerTeam();

        Room currentRoom = PhotonNetwork.CurrentRoom;
        Debug.Log("current room properties: " + currentRoom.CustomProperties);        
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
        string realm = (string)PhotonNetwork.LocalPlayer.CustomProperties["Realm"];

        // Assign the player's team based on the existing team
        GameObject playerPrefab = realm == "Light" ? lightRealmPlayerPrefab : darkRealmPlayerPrefab;

        // Get a random spawn point
        //TO DO - if realm is dark then spawn at dark spawn point array else spawn at light spawn point array
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate the player at the spawn point
        GameObject _player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().LocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;
        _player.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, nickname);

        // @TODO UGLY code reuse - this is done twice! Pull out into a shared function.
        // Put name on screen
        TextMeshProUGUI nicknameText = GameObject.Find("Nickname").GetComponent<TextMeshProUGUI>();
        nicknameText.text = "Name: " + nickname;

        // Put realm on screen
        TextMeshProUGUI realmText = GameObject.Find("Realm").GetComponent<TextMeshProUGUI>();
        realmText.text = "Realm: " + realm;

        if ("Light" == realm)
        {
            CameraLayersController.switchToLR();
            _player.GetComponent<PhotonView>().RPC("UpdateCollectibleCountText", RpcTarget.AllBufferedViaServer);
        } 
        else
        {
            CameraLayersController.switchToDR();
        }

    }


    void AssignPlayerTeam()
    {
        Debug.Log("*** NEW PLAYER JOINING ***");

        //TO DO - if realm is dark then spawn at dark spawn point array else spawn at light spawn point array
        //TO DO - then make the code into one function for spanwing player

        // Set the realm of the player based on the number of players in the room
        bool isLightRealm = PhotonNetwork.PlayerList.Length % 2 == 1;  // Check if the number of players in the room is even
        string realm = isLightRealm ? "Light" : "Dark";
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Realm", realm } });
        Debug.Log("Player ["+nickname+"] assigned to realm ["+realm+"]");

        GameObject playerPrefab = isLightRealm ? lightRealmPlayerPrefab : darkRealmPlayerPrefab;
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject _player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().LocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;
        _player.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, nickname);

        // Instantiate the Machine Parts too (this has to be done by Photon, so that Photon can correctly manage their lifecycle!
        GameObject machine1Prefab = (GameObject)Resources.Load("Machine", typeof(GameObject)); 
        GameObject machine2Prefab = (GameObject)Resources.Load("Machine2", typeof(GameObject)); 

        PhotonNetwork.InstantiateRoomObject(machine1Prefab.name, machine1Prefab.transform.position, Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(machine2Prefab.name, machine2Prefab.transform.position, Quaternion.identity);

        // @TODO code reuse - this is done twice! Pull out into a shared function.
        // Put name on screen
        TextMeshProUGUI nicknameText = GameObject.Find("Nickname").GetComponent<TextMeshProUGUI>();
        nicknameText.text = "Name: " + nickname;

        // Put realm on screen
        TextMeshProUGUI realmText = GameObject.Find("Realm").GetComponent<TextMeshProUGUI>();
        realmText.text = "Realm: " + realm;

        if ("Light" == realm)
        {                //Debug.Log("Invoking assignment on realm: " + realm);
            CameraLayersController.switchToLR();
            _player.GetComponent<PhotonView>().RPC("UpdateCollectibleCountText", RpcTarget.AllBufferedViaServer);
        }
        else
        {
            CameraLayersController.switchToDR();
        }
    }
}