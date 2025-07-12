using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Collections.Generic;

public class WeaponIcon : MonoBehaviour
{
    [SerializeField] private List<Sprite> weaponSprites;

    public Image img;

    public float scrollValue = 0f;
    public float scrollSpeed = 1f;
    public float minValue = 0f;
    public float maxValue = 10f;

    private int currentIndex = 0;
    private int previousIconIndex = -1;

    private float lastScrollTime = 0f;
    [SerializeField] private float scrollIdleResetTime = 1.5f; // 입력 없을 시 초기화 시간 (초)

    private void Start()
    {
        img = GetComponent<Image>();

        UpdateIconSprite();
    }

    private void Update()
    {
        if(Mouse.current == null) return;

        float scrollDelta = Mouse.current.scroll.ReadValue().y;

        if(Mathf.Abs(scrollDelta) > 0.01f)
        {
            if(scrollDelta > 0f)
                currentIndex = (currentIndex + 1) % weaponSprites.Count;
            else if(scrollDelta < 0f)
                currentIndex = (currentIndex - 1 + weaponSprites.Count) % weaponSprites.Count;
        }

        UpdateIconSprite();
    }

    private void UpdateIconSprite()
    {
        if(currentIndex != previousIconIndex)
        {
            img.sprite = weaponSprites[currentIndex];
            previousIconIndex = currentIndex;

            Debug.Log($"아이콘 변경: {weaponSprites[currentIndex].name} (인덱스: {currentIndex})");
        }
    }

    public int GetCurrentIndex() => currentIndex;

    public Sprite GetCurrentWeaponSprite()
    {
        return weaponSprites[currentIndex]; // currentIndex는 내부에서 관리 중
    }
}