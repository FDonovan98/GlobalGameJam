using UnityEngine;
using Photon.Pun;

using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public PlayerResource playerResource;

    // Variables inherited by above scripts.
    [Header("Health & Time")]
    public int maxHealth = 1000;
    public int startingTime = 300;

    
    public bool shouldDie = false;

    public float globalPingDelay = 300;
    private float globalTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise attached scripts.
        playerResource = new PlayerResource(this.gameObject, maxHealth, startingTime);

        Text[] allUI = this.gameObject.GetComponentsInChildren<Text>();
        foreach (Text element in allUI)
        {
            if (element.gameObject.name == "TXT_Health")
            {
                element.text = "Health: " + playerResource.currentHealth;
            }
        }

        MeshFilter[] meshFilters = this.gameObject.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter element in meshFilters)
        {
            if (element.gameObject.name == "Weapon")
            {
                element.mesh = this.GetComponent<PlayerAttack>().equipedWeapon.objectModel;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        globalTime += Time.deltaTime;

        playerResource.ChangePlayerResource(PlayerResource.Resource.Time, -Time.deltaTime);

        if (shouldDie)
        {
            PhotonNetwork.Destroy(this.gameObject);
            PhotonNetwork.LeaveRoom();
            Debug.Log("Leaving Rom");
        }

        if (Input.GetButtonDown("Ping") || globalTime > globalPingDelay)
        {
            DoRadarPing();
            if (globalTime > globalPingDelay)
            {
                globalTime = 0.0f;
            }
            else
            {
                playerResource.ChangePlayerResource(PlayerResource.Resource.Time, -60);
            }
        }

    }

    private void DoRadarPing()
    {
        Debug.Log("Radar ping");
    }
}
