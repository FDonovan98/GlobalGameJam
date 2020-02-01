using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public PlayerHealth playerHealth;

    // Variables inherited by above scripts.
    [Header("Health")]
    public int maxHealth = 1000;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise attached scripts.
        playerHealth = new PlayerHealth(this.gameObject, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
