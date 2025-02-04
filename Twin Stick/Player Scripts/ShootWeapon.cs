using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootWeapon : MonoBehaviour
{
    private PlayerAnim playerAnim;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] public int maxAmmo;
    [SerializeField] public int currentAmmo;
    [SerializeField] public int reserveAmmo;
    [SerializeField] public int maxReserveAmmo = 100;

    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float reloadTime;

    [SerializeField] private bool canShoot;

    private void Start()
    {
        playerAnim = FindObjectOfType<PlayerAnim>();
        reserveAmmo = maxAmmo * 3;
        currentAmmo = maxAmmo;
        canShoot = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R) && reserveAmmo != 0)
        {
            Reload();
        }
        if (currentAmmo == 0)
        {
            canShoot = false;
        }
        Cheat();
    }
    private void Shoot()
    {
        canShoot = false;
        currentAmmo -= 1;
        Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        StartCoroutine(ShotCooldown());
    }
    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }
    public void Reload()
    {
        playerAnim.animations = PlayerAnimations.reloading;
        StartCoroutine(Reloading());
    }
    [SerializeField] private int reloadedAmmo;
    IEnumerator Reloading()
    {
        canShoot = false;
        reloadedAmmo = maxAmmo - currentAmmo;
        if (reserveAmmo <= reloadedAmmo)
        {
            currentAmmo = reserveAmmo + currentAmmo;
            reserveAmmo = 0;
        }
        else
        {
            reserveAmmo = reserveAmmo - reloadedAmmo;
            currentAmmo = reloadedAmmo + currentAmmo;
        }
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
        
    }

    private void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
        maxAmmo = 100000000;
        currentAmmo = 100000000;
        }
    }
}
