using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    ZombieScript zombieScript;
    public List<GameObject> zombies;
    public List<Transform> subSpawners;
    public GameObject zombiePrefab;
    [SerializeField] private int zombieCount;
    [SerializeField] private bool roundStarted;
    public int roundCounter;

    private void Start()
    {
        zombieScript = FindObjectOfType<ZombieScript>();
        roundStarted = true;
    }
    void Update()
    {
        if (zombies.Count <= 0 && roundStarted == true)
        {
            StartCoroutine(NewRoundDelay());
        }
    }
    private void RoundStart()
    {
        roundCounter++;
        StartCoroutine(SpawnZombies());
    }
    IEnumerator SpawnZombies()
    {
        zombieCount += roundCounter;
        for (int i = 0; i < zombieCount; i++)
        {
            var randomWindow = Random.Range(0, subSpawners.Count);
            GameObject zombie = Instantiate(zombiePrefab);
            Debug.Log(randomWindow);
            zombie.transform.position = subSpawners[randomWindow].transform.position;
            zombie.name = $"Zombie {i}";
            zombies.Add(zombie);
            yield return new WaitForSeconds(1f);
        }
            roundStarted = true;
    }
    IEnumerator NewRoundDelay()
    {
        roundStarted = false;
        yield return new WaitForSeconds(2f);
        RoundStart();
    }
}
