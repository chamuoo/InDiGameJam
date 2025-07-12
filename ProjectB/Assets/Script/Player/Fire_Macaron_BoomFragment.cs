using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Macaron_BoomFragment : MonoBehaviour
{
    float speed;
    float maxSpeed;
    Vector2 randDir;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] macaronSprites;
    float timer = 1f;
    void Start()
    {
        speed = Random.Range(0.15f, 1.1f);
        maxSpeed = speed;
        randDir = Random.insideUnitCircle.normalized;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (macaronSprites.Length >= 1)
            spriteRenderer.sprite = macaronSprites[Random.Range(0, macaronSprites.Length)];
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 365f));
    }
    
    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            Destroy(this.gameObject);


        transform.position = new Vector2(transform.position.x, transform.position.y) + randDir * speed * Time.deltaTime;

        if (speed > 0)
            speed -= maxSpeed * Time.deltaTime * 1f;
        else
            speed = 0;

    }
}
