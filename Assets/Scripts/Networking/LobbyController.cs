using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject StartSearchButton; //Button for creating/joining games
    [SerializeField]
    private GameObject CancelSearchButton; //Button used to stop searching for game
    [SerializeField]
    private GameObject QuitGameButton;
    [SerializeField]
    private int MaxLobbySize; //Max players per lobby

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.AutomaticallySyncScene = true;
        StartSearchButton.SetActive(true);
        QuitGameButton.SetActive(true);
    }

    public void JoinLobby()
    {
        StartSearchButton.SetActive(false);
        CancelSearchButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom(); //Try to join existing room
        Debug.Log("Searching for game");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Failed to join random lobby");
        CreateLobby();
    }

    void CreateLobby()
    {
        Debug.Log("Creating lobby");
        int RandomRoomNumber = Random.Range(0, 10000);
        RoomOptions LobbyOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MaxLobbySize };
        PhotonNetwork.CreateRoom("Lobby" + RandomRoomNumber, LobbyOptions);
        Debug.Log("Successfully created Lobby" + RandomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Failed to create room. Trying again..");
        CreateLobby();
    }

    public void CancelGameSearch()
    {
        CancelSearchButton.SetActive(false);
        StartSearchButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public void QuitGame()
    {
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        Application.Quit();
    }
}
