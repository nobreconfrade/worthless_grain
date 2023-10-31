using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerCatSpawner : MonoBehaviour
{
    public GameObject customerCat;
    public float originalSpawnRate = 10f;
    public int spawnRateChance = 3;
    private float timer = 0;
    private float spawnRate;
    void Start()
    {
        spawnRate = originalSpawnRate;
        if (gameObject.name == "CustomerSpawner 1")
        {
            timer = spawnRate - 2f;
        }
    }
    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        } else {
            SpawCustomerCat();
            timer = 0;
            spawnRate = originalSpawnRate + Random.Range(-spawnRateChance, spawnRateChance);
        }
    }

    void SpawCustomerCat()
    {
        Vector3 customerCatPosition = new(transform.position.x, transform.position.y, 0);
        Instantiate(customerCat, customerCatPosition, Quaternion.identity);
    }
}
