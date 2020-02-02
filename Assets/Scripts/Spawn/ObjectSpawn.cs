using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [SerializeField]
    private float SpawnDiameter;
    private float SpawnRadius;
    [SerializeField]
    private float SpawnOffset;
    private int MaxItemCount;
    private int CurrentItemCount = 0;
    [SerializeField]
    private string[] PrefabNames;

    string[] ItemType = { "Weapon", "Health", "Ammo", "Time"};

    public enum Value { Common, Uncommon, Rare}

    private List<string> items = new List<string>();

    public LayerMask Pickup;

    [SerializeField]
    private Value Rarity;

    Vector3 SquareCentre;
    void OnDrawGizmos()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, new Vector3(SpawnDiameter, 1, SpawnDiameter));
    }

    // Start is called before the first frame update
    void Start()
    {
        SquareCentre = transform.position;
        SpawnRadius = SpawnDiameter / 2;
        foreach(string s in PrefabNames)
        {
            items.Add(s);
        }

        SpawnItems();
    }

    void SpawnItems()
    {
        do
        {
            CurrentItemCount++;
            CreateObject(GetItem(), 0.0f, GetSpawnPosition());
        } while (CurrentItemCount < MaxItemCount);
    }

    string GetItem()
    {
        int index = Random.Range(0, ItemType.Length);

        return items[index];
    }

    Vector3 GetSpawnPosition()
    {
        float RandomX = Random.Range(SquareCentre.x - SpawnRadius, SquareCentre.x + SpawnRadius);
        float RandomZ = Random.Range(SquareCentre.z - SpawnRadius, SquareCentre.z + SpawnRadius);

        return new Vector3(RandomX, transform.position.y + SpawnOffset, RandomZ);
    }

    void CreateObject(string name, float value, Vector3 Pos)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs/Networking", name), Pos + (Vector3.up * 2), Quaternion.identity);
        }
    }
}
