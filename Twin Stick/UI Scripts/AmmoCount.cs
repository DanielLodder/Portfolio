using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCount : MonoBehaviour
{
    ShootWeapon shootWeapon;

    public Text displayMagAmmo;
    public Text displayReserveAmmo;
    private void Start()
    {
        shootWeapon = FindObjectOfType<ShootWeapon>();
    }
    void Update()
    {
        displayReserveAmmo.text = shootWeapon.reserveAmmo.ToString();
        displayMagAmmo.text = shootWeapon.currentAmmo.ToString();
    }
}
