using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Reflection;
using System.Collections.Generic;

public struct WallCount
{
    public int normalCount;
    public int strongCount;
    public int bombCount;
    public int autoCount;
    public int challengeCount;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Sprite> wallSprites;

    WallCount wallCount;

    public int keyNum { get; private set; } = 0;// 인벤토리에서 클릭한 숫자 값

    [SerializeField] Image Icon;

    private void Start()
    {
        Icon = GetComponent<Image>();

        keyNum = 1;
        Icon.sprite = wallSprites[0];

        UpdateIconSprite();
    }

    void Update()
    {
        // Q 키를 눌렀을 때 keyNum 감소 및 순환 (2줄)
        if(Keyboard.current.qKey.wasPressedThisFrame) { keyNum = (keyNum - 1 == 0) ? wallSprites.Count : keyNum - 1; UpdateIconSprite(); }
        // E 키를 눌렀을 때 keyNum 증가 및 순환 (2줄)
        else if(Keyboard.current.eKey.wasPressedThisFrame) { keyNum = (keyNum + 1 > wallSprites.Count) ? 1 : keyNum + 1; UpdateIconSprite(); }

        else if(Keyboard.current.digit1Key.wasPressedThisFrame) { keyNum = 1; UpdateIconSprite(); }
        else if(Keyboard.current.digit2Key.wasPressedThisFrame) { keyNum = 2; UpdateIconSprite(); }
        else if(Keyboard.current.digit3Key.wasPressedThisFrame) { keyNum = 3; UpdateIconSprite(); }
        else if(Keyboard.current.digit4Key.wasPressedThisFrame) { keyNum = 4; UpdateIconSprite(); }
        else if(Keyboard.current.digit5Key.wasPressedThisFrame) { keyNum = 5; UpdateIconSprite(); }
    }

    private void UpdateIconSprite()
    {
        int spriteIndex = keyNum - 1;

        if(spriteIndex >= 0 && spriteIndex < wallSprites.Count)
        {
            Icon.sprite = wallSprites[spriteIndex];
            Debug.Log($"아이콘 변경: {wallSprites[spriteIndex].name}, 현재 keyNum: {keyNum}");
        }
        else
        {
            Debug.LogWarning($"유효하지 않은 keyNum ({keyNum})으로 인해 아이콘을 업데이트할 수 없습니다. 스프라이트 리스트 크기: {wallSprites.Count}");
        }
    }

    public int AddCount(int count, int wallTypeKeyNum)
    {
        switch(wallTypeKeyNum)
        {
            case 1: return wallCount.normalCount += count;
            case 2: return wallCount.strongCount += count;
            case 3: return wallCount.bombCount += count;
            case 4: return wallCount.autoCount += count;
            case 5: return wallCount.challengeCount += count;
            default: return 0;
        }
    }

    public int RemoveCount(int count, int wallTypeKeyNum)
    {
        switch(wallTypeKeyNum)
        {
            case 1: return wallCount.normalCount -= count;
            case 2: return wallCount.strongCount -= count;
            case 3: return wallCount.bombCount -= count;
            case 4: return wallCount.autoCount -= count;
            case 5: return wallCount.challengeCount -= count;
            default: return 0;
        }
    }
}
