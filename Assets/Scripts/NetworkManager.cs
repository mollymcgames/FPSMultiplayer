using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public static NetworkManager instance;
    public GameObject player;
    public Transform[] spawnPoints;

    public GameObject roomCamera;

    [Space]
    public GameObject nameUI;
    public GameObject connectUI;

    private string nickname = "unnamed";

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

        RespawnPlayer();
    }

    public void RespawnPlayer()
    {

        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().LocalPlayer(); //Only called on local player or the person running the game
        _player.GetComponent<Health>().isLocalPlayer = true;
        _player.GetComponent<PhotonView>().RPC("SetName", RpcTarget.AllBuffered, nickname); //buffers the name so that it is set for all players
    }
}
