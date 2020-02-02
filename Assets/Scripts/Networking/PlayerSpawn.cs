using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private PlayerPoint[] PossibleSpawnPoints;
    float Radius = 0f;

    [SerializeField]
    private LayerMask Player;

    // Start is called before the first frame update
    void Awake()
    {
        PossibleSpawnPoints = GameObject.FindObjectsOfType<PlayerPoint>();
    }



    public Transform findSuitableSpawn()
    {
        int SelectPoint = Random.Range(0, PossibleSpawnPoints.Length);
        
        if (!Physics.CheckSphere(PossibleSpawnPoints[SelectPoint].transform.position, Radius, Player))
        {
            Debug.Log("Empty point found");
            return PossibleSpawnPoints[SelectPoint].transform;
        }
        else
        {
            Debug.Log("Player already at pos");
            return findSuitableSpawn();
        }

    }

}

