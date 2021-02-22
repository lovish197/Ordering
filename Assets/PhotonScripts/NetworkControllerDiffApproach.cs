using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class NetworkControllerDiffApproach : MonoBehaviourPunCallbacks
{
    public Text Status_text;
    public TMP_InputField RoomName;
    const int maxPlayersPerRoom = 2;
    public Button StartGame;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        StartGame.transform.gameObject.SetActive(false);
        Status("Connecting to the server");
    }

    private void Status(string msg)
    {
        Debug.Log(msg);
        Status_text.text = msg;
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.AutomaticallySyncScene = true;
        StartGame.transform.gameObject.SetActive(true);

        Status("Connected to " + PhotonNetwork.ServerAddress);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected due to : {cause} ");
    }
    public void On_start_Click()
    {
        string roomname = RoomName.text;
        Debug.Log(roomname);

        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = maxPlayersPerRoom;

        PhotonNetwork.JoinOrCreateRoom(roomname, options, TypedLobby.Default);
        StartGame.transform.gameObject.SetActive(false);
        Status("Joining " + roomname);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Status("no room available");
    }

    public override void OnJoinedRoom()
    {
        int playerCount;
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if(playerCount != maxPlayersPerRoom)
        {
            Status("less no of players inside the romm so waiting for other players to join");
        }
        else
        {
            Status("All opponents found game is starting");
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
