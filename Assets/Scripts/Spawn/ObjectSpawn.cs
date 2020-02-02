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

    private int MaxItemCount;
    private int CurrentItemCount = 0;

    IDictionary<string, List<string>> ItemDictionary = new Dictionary<string, List<string>>();

    private List<string> WeaponNames;
    private List<string> HealthPickupNames;
    private List<string> TimePickupNames;

    [SerializeField]
    private float SpawnOffset;
    public enum Value { Common, Uncommon, Rare}

    [SerializeField]
    private Value Rarity;

    Vector3 SquareCentre;

    string AssetPath = null;

    bool SpawnAmmo = false;
    void OnDrawGizmos()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, new Vector3(SpawnDiameter, 1, SpawnDiameter));
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (Rarity)
        {
            case Value.Common:
                MaxItemCount = 1;
                break;
            case Value.Uncommon:
                MaxItemCount = Random.Range(1, 3);
                break;
            case Value.Rare:
                MaxItemCount = Random.Range(2, 4);
                break;
            default:break;
        }

        SquareCentre = transform.position;

        if(SpawnDiameter == 0.0f)
        {
            SpawnDiameter = 2.0f;
        }

        SpawnRadius = SpawnDiameter / 2;
        
        WeaponNames = new List<string>();
        HealthPickupNames = new List<string>();
        TimePickupNames = new List<string>();

        InitItemLists("Prefabs/Items/Weapons", WeaponNames);
        InitItemLists("Prefabs/Items/Health", HealthPickupNames);
        InitItemLists("Prefabs/Items/Time", TimePickupNames);

        ItemDictionary.Add(new KeyValuePair<string, List<string>>("Weapon", WeaponNames));
        ItemDictionary.Add(new KeyValuePair<string, List<string>>("Health", HealthPickupNames));
        ItemDictionary.Add(new KeyValuePair<string, List<string>>("Time", TimePickupNames));

        SpawnItems();
    }

    void InitItemLists(string path, List<string> itemlists)
    {
        GameObject[] objects = Resources.LoadAll<GameObject>(path);

        foreach (GameObject ob in objects)
        {
            itemlists.Add(ob.name);
        }
    }

    void SpawnItems()
    {
        do
        {
            CurrentItemCount++;
            CreateObject(GetItem(), GetSpawnPosition());
        } while (CurrentItemCount < MaxItemCount);
    }

    string GetItem()
    {
        float RandomChance = Random.Range(0.0f, 10.0f);
        string ItemType = null;

        if(RandomChance <= 5.0f)
        {
            ItemType = "Weapon";
            SpawnAmmo = true;
            AssetPath = "Prefabs/Items/Weapons";
            CurrentItemCount++;
        }
        else if(RandomChance > 5.0f && RandomChance <= 8.5f)
        {
            ItemType = "Health";
            AssetPath = "Prefabs/Items/Health";
        }
        else if(RandomChance > 8.5f && RandomChance <= 10.0f)
        {
            ItemType = "Time";
            AssetPath = "Prefabs/Items/Time";
        }
       

        int ItemIndex = Random.Range(0, ItemDictionary[ItemType].Count);

        return ItemDictionary[ItemType][ItemIndex];
    }

    Vector3 GetSpawnPosition()
    {
        float RandomX = Random.Range(SquareCentre.x - SpawnRadius, SquareCentre.x + SpawnRadius);
        float RandomZ = Random.Range(SquareCentre.z - SpawnRadius, SquareCentre.z + SpawnRadius);

        return new Vector3(RandomX, transform.position.y, RandomZ);
    }

    void CreateObject(string name, Vector3 Pos)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(SpawnAmmo)
            {
                PhotonNetwork.Instantiate(Path.Combine("Prefabs/Items/Weapons", "Ammo"), Pos + Vector3.right * 2, Quaternion.identity);
                SpawnAmmo = false;
            }

            PhotonNetwork.Instantiate(Path.Combine(AssetPath, name), Pos, Quaternion.identity);
        }

        Debug.Log("Spawned: " + name);
    }
}
