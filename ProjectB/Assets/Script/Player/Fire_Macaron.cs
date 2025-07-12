using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Macaron : MonoBehaviour
{
    Vector2 targetDirection = new Vector2(1, 0);
    float speed = 3.84f;
    float nowSpeed = 0f;
    float maxSpeed = 3.84f;
    Rigidbody2D rb;
    float timer = 2f;
    SpriteRenderer spriteRenderer;
    [SerializeField] GameObject macaron_Boom;
    [SerializeField] Sprite[] macaronSprites;
    public float damage = 10f;
    bool isBoom = false;

    public void SetDirection(Vector2 dir)
    {
        targetDirection = dir;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.None;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(macaronSprites.Length >= 1)
            spriteRenderer.sprite = macaronSprites[Random.Range(0, macaronSprites.Length)];
    }

    private void Update()
    {
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);

        if (rb != null && targetDirection != null)
            rb.velocity = targetDirection * nowSpeed;

        if (timer > 0)
            timer -= Time.deltaTime;
        else
            Destroy(this.gameObject);

        if(nowSpeed + speed * Time.deltaTime < maxSpeed)
        {
            nowSpeed += speed * Time.deltaTime;
        }
        else
        {
            nowSpeed = maxSpeed;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isBoom && (collision.CompareTag("Enemy") || collision.CompareTag("Wall") || collision.CompareTag("AggroWall")))
        {
            isBoom = true;
            GameObject obj = Instantiate(macaron_Boom,transform.position, Quaternion.identity);
            obj.GetComponent<Fire_Macaron_Boom>().damage = damage;
            Destroy(this.gameObject);
        }
    }
}
