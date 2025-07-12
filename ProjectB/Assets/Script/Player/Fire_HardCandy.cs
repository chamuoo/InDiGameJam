using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fire_HardCandy : MonoBehaviour
{
    Vector2 targetDirection;
    float speed = 4.8f;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    public float timer = 1f;
    public int damage = 10;
    [SerializeField] Sprite[] hardCandySprites;

    public void SetDirection()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        targetDirection = (mousePos - (Vector2)(transform.position)).normalized;
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
        if (hardCandySprites.Length >= 1)
        {
            spriteRenderer.sprite = hardCandySprites[Random.Range(0, hardCandySprites.Length)];
        }
        SetDirection();
    }

    private void Update()
    {
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);

        if (rb != null && targetDirection != null)
            rb.velocity = targetDirection * speed;

        if (timer > 0)
            timer -= Time.deltaTime;
        else
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<Enemy1>() != null)
                collision.GetComponent<Enemy1>().TakeDamage(damage);
            else if (collision.GetComponent<Enemy2>() != null)
                collision.GetComponent<Enemy2>().TakeDamage(damage);
            //투사체가 폭발하는 이펙트가 있다면 생성
            Destroy(this.gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            //벽이 대미지를 입는 함수 호출
            //투사체가 폭발하는 이펙트가 있다면 생성
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //적이 대미지를 입는 함수 호출
            
            //투사체가 폭발하는 이펙트가 있다면 생성
            Destroy(this.gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            //벽이 대미지를 입는 함수 호출
            collision.GetComponent<Wall>().TakeDamage(damage);
            //투사체가 폭발하는 이펙트가 있다면 생성
            Destroy(this.gameObject);
        }
        else if (collision.CompareTag("AggroWall"))
        {
            //벽이 대미지를 입는 함수 호출
            collision.GetComponent<Wall>().TakeDamage(damage);
            //투사체가 폭발하는 이펙트가 있다면 생성
        }
    }
}
