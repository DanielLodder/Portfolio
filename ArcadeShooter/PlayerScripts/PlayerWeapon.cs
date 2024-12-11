using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerWeapon : MonoBehaviour
{
    private enum Weapon
    {
        None = 0,
        SemiAuto,
        FullAuto,
        Burst,
        Shotgun,
    }
    [Header("Weapon Stats")]
    [SerializeField] private Weapon weapon = Weapon.None;
    [SerializeField] public int currentBullets, maxBullets, reserveBullets, maxReserveBullets, burstBullets, pallets;
    [SerializeField] private float rateOfFire;
    [SerializeField] private bool canFire = true;
    [SerializeField] private float reloadSpeed;
    public float bulletDamage;

    Camera playerCamera;
    RaycastHit hitInfo;

    private void Start()
    {
        currentBullets = maxBullets;
        playerCamera = GetComponentInParent<Camera>();
    }
    public void FireWeapon()
    {
        if (currentBullets <= 0)
        {
            canFire = false;
        }
        if (canFire)
        {
            switch (weapon)
            {
                case Weapon.None:
                    Debug.LogWarning("No weapon selected");
                    break;
                case Weapon.SemiAuto:
                    StartCoroutine(SemiAutoDelay());
                    break;
                case Weapon.FullAuto:
                    StartCoroutine(FullAutoDelay());
                    break;
                case Weapon.Burst:
                    StartCoroutine(BurstDelay());
                    break;
                case Weapon.Shotgun:
                    StartCoroutine(ShotgunDelay());
                    break;
            }
        }
    }
    public void ReloadWeapon()
    {
        if (currentBullets != maxBullets && reserveBullets != 0)
        {
            StartCoroutine(Reload());
        }
    }
    IEnumerator SemiAutoDelay()
    {
        canFire = false;
        Debug.Log("Fired semiAuto");
        Fire();
        currentBullets--;
        yield return new WaitForSeconds(rateOfFire);
        canFire = true;
    }
    IEnumerator BurstDelay()
    {
        for (int i = 1; i <= burstBullets; i++)
        {
            canFire = false;
            Fire();
            currentBullets--;
            Debug.Log("fired burst");
            yield return new WaitForSeconds(rateOfFire);
            canFire = true;
        }
    }
    IEnumerator FullAutoDelay()
    {
        while (Input.GetKey(KeyCode.Mouse0) && currentBullets != 0)
        {
            canFire = false;
            Fire();
            currentBullets--;
            Debug.Log("fired fullAuto");
            yield return new WaitForSeconds(rateOfFire);
            canFire = true;
        }
    }
    IEnumerator ShotgunDelay()
    {
        for (int i = 1; i <= pallets; i++)
        {
            canFire = false;
            Fire();
            Debug.Log(hitInfo);
            yield return new WaitForSeconds(rateOfFire);
            canFire = true;
        }
        currentBullets--;
    }
    private void Fire()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitInfo, 100f))
        {
            if (hitInfo.collider.CompareTag("Zombie"))
            {
                hitInfo.transform.SendMessage("ZombieHit");
            }
            else
            {
                Debug.Log("Missed");
            }
        }
    }
    IEnumerator Reload()
    {
        Debug.Log("reloaded");
        canFire = false;
        reserveBullets += currentBullets;
        currentBullets = 0;
        yield return new WaitForSeconds(reloadSpeed);
        if (reserveBullets >= maxBullets)
        {
            currentBullets = maxBullets;
            reserveBullets -= maxBullets;
        }
        else
        {
            currentBullets = reserveBullets;
            reserveBullets = 0;
        }
        canFire = true;
    }
}