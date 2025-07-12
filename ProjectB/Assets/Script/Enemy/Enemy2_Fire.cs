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
    public int attackDamage = 2;
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
        if (isHeat == false)
        {
            if (collision.CompareTag("Player"))
            {
                isHeat = true;
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(attackDamage);
                Destroy(this.gameObject);
            }
            else if (collision.CompareTag("Wall") || collision.CompareTag("ChallengeWall"))
            {
                isHeat = true;
                collision.GetComponent<Wall>().TakeDamage(attackDamage);
                Destroy(this.gameObject);
            }
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
            else if (collision.CompareTag("Wall") || collision.CompareTag("ChallengeWall"))
            {
                isHeat = true;
                collision.GetComponent<Wall>().TakeDamage(attackDamage);
                Destroy(this.gameObject);
            }
        }
    }
}
