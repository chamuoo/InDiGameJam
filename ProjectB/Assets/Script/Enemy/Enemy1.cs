using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy1 : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb;
    GameObject aggroWall;
    GameObject targetObject;
    SpriteRenderer spriteRenderer;
    Color originalColor;
    Animator animator;

    ExpBar expbar;

    float speed = 0.64f;
    int moveAnim = 0;
    float moveCooldown = 0.5f;
    Vector2 targetDirection;

    bool isBlocked = false;
    float blockedTimer = 0.4f;
    Vector2 avoidingDirection;

    float attackCooldown = 2f;
    bool canAttack = false;
    bool isAttack = false;

    public int HP = 10;
    float takeDamageTimer = 0f;
    float dieTimer = 3f;
    bool isDie = false;

    int attackDamage = 2;

    public Image hpFillImage;
    public int currentHP;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        animator = GetComponent<Animator>();

        expbar = FindObjectOfType<ExpBar>();
        currentHP = HP;
    }

    void Move(Vector2 moveDir)
    {
        if(rb != null)
            rb.velocity = moveDir * speed;
        isAttack = true;
    }

    void Stop()
    {
        if (rb != null)
            rb.velocity = Vector2.zero;
        isAttack = false;
    }

    void Update()
    {
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);

        if (currentHP > 0)
        {
            if (aggroWall == null)
            {
                aggroWall = GameObject.FindGameObjectWithTag("ChallengeWall");
                if (aggroWall != null)
                    targetObject = aggroWall;
                else if (player != null)
                    targetObject = player;
            }

            targetDirection = (targetObject.transform.position - this.transform.position).normalized;

            if (!isBlocked && blockedTimer > 0)
                blockedTimer -= Time.deltaTime;

            if (attackCooldown > 0)
                attackCooldown -= Time.deltaTime;

            if (targetObject != null && canAttack && isAttack && attackCooldown <= 0)
            {
                if (player != null && targetObject == player)
                {
                    player.GetComponent<PlayerController>().TakeDamage(attackDamage);
                }
                else if (aggroWall != null && targetObject == aggroWall)
                {
                    aggroWall.GetComponent<Wall>().TakeDamage(attackDamage);
                }
                attackCooldown = 2f;
            }

            animator.SetFloat("Move", moveAnim);

            if(moveCooldown > 0)
            {
                moveCooldown -= Time.deltaTime;
            }    
            else
            {
                if (moveAnim == 0)
                {
                    moveAnim = 1;
                    moveCooldown = 0.3f;
                }
                else if (moveAnim == 1)
                {
                    moveAnim = 2;
                    moveCooldown = 0.3f;
                }
                else if (moveAnim == 2)
                {
                    moveAnim = 3;
                    moveCooldown = 0.3f;
                }
                else if (moveAnim == 3)
                {
                    moveAnim = 4;
                    moveCooldown = 0.3f;
                }
                else if (moveAnim == 4)
                {
                    moveAnim = 0;
                    moveCooldown = 0.5f;
                }
            }

            if (moveAnim >= 2 && moveAnim <= 3)
            {
                if (isBlocked == false)
                {
                    Move(targetDirection);
                }
                else
                {
                    Move(avoidingDirection);
                }
            }
            else
            {
                Stop();
            }

            if(isBlocked == false)
            {
                if (Mathf.Abs(targetDirection.x) > 0.01f)
                {
                    if (targetDirection.x >= 0)
                        this.transform.localScale = new Vector3(0.32f, 0.32f, 0.32f);
                    else
                        this.transform.localScale = new Vector3(-0.32f, 0.32f, 0.32f);
                }
            }
            else
            {
                if (Mathf.Abs(targetDirection.x) > 0.01f)
                {
                    if (avoidingDirection.x >= 0)
                        this.transform.localScale = new Vector3(0.32f, 0.32f, 0.32f);
                    else
                        this.transform.localScale = new Vector3(-0.32f, 0.32f, 0.32f);
                }
            }

            if (takeDamageTimer > 0)
            {
                spriteRenderer.color = new Color(0.8f, 0f, 0f, 1f);
                takeDamageTimer -= Time.deltaTime;
            }
            else if (spriteRenderer.color != originalColor)
            {
                spriteRenderer.color = originalColor;
            }
        }
        else
        {
            Stop();
            spriteRenderer.color = originalColor;
            if (!isDie)
            {
                isDie = true;
                //소리 재생 함수 필요
                animator.SetBool("Die", true);

                SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[8]);
                expbar.AddExp(1);
            }    
            if(dieTimer > 0)
            {
                dieTimer -= Time.deltaTime;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        //소리 재생 함수필요
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[9]);
        takeDamageTimer = 0.5f;
        float fillAmount = (float)currentHP / HP;
        hpFillImage.fillAmount = fillAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == targetObject)
        {
            canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == targetObject)
        {
            canAttack = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBlocked)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.collider.gameObject.CompareTag("Wall"))
                {
                    isBlocked = true;
                    if(blockedTimer <= 0)
                    {
                        if (Mathf.Abs(contact.normal.x) > 0.75)
                        {
                            if (targetDirection.y > 0)
                                avoidingDirection = Vector2.up;
                            else
                                avoidingDirection = Vector2.down;
                        }
                        else if (Mathf.Abs(contact.normal.y) > 0.75)
                        {
                            if (targetDirection.x > 0)
                                avoidingDirection = Vector2.right;
                            else
                                avoidingDirection = Vector2.left;
                        }
                    }
                    blockedTimer = 0.4f;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isBlocked)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.collider.gameObject.CompareTag("Wall"))
                {
                    isBlocked = true;
                    if (blockedTimer <= 0)
                    {
                        if (Mathf.Abs(contact.normal.x) > 0.75)
                        {
                            if (targetDirection.y > 0)
                                avoidingDirection = Vector2.up;
                            else
                                avoidingDirection = Vector2.down;
                        }
                        else if (Mathf.Abs(contact.normal.y) > 0.75)
                        {
                            if (targetDirection.x > 0)
                                avoidingDirection = Vector2.right;
                            else
                                avoidingDirection = Vector2.left;
                        }
                    }
                    blockedTimer = 0.4f;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isBlocked == true && collision.gameObject.CompareTag("Wall"))
            isBlocked = false;
    }

}

