using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public HealthBar healthBar;
    public ArmorBar armorBar;

    HealthPickup healthPickUp;
    [SerializeField] private int remainingDamage;

    public int maxHealth = 50;
    public int currentHealth;

    public int maxArmor = 100;
    public int currentArmor;

    [SerializeField]public int enemyDamage = 1;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentArmor = 0;
        armorBar.SetArmor(currentArmor);
        healthPickUp = GetComponent<HealthPickup>();
    }

    // Update is called once per frame
    void Update()
    {
        Health();
        Armor();
        Cheat();
        healthBar.SetHealth(currentHealth);
        armorBar.SetArmor(currentArmor);

        //player death
        if (currentHealth <= 0)
        {
            PlayerDeath();
        }
    }
    private void Health()
    {
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    private void Armor()
    {
        if (currentArmor >= maxArmor)
        {
            currentArmor = maxArmor;
        }
    }
    public void TakeDamage(int damage)
    {
        remainingDamage = damage - currentArmor;
        if (currentArmor <= damage)
        {
            currentArmor = currentArmor - damage;
            currentHealth = currentHealth - remainingDamage;
            currentArmor = 0;

        }
        else if (currentArmor >= damage)
        {
            currentArmor = currentArmor - damage;
        }
        else
        {
            currentHealth = remainingDamage + currentHealth;
        }
    }
    private void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            maxHealth = 1000000;
            currentHealth = 1000000;
        }
    }

    public void PlayerDeath()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene("game over");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //damage van enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(enemyDamage);
        }
    }
}
