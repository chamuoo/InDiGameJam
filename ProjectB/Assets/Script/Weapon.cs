using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawnPos;
    [SerializeField] GameObject weapon;
    WeaponIcon weaponIcon;

    PlayerInput playerInput;
    [SerializeField] PlayerController player;

    float fireTimer = 0f;
    [SerializeField] float fireDelay = 0.2f;

    bool isFiring = false;
    bool hasFiredOnce = false;

    float pressDuration = 0f;
    [SerializeField] float autoFireThreshold = 0.25f;

    enum FireMode { None, Single, Auto }
    FireMode currentFireMode = FireMode.None;

    private void Start()
    {
        weaponIcon = weapon.GetComponent<WeaponIcon>();
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
        if(isFiring)
        {
            pressDuration += Time.deltaTime;
            fireTimer += Time.deltaTime;

            if(currentFireMode == FireMode.None && fireTimer >= fireDelay)
            {
                currentFireMode = FireMode.Auto;
                fireTimer = fireDelay; // 즉시 첫 발
            }

            if(currentFireMode == FireMode.Auto && fireTimer >= fireDelay)
            {
                FireBullet();
                fireTimer = 0f;
            }
        }
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            isFiring = true;
            player.anim.SetBool("Attack", true);
            pressDuration = 0f;
            fireTimer = 0f;
            hasFiredOnce = false;
            currentFireMode = FireMode.None;
        }
        else if(context.canceled)
        {
            isFiring = false;
            player.anim.SetBool("Attack", false);

            // 눌렀다 바로 뗀 경우 (단발 모드)
            if(pressDuration < autoFireThreshold && !hasFiredOnce)
            {
                FireBullet();
                hasFiredOnce = true;
            }

            currentFireMode = FireMode.None;
        }
    }

    private void FireBullet()
    {
        Sprite sprite = weaponIcon.GetCurrentWeaponSprite();
        if(sprite == null) return;

        string objectName = "Fire_" + sprite.name;
        string resourcePath = "Prefab/Player/" + objectName;

        bullet = Resources.Load<GameObject>(resourcePath);
        Instantiate(bullet, transform.position, Quaternion.identity);

        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[6]);
    }
}
