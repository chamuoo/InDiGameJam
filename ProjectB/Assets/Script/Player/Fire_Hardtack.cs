using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Fire_Hardtack : MonoBehaviour
{
    public Vector2 targetDirection;
    float speed = 3.84f;
    Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    public float timer = 1f;
    public int damage = 3;
    bool isHit = false;

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
        print("start");
        rb = GetComponent<Rigidbody2D>();
        SetDirection();
    }

    private void Update()
    {

        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);

        if (rb != null && targetDirection != null)
        {
            print($"작동중, 방향은 {targetDirection}");
            rb.velocity = targetDirection * speed;
        }

        if (timer > 0)
            timer -= Time.deltaTime;
        else
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit == false)
        {
            if (collision.CompareTag("Enemy"))
            {
                isHit = true;
                if (collision.GetComponent<Enemy1>() != null)
                    collision.GetComponent<Enemy1>().TakeDamage(damage);
                else if (collision.GetComponent<Enemy2>() != null)
                    collision.GetComponent<Enemy2>().TakeDamage(damage);
                //투사체가 폭발하는 이펙트가 있다면 생성
                Destroy(this.gameObject);
            }
            else if (collision.CompareTag("Wall") || collision.CompareTag("ChallengeWall"))
            {
                isHit = true;
                collision.GetComponent<Wall>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isHit == false)
        {
            if (collision.CompareTag("Enemy"))
            {
                isHit = true;
                if (collision.GetComponent<Enemy1>() != null)
                    collision.GetComponent<Enemy1>().TakeDamage(damage);
                else if (collision.GetComponent<Enemy2>() != null)
                    collision.GetComponent<Enemy2>().TakeDamage(damage);
                //투사체가 폭발하는 이펙트가 있다면 생성
                Destroy(this.gameObject);
            }
            else if (collision.CompareTag("Wall") || collision.CompareTag("ChallengeWall"))
            {
                isHit = true;
                collision.GetComponent<Wall>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
