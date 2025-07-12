using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Macaron_Boom : MonoBehaviour
{
    float timer = 0.2f;
    public int damage = 3;

    int minBoomFragment = 6;
    int maxBoomFragment = 12;
    [SerializeField] GameObject boomFragment;
    List<GameObject> hitObjects = new List<GameObject>();
    void Start()
    {
        int fragmentCount = Random.Range(minBoomFragment, maxBoomFragment);
        print(fragmentCount);
        for(int i = 0; i < fragmentCount; i++)
        {
            GameObject obj = Instantiate(boomFragment, transform.position, Quaternion.identity);
        }
    }

    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(!hitObjects.Contains(collision.gameObject))
        {
            hitObjects.Add(collision.gameObject);

            if (collision.CompareTag("Enemy"))
            {
                if (collision.GetComponent<Enemy1>() != null)
                    collision.GetComponent<Enemy1>().TakeDamage(damage);
                else if (collision.GetComponent<Enemy2>() != null)
                    collision.GetComponent<Enemy2>().TakeDamage(damage);
            }
            else if (collision.CompareTag("Wall") || collision.CompareTag("ChallengeWall"))
            {
                collision.GetComponent<Wall>().TakeDamage(damage);
            }
        }

    }
}
