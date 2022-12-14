using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyScript : MonoBehaviourPunCallbacks
{
    public static LobbyScript Instance;

    [Header("Login Panel")]
    public GameObject LoginPanel;

    public TMP_InputField PlayerNameInput;

    [Header("Selection Panel")]
    public GameObject SelectionPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;

    public TMP_InputField RoomNameInputField;

    [Header("Loading Panel")]
    public GameObject LoadingPanel;

    [Header("Rooms Panel")]
    public GameObject RoomsPanel;
    public GameObject RoomListItemPrefab;
    public Transform RoomListContent;

    [Header("Room Panel")]
    public GameObject RoomPanel;
    public GameObject PlayerListItemPrefab;
    public Transform PlayerListContent;
    public Button StartGameButton;
    public TMP_Text RoomNameText;

    public void Awake()
    {
        Instance = this;
        PhotonNetwork.AutomaticallySyncScene = true; //This will synchronize all players states
    }

    public override void OnConnectedToMaster()
    {
        this.SetActivePanel(SelectionPanel.name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform t in RoomListContent)
        {
            Destroy(t.gameObject);
        }
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                continue;
            }
            Instantiate(RoomListItemPrefab, RoomListContent)
                .GetComponent<RoomListItem>()
                .SetUp(info);
        }
    }

    public void JoinRoom(RoomInfo info)
    {
        SetActivePanel(LoadingPanel.name);
        PhotonNetwork.JoinRoom(info.Name); // Here the client will tell server to join them in to room with specific name
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(1, 100);

        RoomOptions options = new RoomOptions { MaxPlayers = 8 };

        PhotonNetwork.CreateRoom(roomName, options, null); // Here client create new room in to the server
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, PlayerListContent)
            .GetComponent<PlayerListItem>()
            .SetUp(newPlayer);
    }

    public override void OnJoinedRoom()
    {
        RoomNameText.text = PhotonNetwork.CurrentRoom.Name; // Here Client ask name of the current room
        SetActivePanel(RoomPanel.name);
        foreach (Transform t in PlayerListContent)
        {
            Destroy(t.gameObject);
        }

        foreach (Player p in PhotonNetwork.PlayerList) // Client ask who else are in same room with  them
        {
            Instantiate(PlayerListItemPrefab, PlayerListContent)
                .GetComponent<PlayerListItem>()
                .SetUp(p);
        }
    }

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby) // Check if users state is "in-lobby"
        {
            PhotonNetwork.LeaveLobby();
        }

        SetActivePanel(SelectionPanel.name);
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = RoomNameInputField.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1, 100) : roomName;

        RoomOptions options = new RoomOptions { MaxPlayers = 8, PlayerTtl = 10000 };

        PhotonNetwork.CreateRoom(roomName, options, null); // Client creates room to the server
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        SetActivePanel(LoadingPanel.name);

        PhotonNetwork.JoinRandomRoom(); //Client joining to random room in the server
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
        SetActivePanel(SelectionPanel.name);
    }

    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;

        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName; // Client set their name to the server
            PhotonNetwork.ConnectUsingSettings(); // Client connects to the server
        }
        else
        {
            Debug.LogError("Player Name is invalid.");
        }
    }

    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby(); // Client join to the lobby in case they are not joined already
        }

        SetActivePanel(RoomsPanel.name);
    }

    public void OnStartGameButtonClicked()
    {
        
        PhotonNetwork.CurrentRoom.IsOpen = false; // Client tell server to prevent others to join to the room
        PhotonNetwork.CurrentRoom.IsVisible = false; // Client tell server to prevent others to see the room

        PhotonNetwork.LoadLevel("SampleScene"); // Client tells game server to start the game.
    }

    public void OnCreateNewButtonClicked()
    {
        SetActivePanel(CreateRoomPanel.name);
    }

    private void SetActivePanel(string activePanel)
    {
        LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
        SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
        CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
        LoadingPanel.SetActive(activePanel.Equals(LoadingPanel.name));
        RoomsPanel.SetActive(activePanel.Equals(RoomsPanel.name)); // UI should call OnRoomListButtonClicked() to activate this
        RoomPanel.SetActive(activePanel.Equals(RoomPanel.name));
    }
}
