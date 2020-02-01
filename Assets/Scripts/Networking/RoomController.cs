using Photon.Pun;
using UnityEngine;

public class RoomController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private int MultiplayerSceneIndex;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room");
        StartGame();
    }

    private void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(MultiplayerSceneIndex);
        }
    }
}
