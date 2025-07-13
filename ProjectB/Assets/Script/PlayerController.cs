using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // �Է� �׼�
    PlayerInput playerInput;

    [SerializeField] Tilemap tilemap;

    // �ִϸ��̼�
    public Animator anim { get; private set; }

    Inventory inventory;

    Vector2 moveInput;  // �Է°�

    public List<GameObject> wallPrefab; //{ get; [SerializeField] set; }

    // ���߿� GameManager�� ������Ʈ�� �߰��� �ϰ� ������ �� ����. 
    public int _point; // { get; [SerializeField] set; }
    int hp = 10;
    int MaxPoint = 50;

    public Image hpFillImage;
    public int currentHP;

    [SerializeField]Rigidbody2D rigidbody;

    public bool isDie { get; private set; } = false;

    public Vector2 targetPos { get; private set; }  // �÷��̾� ���⺤��

    private readonly List<Vector2Int> disallowedAbsoluteTileCoords = new List<Vector2Int>
{
        new Vector2Int(0, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, -1)
};

    float movespeed = 1f;

    BreadPointBar breadpointbar;

    public AudioSource footstepAudioSourse01;
    public AudioSource footstepAudioSourse02;
    private bool switchStepSound = false;

    [SerializeField] private GameObject DiePanel;

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        inventory = GameObject.Find("WallIcon").GetComponent<Inventory>();

        wallPrefab = Resources.LoadAll<GameObject>("Walls").ToList();
        rigidbody = GetComponent<Rigidbody2D>();

        breadpointbar = FindObjectOfType<BreadPointBar>();

        currentHP = hp;
    }

    private void Update()
    {
        if (rigidbody.velocity != Vector2.zero)
            rigidbody.velocity = Vector2.zero;

        // ���ο� InputSystem�� ���콺 ��ġ ��������
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 playerPos = transform.position;

        Vector2 basePos = mousePos - playerPos;
        // Ÿ�ϱ��� �÷��̾��� ��ġ
        Vector3Int playerCell = tilemap.WorldToCell(playerPos);

        // Target tile for wall placement (based on player's position + small offset)
        Vector3Int targetCell = tilemap.WorldToCell((Vector3)playerPos + Vector3.down * 0.2f); // Using the existing logic for baseCell

        // World center of the target tile
        Vector3 centerPos = tilemap.GetCellCenterWorld(targetCell);

        targetPos = basePos.normalized;

        Vector2 movement = new Vector2(moveInput.x, moveInput.y);
        
        // �밢�� �̵� �ӵ� ����
        if(movement.magnitude > 1)
        {
            movement.Normalize();
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (switchStepSound && !footstepAudioSourse02.isPlaying)
            {
                switchStepSound = !switchStepSound;
                footstepAudioSourse01.Play();


            }
            else if (!switchStepSound && !footstepAudioSourse01.isPlaying)
            {
                switchStepSound = !switchStepSound;
                footstepAudioSourse02.Play();
            }
        }

        transform.Translate(movement * movespeed * Time.deltaTime);

        // ȭ�� �� �̵� ����
        LimitToCameraBounds();

        // �� ����
        if(Keyboard.current.spaceKey.wasPressedThisFrame && LevelManager.Instance.isWave == false)
        {
            print("�� ��ġ �õ� Ÿ��: " + targetCell);

            Vector2Int targetCell2D = new Vector2Int(targetCell.x, targetCell.y);

            if(disallowedAbsoluteTileCoords.Contains(targetCell2D))
            {
                Debug.Log($"�� ��ġ �Ұ�: Ÿ�ϸ��� ���� ��ǥ ({targetCell2D.x}, {targetCell2D.y})�� ������ �ʴ� ��ġ�Դϴ�.");
                return;
            }

            if(_point > 0)
            {
                SendWallPoint(centerPos, inventory.keyNum, wallPrefab[inventory.keyNum - 1], inventory.keyNum);
                SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[3]);
            }
            else
            {
                Debug.Log("���� ��ġ�� ����Ʈ�� �����մϴ�.");
            }
        }
    }

    void LimitToCameraBounds()
    {
        Camera cam = Camera.main;
        Vector3 camPos = cam.transform.position;

        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        float minX = camPos.x - camHalfWidth;
        float maxX = camPos.x + camHalfWidth;
        float minY = camPos.y - camHalfHeight;
        float maxY = camPos.y + camHalfHeight;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        float Move = (int) (Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y)); 

        anim.SetFloat("Move", Move);

        // ���� ����
        if(moveInput.x != 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = moveInput.x > 0 ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
            //hpFillImage.rectTransform.localScale.x = moveInput.x > 0 ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }

    }

    // ���ӸŴ����� �� ������.
    // ����Ʈ ���
    public void SendWallPoint(Vector3 center, int point, GameObject targetWall, int keyNum)
    {
        if(_point - point < 0) return;

        Vector3 playerWorldPos = transform.position;
        float distanceToPlayer = Vector2.Distance(center, playerWorldPos);

        Vector3 targetPosition = center;

        // �Ÿ��� 1���� ũ��, �ֺ����� ���� ����� ��� ã��
        if(distanceToPlayer > 1f)
        {
            Vector3Int playerCell = tilemap.WorldToCell(playerWorldPos);

            Vector3Int[] directions = new Vector3Int[]
            {
                new Vector3Int (0, 0, 0), // �������
                new Vector3Int(1, 0, 0),   // ������
                new Vector3Int(-1, 0, 0),  // ����
                new Vector3Int(0, 1, 0),   // ��
                new Vector3Int(0, -1, 0),  // �Ʒ�
            };

            float minDistance = float.MaxValue;
            Vector3 bestPos = center; // fallback
            bool foundValid = false;

            foreach(var dir in directions)
            {
                Vector3Int checkCell = playerCell + dir;
                Vector3 checkWorldPos = tilemap.GetCellCenterWorld(checkCell);

                print("checkPos: " + checkWorldPos);

                float dist = Vector2.Distance(center, checkWorldPos);
                // �Ÿ� ���� 1���� ū ��츸 ��ȿ
                if(dist > 1f && dist < minDistance)
                {
                    minDistance = dist;
                    bestPos = checkWorldPos;
                    foundValid = true;
                }
            }

            // ��ġ ������ ��ġ�� ������ ����
            if(!foundValid)
            {
                Debug.Log("�÷��̾� �ֺ��� ��ġ ������ ��ȿ�� Ÿ���� �����ϴ�.");
                return;
            }

            targetPosition = bestPos;
        }
       

        // ���� ��ġ�� �� ��ġ
        _point -= point;
        Instantiate(targetWall, targetPosition, Quaternion.identity);
        breadpointbar.CostBreadPoint(point);
    }

    // ����Ʈ ȹ��
    public void ApeendWallPoint(int point)
    {
        _point = _point >= 50 ? Mathf.Max(MaxPoint) : _point + point;
        breadpointbar.AddBreadPoint(point);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        UpdateHPBar();
        print($"�÷��̾��� hp�Ǵ� {currentHP}�Դϴ�.");
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[4]);

        if(currentHP <= 0)
        {
            isDie = true;
            anim.SetBool("Die", isDie);
            Time.timeScale = 0f;
            DiePanel.SetActive(true);
        }
    }

    public void UpdateHPBar()
    {
        float fillAmount = (float)currentHP / hp;
        hpFillImage.fillAmount = fillAmount;
        Debug.Log(fillAmount);
    }
}
