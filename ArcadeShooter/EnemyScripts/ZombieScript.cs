using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Linq;

public class ZombieScript : MonoBehaviour
{
    NavMeshAgent navMesh;
    Barricade barricades;
    PlayerWeapon playerWeapon;
    PlayerPoints playerPoints;
    ZombieSpawner zombieSpawner;

    public float zombieHealth;
    [SerializeField] private bool canDestroyBarricade;
    [SerializeField] private bool followPlayer;
    [SerializeField] private LayerMask layerMask;

    public GameObject[] allbarricades;
    public GameObject[] allPlayers;
    public GameObject nearestBarricade;
    public GameObject nearestPlayer;
    float distance;
    float nearestDistance = 9999;

    [Obsolete]
    void Start()
    {
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        barricades = FindObjectOfType<Barricade>();
        zombieSpawner = FindObjectOfType<ZombieSpawner>();
        playerWeapon = FindObjectOfType<PlayerWeapon>();
        playerPoints = FindObjectOfType<PlayerPoints>();
        navMesh = GetComponent<NavMeshAgent>();

        FindNearestBarricade();
        //zombieHealth = zombieSpawner.roundCounter * 100f;
        followPlayer = false;
        canDestroyBarricade = true;
    }
    private void FindNearestBarricade()
    {
        allbarricades = GameObject.FindGameObjectsWithTag("Barricade");
        for (int i = 0; i < allbarricades.Length; i++)
        {
            distance = Vector3.Distance(this.transform.position, allbarricades[i].transform.position);
            if (distance < nearestDistance)
            {
                nearestBarricade = allbarricades[i];
                nearestDistance = distance;
            }
        }
        navMesh.SetDestination(nearestBarricade.transform.position);
    }
    private void Update()
    {
        Death();
        if (followPlayer == false)
        {
            MoveToBarricade();
        }
        else
        {
            StartCoroutine(FindNearestPlayer());
        }
    }
    private void MoveToBarricade()
    {
        if (navMesh.remainingDistance <= 1 && barricades.planks.Count == 0)
        {
            followPlayer = true;
            nearestDistance = 9999;
        }
        else if (navMesh.remainingDistance <= 1 && barricades.planks.Count > 0 && canDestroyBarricade == true)
        {
            StartCoroutine(DestroyPlankDelay());
        }

    }
    IEnumerator FindNearestPlayer()
    {
        yield return new WaitForSeconds(4f);
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        nearestDistance = 9999;
        for (int i = 0; i < allPlayers.Length; i++)
        {
            distance = Vector3.Distance(this.transform.position, allPlayers[i].transform.position);
            if (distance < nearestDistance)
            {
                nearestPlayer = allPlayers[i];
                nearestDistance = distance;
            }
        }
        navMesh.SetDestination(nearestPlayer.transform.position);
    }
    private void Death()
    {
        if (zombieHealth <= 0)
        {
            playerPoints.playerPoints += 60;
            zombieSpawner.zombies.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    public void ZombieHit()
    {
        Debug.Log("Zombie Hit By Ray");
        Debug.Log(playerWeapon);
        playerPoints.playerPoints += 10;
        zombieHealth -= playerWeapon.bulletDamage;
    }

    IEnumerator DestroyPlankDelay()
    {
        for (int i = 0; i < barricades.planks.Count; i++)
        {
            canDestroyBarricade = false;
            barricades.DestroyBarricade();
            yield return new WaitForSeconds(3f);
            canDestroyBarricade = true;
        }
    }
}
