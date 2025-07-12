using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool isWave = false;
    public int spawnableEnemy1 = 0;
    public int spawnableEnemy2 = 0;

    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;

    float canSpawnTimer = 3f;
    float spawnCooldown = 1f;


    void Update()
    {
        if (isWave && canSpawnTimer <= 0 && (spawnableEnemy1 >= 1 || spawnableEnemy2 >= 1) && spawnCooldown <= 0)
        {
            int spawnWho = 0;

            if (spawnableEnemy1 <= 0 && spawnableEnemy2 >= 1)
            {
                spawnWho = 2;
            }
            else if (spawnableEnemy1 >= 1 && spawnableEnemy2 <= 0)
            {
                spawnWho = 1;
            }
            else if (spawnableEnemy1 >= 1 && spawnableEnemy2 >= 1)
            {
                spawnWho = Random.Range(1, 3);
            }

            if (spawnWho == 1)
            {
                GameObject obj = Instantiate(enemy1, transform.position, Quaternion.identity);
                LevelManager.Instance.enemys.Add(obj);
                spawnableEnemy1--;
                spawnCooldown = 1f;
            }
            if (spawnWho == 2)
            {
                GameObject obj = Instantiate(enemy2, transform.position, Quaternion.identity);
                LevelManager.Instance.enemys.Add(obj);
                spawnableEnemy2--;
                spawnCooldown = 1f;
            }

        }
        else if (isWave && spawnableEnemy1 <= 0 && spawnableEnemy2 <= 0)
            isWave = false;
      

        if (spawnCooldown > 0)
            spawnCooldown -= Time.deltaTime;

        if (canSpawnTimer > 0)
            canSpawnTimer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            canSpawnTimer = 3f;
        }
    }

}
