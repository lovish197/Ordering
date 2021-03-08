using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class NetworkControllerDiffApproach : MonoBehaviourPunCallbacks
{
    public static NetworkControllerDiffApproach instance;
    public Text Status_text;
    public Text roomName_Text;
    string username;
    public TMP_InputField RoomName;
    const int maxPlayersPerRoom = 2;
    public Button StartGame, useRoomCode;
    bool connectedToServer, flagToggle;
    string roomID;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        username = login.instance.userNameInput.text; // get the user name of the player entering the room
        PhotonNetwork.ConnectUsingSettings();
        StartGame.transform.gameObject.SetActive(false);
        useRoomCode.transform.gameObject.SetActive(false);
        Status("Connecting to the server");
    }
    private void Update()
    {
        /*
         * to make sure that input field ka text is equal to the generated roomname
         * and also the game is connected to the master server
         */
        if (!flagToggle)
        {
            if (ClientCategorySelector() == "host")
            {
                if (connectedToServer && RoomName.text == roomID)
                {
                    flagToggle = true;
                    StartGame.transform.gameObject.SetActive(true);
                }
            }
            else
            {
                if (connectedToServer && RoomName.text != null)
                {
                    flagToggle = true;
                    StartGame.transform.gameObject.SetActive(true);
                }
            }
        }
    }
    private void Status(string msg)
    {
        Debug.Log(msg);
        Status_text.text = msg;
    }

    public override void OnConnectedToMaster() // when the server got connected to the master
    {
        base.OnConnectedToMaster();
        PhotonNetwork.AutomaticallySyncScene = true;

        if (ClientCategorySelector() == "host") // things that should be visible to the user only
        {
            useRoomCode.transform.gameObject.SetActive(true);
            roomName_Text.text = "searching for suitable room id";
            RoomNameGenerator();
        }
        connectedToServer = true;
        Status("Connected to " + PhotonNetwork.ServerAddress);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected due to : {cause} ");
    }

    public void On_start_Click()
    {
        string roomname = RoomName.text;
        StartCoroutine(RoomTableCreator(username, roomname, ClientCategorySelector())); // create a tables of the player entering the room


        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = maxPlayersPerRoom;

        PhotonNetwork.JoinOrCreateRoom(roomname, options, TypedLobby.Default);
        StartGame.transform.gameObject.SetActive(false);
        Status("Joining " + roomname);

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        Debug.Log("Inside list update wala method");
    }

    private string ClientCategorySelector()
    {
        string category;
        if(username == "lavi")
        {
            category = "host";
        }
        else
        {
            category = "client";
        }
        return category;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Status("no room available");
    }

    public override void OnJoinedRoom()
    {
        int playerCount;
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int requiredPlayer = maxPlayersPerRoom - playerCount;
        StartGame.transform.gameObject.SetActive(false); // deactivate the button after joining is successful
        if(playerCount != maxPlayersPerRoom)
        {
            Status("less no of players inside the romm so waiting for "+ requiredPlayer + " more player to join");
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
    public IEnumerator RoomTableCreator(string userName, string RoomId, string Category)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", userName);
        form.AddField("roomid", RoomId);
        form.AddField("category", Category);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/orderinggame/RoomTableUpdate.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    IEnumerator RoomIdsRetiever(string generatedRoomId)
    {
        WWWForm form = new WWWForm();
        form.AddField("generatedRoomID", generatedRoomId);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/OrderingGame/checkForRoomId.php", form))
        {
            yield return www.Send();

            if(www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                if (www.downloadHandler.text == "0")
                {
                    RoomNameGenerator();
                }
                else
                {
                    roomID = generatedRoomId;
                    roomName_Text.text = "Available Room Id is " + roomID;
                }
            }
        }
    }
    void RoomNameGenerator()
    {
        int roomNo = UnityEngine.Random.Range(11111, 99999);
        string roomString = roomNo.ToString();
        StartCoroutine(RoomIdsRetiever(roomString));
    }
    public void useRoomId()
    {
        RoomName.text = roomID;
    }
}
