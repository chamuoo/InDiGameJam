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
    [SerializeField] Sprite normal;
    [SerializeField] Sprite strong;
    [SerializeField] Sprite bomb;
    [SerializeField] Sprite auto;
    [SerializeField] Sprite callenge;

    WallCount wallCount;

    public int keyNum { get; private set; } = 0;// 인벤토리에서 클릭한 숫자 값

    [SerializeField] Image Icon;

    private void Start()
    {
        Icon = GetComponent<Image>();

        keyNum = 1;
        Icon.sprite = normal;
    }

    void Update()
    {
        if(Keyboard.current.digit1Key.wasPressedThisFrame)    // 일반 블록 아이콘 보여주기
        {
            keyNum = 1;
            Icon.sprite = normal;
        }
        else if(Keyboard.current.digit2Key.wasPressedThisFrame)    // 일반 블록 아이콘 보여주기
        {
            keyNum = 2;
            Icon.sprite = strong;
        }
        else if(Keyboard.current.digit3Key.wasPressedThisFrame)    // 일반 블록 아이콘 보여주기
        {
            keyNum = 3;
            Icon.sprite = bomb;
        }
        else if(Keyboard.current.digit4Key.wasPressedThisFrame)    // 일반 블록 아이콘 보여주기
        {
            keyNum = 4;
            Icon.sprite = auto;
        }
        else if(Keyboard.current.digit5Key.wasPressedThisFrame)    // 일반 블록 아이콘 보여주기
        {
            keyNum = 5;
            Icon.sprite = callenge;
        }

    }

    public int AddCount(int count, int KeyNum)
    {
        switch(keyNum)
        {
            case 1:
                return wallCount.normalCount += count;
            case 2:
                return wallCount.strongCount += count;
            case 3:
                return wallCount.bombCount += count;
            case 4:
                return wallCount.autoCount += count;
            case 5:
                return wallCount.callengeCount += count;
            default:
                break;
        }

        return 0;
    }

    public int RemoveCount(int count, int KeyNum)
    {
        switch(keyNum)
        {
            case 1:
                return wallCount.normalCount -= count;
            case 2:
                return wallCount.strongCount -= count;
            case 3:
                return wallCount.bombCount -= count;
            case 4:
                return wallCount.autoCount -= count;
            case 5:
                return wallCount.callengeCount -= count;
            default:
                break;
        }

        return 0;
    }
}
