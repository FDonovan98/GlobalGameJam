using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Objects/Create New Weapon")]
public class WeaponObject : BaseObject
{
    public float range = 100.0f;
    public float damagePerShot = 10.0f;
    public int magazineSize = 30;
    public float reloadTime = 1.0f;
    public float TimeBetweenShots = 1.0f;
    //public float ScopeTime = 1.0f;
    // public float zoomMultiplier = 2.0f;
}
