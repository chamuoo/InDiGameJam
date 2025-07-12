using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Collections.Generic;

public struct GunCount
{
    public int pirtalCount;
    public int UZICount;
    public int RailGunCount;
    public int RocketRuncherCount;
}

public class WeaponIcon : MonoBehaviour
{
    [SerializeField] private List<Sprite> weaponSprites;

    public Image img;

    public float scrollValue = 0f;
    public float scrollSpeed = 1f;
    public float minValue = 0f;
    public float maxValue = 10f;

    public int previousIconIndex  = -1;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        if(Mouse.current == null) return;

        float scrollDelta = Mouse.current.scroll.ReadValue().y;

        if(Mathf.Abs(scrollDelta) > 0.01f)
        {
            scrollValue += scrollDelta * scrollSpeed * Time.deltaTime;
            scrollValue = Mathf.Clamp(scrollValue, minValue, maxValue);
            Debug.Log($"Scroll Value: {scrollValue}");
        }
        else if(scrollDelta >= 10f)
        {
            scrollDelta = 0;
        }

        UpdateIconSprite();
    }

    private void UpdateIconSprite()
    {
        int newIconIndex = Mathf.FloorToInt(scrollValue / 2f) % weaponSprites.Count;
        newIconIndex = (newIconIndex + weaponSprites.Count) % weaponSprites.Count; // 음수 결과 방지

        // 이전 인덱스와 다를 때만 스프라이트 업데이트
        if(newIconIndex != previousIconIndex)
        {
            img.sprite = weaponSprites[newIconIndex]; // [한 줄] 스프라이트 할당
            previousIconIndex = newIconIndex; // 현재 인덱스 저장

            Debug.Log($"아이콘 변경: {weaponSprites[newIconIndex].name} (인덱스: {newIconIndex})");
        }
    }
}
