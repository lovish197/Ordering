using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NetworkControllerDiffApproach : MonoBehaviourPunCallbacks
{
    bool IsConnecting;
    public TMP_InputField RoomName;
    const int maxPlayersPerRoom = 2;
    public Button CreateRoom, _JoinRoom;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public void CreateTheRoom(string RoomName)
    {
        IsConnecting = true;
        PhotonNetwork.CreateRoom(RoomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom }); // tis will create a room for us witha specific name
                                                                                                // if wants a room with random name just applu null at the place of room name
    }
    /*public void JoinRoom(string RoomName)
    {
        Debug.Log("Room manually create hua hai");
        PhotonNetwork.JoinRoom(RoomName);
    }*/
    public override void OnConnectedToMaster()
    {
        CreateRoom.onClick.AddListener(() => { CreateTheRoom(RoomName.text); }); // create the room when button is pressed
        //_JoinRoom.onClick.AddListener(() => { JoinRoom(RoomName.text); });
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected due to : {cause} ");
    }

    /*public override void OnJoinRandomFailed(short returnCode, string message) // this method is for random rooms will modify later for specific room
    {
        Debug.Log("no room available, hence creating a new room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }*/

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("no room available, hence creating a new room");
        CreateTheRoom(RoomName.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(" joined in " + PhotonNetwork.CurrentRoom.Name + " Room");
        int playerCount;
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
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
