using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NetworkController : MonoBehaviourPunCallbacks
{
    bool IsConnecting;
    public TMP_InputField RoomName;
    const string GameVersion = "0.1";
    const int maxPlayersPerRoom = 4;
    public Button CreateRoom;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        /*if (PhotonNetwork.IsConnected == false)
        {
            Debug.Log("lol");
            PhotonNetwork.ConnectUsingSettings(); // coonect to the photon master server
        }*/
        if (RoomName.text != null)
        {
            Debug.Log("Room name null ni hai");
            CreateRoom.onClick.AddListener(() => { CreateTheRoomAndJoin(RoomName.text); }); // create and join the room when button is pressed
        }
    }
    public void CreateTheRoomAndJoin(string RoomName)
    {
        IsConnecting = true;
        if (PhotonNetwork.IsConnected) // check if the server is already connected 
        {
            PhotonNetwork.CreateRoom(RoomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom }); // tis will create a room for us witha specific name
                                                                                                // if wants a room with random name just applu null at the place of room name
            Debug.Log("already connected hai random room join ho rha hai");
            PhotonNetwork.JoinRoom(RoomName); // used the random room for trail
                                            // have to use PhotonNetwork.JoinRoom(with overloads) instead
                                            // as we required to join the specific room
        }
        else
        {
            Debug.Log("phle se connected nahi tha ab connect hoga");
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings(); // coonect to the photon master server
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("We are now connected to " + PhotonNetwork.CloudRegion + " server:");
        PhotonNetwork.CreateRoom(RoomName.text, new RoomOptions { MaxPlayers = maxPlayersPerRoom }); // tis will create a room for us witha specific name
                                                                                                // if wants a room with random name just applu null at the place of room name
        if (IsConnecting)
        {
            PhotonNetwork.JoinRoom(RoomName.text);
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected due to : {cause} ");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // this method is for random rooms will modify later for specific room
    {
        Debug.Log("no room available, hence creating a new room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("no room available, hence creating a new room");
        PhotonNetwork.CreateRoom(RoomName.text, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(" joined in " + RoomName.text + " Room");
        int playerCount;
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log("No of players inside the room :" + playerCount);
        if(playerCount != maxPlayersPerRoom)
        {
            Debug.Log("less no of players inside the romm so waiting for other players to join");
        }
        else
        {
            Debug.Log(" all opponenets found, match is ready  to begin");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false; // this avoids new player joining the room when the room is full

            PhotonNetwork.LoadLevel("Ascending2Digit");
        }
    }
}
