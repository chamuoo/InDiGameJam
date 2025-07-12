using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    GameObject bullet;
    [SerializeField] Transform bulletSpawnPos;
    [SerializeField] WeaponIcon weaponIcon;

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

    private void Update()
    {

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

    private void LoadBulletFromSprite()
    {
        if (weaponIcon == null || weaponIcon.img == null || weaponIcon.img.sprite == null) return;

        string spriteName = "Fire_" + weaponIcon.img.sprite.name;
        GameObject loadedPrefab = Resources.Load<GameObject>($"Prefab/Player/{spriteName}");
        print($"localedPrefabs: {loadedPrefab}, spriteName: {spriteName}" );

        if (loadedPrefab != null)
        {
            bullet = loadedPrefab;
        }
    }

    // 총알 발사 메서드
    private void FireBullet()
    {
<<<<<<< Updated upstream
        Instantiate(bullet, transform.position, Quaternion.identity);
        Fire_Hardtack hardtackMethod = bullet.GetComponent<Fire_Hardtack>();
        print(player.targetPos);
        hardtackMethod.targetDirection = player.targetPos;
        // 사운드, 이펙트 등을 여기에 추가
        // 
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[6]);
=======
        LoadBulletFromSprite();
        if(bullet == null) return;

        GameObject spawnedBullet = Instantiate(bullet, bulletSpawnPos.position, Quaternion.identity);
        Fire_Hardtack hardtackMethod = spawnedBullet.GetComponent<Fire_Hardtack>();
        if(hardtackMethod != null)
        {
            hardtackMethod.targetDirection = player.targetPos;
        }

        //Instantiate(bullet, transform.position, Quaternion.identity);
        //Fire_Hardtack hardtackMethod = bullet.GetComponent<Fire_Hardtack>();
        //print(player.targetPos);
        //hardtackMethod.targetDirection = player.targetPos;
        // 사운드, 이펙트 등을 여기에 추가 
>>>>>>> Stashed changes
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


