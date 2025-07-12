using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    // 일시정지
    [SerializeField] private GameObject PausePanel;
    public bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // 게임 일시정지
            PausePanel.SetActive(true); // UI 표시
        }
        else
        {
            Time.timeScale = 1f; // 게임 재개
            PausePanel.SetActive(false); // UI 숨김
        }
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f; // 게임 재개
        PausePanel.SetActive(false); // UI 숨김
        SceneManager.LoadScene("Menu");
        SceneLoadManager.Instance.MainMenuCanvas.SetActive(true);
    }
    public void SettingButton()
    {
        SoundManager.Instance.OnSettingsButton();
    }
}
