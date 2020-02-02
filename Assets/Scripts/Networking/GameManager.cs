using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating player");
        PhotonNetwork.Instantiate(Path.Combine("Prefabs/Networking", "TestPlayer"), Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
        // Unlocks the cursor to the centre of the screen.
        Cursor.lockState = CursorLockMode.None;
        // Shows cursor.
        Cursor.visible = true;
    }
}
