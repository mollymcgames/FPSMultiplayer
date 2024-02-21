using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

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


    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Connecting to server...");

        // PhotonNetwork.ConnectUsingSettings();
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

        PhotonNetwork.JoinOrCreateRoom("test", null, null);

        Debug.Log("Joined lobby!");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room!");

        roomCamera.SetActive(false);

        // Set the realm of the player based on the number of players in the room
        bool isLightRealm = PhotonNetwork.PlayerList.Length % 2 == 0;  // Check if the number of players in the room is even
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Realm", isLightRealm ? "Light" : "Dark" } });
        AssignPlayerTeam();

        Room currentRoom = PhotonNetwork.CurrentRoom;
        Debug.Log("current room properties: " + currentRoom.CustomProperties);        
    }    
    public void RespawnPlayer()
    {
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
    }


    void AssignPlayerTeam()
    {
        //TO DO - if realm is dark then spawn at dark spawn point array else spawn at light spawn point array
        //TO DO - then make the code into one function for spanwing player

        bool isLightRealm = PhotonNetwork.PlayerList.Length % 2 == 0; // Assign alternating teams
        GameObject playerPrefab = isLightRealm ? lightRealmPlayerPrefab : darkRealmPlayerPrefab;
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject _player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().LocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;
        _player.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, nickname);

    }

    //TO DO - measure light or dark realm player count accordingly
    //TO DO - make sure player spawn points arent overlapping

}
