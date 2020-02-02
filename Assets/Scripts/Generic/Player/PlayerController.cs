﻿using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public PlayerResource playerResource;

    // Variables inherited by above scripts.
    [Header("Health & Time")]
    public int maxHealth = 1000;
    public int startingTime = 300;

    
    public bool shouldDie = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise attached scripts.
        playerResource = new PlayerResource(this.gameObject, maxHealth, startingTime);
    }

    // Update is called once per frame
    void Update()
    {
        playerResource.ChangePlayerResource(PlayerResource.Resource.Time, -Time.deltaTime);

        if (shouldDie)
        {
            PhotonNetwork.Destroy(this.gameObject);
            PhotonNetwork.LeaveRoom();
            Debug.Log("Leaving Rom");
        }
    }
}
