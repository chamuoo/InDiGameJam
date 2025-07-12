using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2_Fire : MonoBehaviour
{
    Vector2 targetDirection;
    float speed = 1f;
    Rigidbody2D rb;
    public float timer = 5f;
    SpriteRenderer spriteRenderer;
    public int attackDamage = 10;
    bool isHeat = false;

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
        if(collision.CompareTag("Player"))
        {
            //플레이어가 대미지를 입는 함수 호출
            //투사체가 폭발하는 이펙트가 있다면 생성
            Destroy(this.gameObject);
        }
        else if(collision.CompareTag("Wall"))
        {
            //벽이 대미지를 입는 함수 호출
            //투사체가 폭발하는 이펙트가 있다면 생성
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isHeat == false)
        {
            if (collision.CompareTag("Player"))
            {
                isHeat = true;
                collision.GetComponent<PlayerController>().TakeDamage(attackDamage);
                Destroy(this.gameObject);
            }
            else if (collision.CompareTag("Wall"))
            {
                isHeat = true;
                collision.GetComponent<Wall>().TakeDamage(attackDamage);
                Destroy(this.gameObject);
            }
        }
    }
}
