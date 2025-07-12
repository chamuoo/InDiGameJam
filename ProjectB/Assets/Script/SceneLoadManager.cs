using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private static SceneLoadManager instance = null;

    public GameObject MainMenuCanvas;


    private void Awake()
    {
        // 이미 인스턴스가 존재하는지 확인
        if (instance == null)
        {
            // 인스턴스가 존재하지 않으면 현재 인스턴스를 저장하고 삭제되지 않도록 설정
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 인스턴스가 이미 존재하면 중복되는 인스턴스를 삭제
            Destroy(gameObject);
        }
    }
    public static SceneLoadManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoInGame()
    {
        SceneManager.LoadScene("Test1");
        MainMenuCanvas.SetActive(false);
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[0]);
    }
}
