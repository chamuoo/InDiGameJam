using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Reflection;

public struct WallCount
{
    public int normalCount;
    public int strongCount;
    public int bombCount;
    public int autoCount;
    public int callengeCount;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] Sprite[] sprites = new Sprite[5];
    WallCount wallCount;

    public int keyNum { get; private set; } = 0;// 인벤토리에서 클릭한 숫자 값
    [SerializeField] Image Icon;

    private void Start()
    {
        Icon = GetComponent<Image>();

        keyNum = 1;
        Icon.sprite = sprites[0];
    }

    void Update()
    {
        if(Keyboard.current.digit1Key.wasPressedThisFrame) { keyNum = 0; UpdateIcon(); }
        else if(Keyboard.current.digit2Key.wasPressedThisFrame) { keyNum = 1; UpdateIcon(); }
        else if(Keyboard.current.digit3Key.wasPressedThisFrame) { keyNum = 2; UpdateIcon(); }
        else if(Keyboard.current.digit4Key.wasPressedThisFrame) { keyNum = 3; UpdateIcon(); }
        else if(Keyboard.current.digit5Key.wasPressedThisFrame) { keyNum = 4; UpdateIcon(); }

        if(Keyboard.current.qKey.wasPressedThisFrame)
        {
            keyNum = (keyNum - 1 + sprites.Length) % sprites.Length;
            UpdateIcon();
        }
        else if(Keyboard.current.eKey.wasPressedThisFrame)
        {
            keyNum = (keyNum + 1) % sprites.Length;
            UpdateIcon();
        }
    }

    void UpdateIcon()
    {
        if(keyNum >= 0 && keyNum < sprites.Length)
            Icon.sprite = sprites[keyNum];
    }

    public int AddCount(int count, int KeyNum)
    {
        switch(KeyNum + 1)
        {
            case 1: return wallCount.normalCount += count;
            case 2: return wallCount.strongCount += count;
            case 3: return wallCount.bombCount += count;
            case 4: return wallCount.autoCount += count;
            case 5: return wallCount.callengeCount += count;
        }
        return 0;
    }

    public int RemoveCount(int count, int KeyNum)
    {
        switch(KeyNum + 1)
        {
            case 1: return wallCount.normalCount -= count;
            case 2: return wallCount.strongCount -= count;
            case 3: return wallCount.bombCount -= count;
            case 4: return wallCount.autoCount -= count;
            case 5: return wallCount.callengeCount -= count;
        }
        return 0;
    }
}
