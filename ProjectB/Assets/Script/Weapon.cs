using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawnPos;

    PlayerInput playerInput;
    [SerializeField] PlayerController player;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnEnable()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerInput.actions["Fire"].performed += OnFire;
        playerInput.actions["Fire"].canceled += OnFire;
    }

    private void OnDisable()
    {
        playerInput.actions["Fire"].performed -= OnFire;
        playerInput.actions["Fire"].canceled -= OnFire;
    }

    [SerializeField] float fireDelay = 0.2f;
    private Coroutine fireCoroutine;

    private void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed)  // 마우스를 누르기 시작한 순간
        {
            player.anim.SetBool("Attack", true);
            fireCoroutine = StartCoroutine(FireContinuously());
        }
        else if(context.canceled)  // 마우스 버튼에서 손을 뗀 순간
        {
            if(fireCoroutine != null)
            {
                player.anim.SetBool("Attack", false);
                StopCoroutine(fireCoroutine);
            }
        }
    }

    // 총알 발사 메서드
    private void FireBullet()
    {
        Instantiate(bullet, transform.position, Quaternion.identity);
        Fire_Hardtack hardtackMethod = bullet.GetComponent<Fire_Hardtack>();
        print(player.targetPos);
        hardtackMethod.targetDirection = player.targetPos;
        // 사운드, 이펙트 등을 여기에 추가
        // 
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[6]);
    }

    // 일정 시간마다 발사하는 루틴
    private IEnumerator FireContinuously()
    {
        while(true)
        {
            // 총알 발사
            FireBullet();
            yield return new WaitForSeconds(fireDelay);
        }
    }
}


