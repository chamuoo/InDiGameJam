using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // 입력 액션
    PlayerInput playerInput;

    [SerializeField] Tilemap tilemap;

    // 애니메이션
    public Animator anim { get; private set; }

    Inventory inventory;

    Vector2 moveInput;  // 입력값

    public List<GameObject> wallPrefab; //{ get; [SerializeField] set; }

    // 나중에 GameManager의 벽포인트를 추가를 하고 관리를 할 것임. 
    public int _point; // { get; [SerializeField] set; }
    int hp = 10;
    int MaxPoint = 50;

    public Image hpFillImage;
    public int currentHP;

    Rigidbody2D rigidbody;

    public Vector2 targetPos { get; private set; }  // 플레이어 방향벡터

    private readonly List<Vector2Int> disallowedAbsoluteTileCoords = new List<Vector2Int>
{
        new Vector2Int(0, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, -1)
};

    float dot;

    float movespeed = 1f;

    BreadPointBar breadpointbar;

    public AudioSource footstepAudioSourse01;
    public AudioSource footstepAudioSourse02;
    private bool switchStepSound = false;

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
        inventory = GameObject.Find("Icon").GetComponent<Inventory>();

        wallPrefab = Resources.LoadAll<GameObject>("Walls").ToList();
        rigidbody = GetComponent<Rigidbody2D>();

        breadpointbar = FindObjectOfType<BreadPointBar>();
        currentHP = hp;
    }

    private void Update()
    {
        if (rigidbody.velocity != Vector2.zero)
            rigidbody.velocity = Vector2.zero;
        // 새로운 InputSystem의 마우스 위치 가져오기
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 playerPos = transform.position;

        Vector2 basePos = mousePos - playerPos;
        // 타일기준 플레이어의 위치
        Vector3Int playerCell = tilemap.WorldToCell(playerPos);

        // Target tile for wall placement (based on player's position + small offset)
        Vector3Int targetCell = tilemap.WorldToCell((Vector3)playerPos + Vector3.down * 0.2f); // Using the existing logic for baseCell

        // World center of the target tile
        Vector3 centerPos = tilemap.GetCellCenterWorld(targetCell);

        targetPos = basePos.normalized;

        Vector2 movement = new Vector2(moveInput.x, moveInput.y);
        
        // 대각선 이동 속도 보정
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

        // 벽 생성
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            print("벽 설치 시도 타일: " + targetCell);

            Vector2Int targetCell2D = new Vector2Int(targetCell.x, targetCell.y);

            if(disallowedAbsoluteTileCoords.Contains(targetCell2D))
            {
                Debug.Log($"벽 설치 불가: 타일맵의 절대 좌표 ({targetCell2D.x}, {targetCell2D.y})는 허용되지 않는 위치입니다.");
                return;
            }

            if(_point > 0)
            {
                Collider2D hit = Physics2D.OverlapCircle(centerPos, 0.1f, LayerMask.GetMask("Wall"));
                if(hit == null)
                {
                    SendWallPoint(centerPos, inventory.keyNum, wallPrefab[inventory.keyNum - 1], inventory.keyNum);

                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[3]);
                }
                else
                {
                    Debug.Log("이미 벽이 존재합니다. 벽이 설치되지 않았습니다.");
                }
            }
            else
            {
                Debug.Log("벽을 설치할 포인트가 부족합니다.");
            }
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        float Move = (int) (Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y)); 

        anim.SetFloat("Move", Move);
        
    }

    // 게임매니저가 할 예정임.
    // 포인트 사용
    public void SendWallPoint(Vector3 center, int point, GameObject targetWall, int keyNum)
    {
        _point -= point;
        Instantiate(targetWall, center, Quaternion.identity);
        breadpointbar.CostBreadPoint(point);
    }

    // 포인트 획득
    public void ApeendWallPoint(int point)
    {
        _point = _point >= 50 ? Mathf.Max(MaxPoint) : _point + point;
        breadpointbar.AddBreadPoint(point);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        UpdateHPBar();
        print($"플레이어의 hp피는 {currentHP}입니다.");
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[4]);

        if (hp <= 0)
            Destroy(this.gameObject);
    }

    void UpdateHPBar()
    {
        float fillAmount = (float)currentHP / hp;
        hpFillImage.fillAmount = fillAmount;
        Debug.Log(fillAmount);
    }
}
