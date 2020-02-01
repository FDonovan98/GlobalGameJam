using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class LobbyController : MonoBehaviourPunCallbacks
{
    private PhotonView _PhotonView;

    [SerializeField]
    private int GameSceneIndex;
    [SerializeField]
    private int MenuSceneIndex;

    private int PlayerCount;
    private int LobbySize;

    [SerializeField]
    private float MaxWaitTime;

    private float Countdown;
    [SerializeField]
    private Text PlayerCountDisplay;
    [SerializeField]
    private Text CountdownDisplay;

    bool ReadyToStart = false;

    bool GameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        _PhotonView = GetComponent<PhotonView>();
        Countdown = MaxWaitTime;

        PlayerCountUpdate();
    }

    // Update is called once per frame
    void PlayerCountUpdate()
    {
        PlayerCount = PhotonNetwork.PlayerList.Length;
        LobbySize = PhotonNetwork.CurrentRoom.MaxPlayers;

        PlayerCountDisplay.text = PlayerCount + " / " + LobbySize;

        if(PlayerCount > 1)
        {
            ReadyToStart = true;
        }
        else
        {
            ReadyToStart = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        PlayerCountUpdate();

        if(PhotonNetwork.IsMasterClient)
        {
            _PhotonView.RPC("RPC_SendTimer", RpcTarget.Others, Countdown);
        }
    }

    [PunRPC]
    private void RPC_SendTimer(float time)
    {
        Countdown = time;

        if(time < MaxWaitTime)
        {
            MaxWaitTime = time;
        }
    }

    public void QuitToMenu()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(MenuSceneIndex);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        PlayerCountUpdate();
    }

    private void Update()
    {
        if(ReadyToStart)
        {
            Countdown -= Time.deltaTime;
            string timer = string.Format("{0:00}", Countdown);
            CountdownDisplay.text = timer;

            if(Countdown <= 0)
            {
                if (!GameStarted)
                {
                    StartGame();
                }
                else
                {
                    return;
                }
            }
        }
    }

    private void StartGame()
    {
        GameStarted = true;
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(GameSceneIndex);
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        else
        {
            return;
        }
    }
}
