// Author: Harry Donovan
// Based off of from https://github.com/HDonovan96/Glass-Nomad.

using UnityEngine;

using Photon.Pun;

public class PlayerAttack : MonoBehaviourPunCallbacks
{
    public WeaponObject equipedWeapon;
    
    private Camera charCamera;
    private float timeSinceLastShot;
    private float currentBulletsInMag;
    private float numberOfMags = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        charCamera = this.GetComponentInChildren<Camera>();

        // Variable initialisation.
        timeSinceLastShot = equipedWeapon.TimeBetweenShots;
        currentBulletsInMag = equipedWeapon.magazineSize;
    }

    // Update is called once per frame
    void Update()
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
                Debug.LogAssertion(hit.transform.gameObject.name);
            }
        }
    }

    private bool CanFire()
    {
        if (timeSinceLastShot > equipedWeapon.TimeBetweenShots)
        {
            if (currentBulletsInMag > 0)
            {
                return true;
            }
        }

        return false;
    }
}
