using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupManager : MonoBehaviour
{

    private PlayerSpawn SpawnPlayer;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer = GetComponent<PlayerSpawn>();
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating player");
        Transform trans = SpawnPlayer.findSuitableSpawn();
        PhotonNetwork.Instantiate(Path.Combine("Prefabs/Networking", "TestPlayer"), trans.position + (Vector3.up * 2), trans.rotation);
    }
}
