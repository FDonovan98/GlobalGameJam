// Author: Harry Donovan
// Based off of from https://github.com/HDonovan96/Glass-Nomad.

using UnityEngine;

using Photon.Pun;

public class PlayerAttack : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public WeaponObject equipedWeapon;
    
    private Camera charCamera;
    private float timeSinceLastShot;
    private float currentBulletsInMag;
    private float numberOfMags = 2;
    
    // Start is called before the first frame update
    public void Start()
    {
        charCamera = this.GetComponentInChildren<Camera>();

        // Variable initialisation.
        timeSinceLastShot = equipedWeapon.TimeBetweenShots;
        currentBulletsInMag = equipedWeapon.magazineSize;
    }

    // Update is called once per frame
    public void Update()
    {
        // Aborts the script if the GameObject doesn't belong to the client.
        if (!photonView.IsMine)
        {
            return;
        }

        timeSinceLastShot += Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        if (CanFire())
        {
            RaycastHit hit;
            if (Physics.Raycast(charCamera.transform.position, charCamera.transform.forward, out hit, equipedWeapon.range))
            {
                photonView.RPC("Shoot", RpcTarget.All, charCamera.transform.position, charCamera.transform.forward, equipedWeapon.range, equipedWeapon.damagePerShot);
            }
        }
    }

    // Checks if the weapon is ready to fire and reduces ammo.
    // Also resets the time since last shot.
    private bool CanFire()
    {
        if (timeSinceLastShot > equipedWeapon.TimeBetweenShots)
        {
            if (currentBulletsInMag > 0)
            {
                timeSinceLastShot = 0.0f;
                currentBulletsInMag -= 1;
                return true;
            }
        }

        return false;
    }

    [PunRPC]
    public void Shoot(Vector3 cameraPos, Vector3 cameraForward, float weaponRange, int weaponDamage)
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPos, cameraForward, out hit, weaponRange))
        {
            Debug.Log(hit.transform.gameObject.name + " has been hit");
            if (hit.transform.gameObject.tag == "Player")
            {
                hit.transform.GetComponent<PlayerController>().playerResource.ChangePlayerResource(PlayerResource.Resource.Health, weaponDamage);
            }
            
        }
    }
}
