using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    EnemyHealth enemyHealth;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float speed;
    [SerializeField] private float timeToLive;
    [SerializeField] private int damage;
    private Vector3 moveVector;

    private void Start()
    {
        enemyHealth = FindObjectOfType<EnemyHealth>();
        moveVector = Vector3.forward * speed * Time.deltaTime;
        StartCoroutine(DestroyProjectile());
    }

    private void Update()
    {
        transform.Translate(moveVector);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(projectile);
        if (collision.collider.CompareTag("Enemy"))
        {
            Debug.Log("collision");
            enemyHealth.TakeDamage();
        }
    }
    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }
}
