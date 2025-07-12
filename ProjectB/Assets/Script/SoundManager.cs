using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;


    public AudioSource BGMSoundPlay;
    public AudioSource[] EffectSoundPlay;

    public AudioClip[] BGMs;
    public AudioClip[] SFXSounds;

    public GameObject settingsPanel;

    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private Slider m_MusicMasterSlider;
    [SerializeField] private Slider m_MusicBGMSlider;
    [SerializeField] private Slider m_MusicSFXSlider;

    

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
        m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        m_MusicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
        m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // 사운드 조절
    public void SetMasterVolume(float volume)
    {
        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        m_AudioMixer.SetFloat("SE", Mathf.Log10(volume) * 20);
    }

    // 세팅 패널 버튼
    public void OnSettingsButton()
    {
        settingsPanel.SetActive(true);
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[0]);
    }
    public void OnBackButton()
    {
        settingsPanel.SetActive(false);
        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[0]);
    }


    private void Start()
    {
        // 씬이 변경될 때 호출되는 이벤트에 함수를 등록
        //SceneManager.activeSceneChanged += OnSceneChanged;

    }

    //private void OnSceneChanged(Scene currentScene, Scene nextScene)
    //{

    //    if (nextScene.name == "Main" || nextScene.name == "Main 1")
    //    {
    //        // BGM이 재생 중인 경우 멈춤
    //        if (BGMSoundPlay.isPlaying)
    //        {
    //            BGMSoundPlay.Stop();

    //            BGMSoundPlay.clip = BGMs[2];
    //            BGMSoundPlay.Play();
    //        }
    //    }
    //    else if (nextScene.name == "Story")
    //    {
    //        // BGM이 재생 중인 경우 멈춤
    //        if (BGMSoundPlay.isPlaying)
    //        {
    //            BGMSoundPlay.Stop();

    //            BGMSoundPlay.clip = BGMs[1];
    //            BGMSoundPlay.Play();
    //        }
    //    }
    //    else if(nextScene.name == "Menu")
    //    {
    //        // BGM이 재생 중인 경우 멈춤
    //        if (BGMSoundPlay.isPlaying)
    //        {
    //            BGMSoundPlay.Stop();

    //            BGMSoundPlay.clip = BGMs[0];
    //            BGMSoundPlay.Play();
    //        }
    //    }
    //}

    private void Update()
    {
        
    }

    public static SoundManager Instance
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

    public void SFXPlay(AudioClip SFX)
    {
        for (int i = 0; i < EffectSoundPlay.Length; i++)
        {
            if (!EffectSoundPlay[i].isPlaying)
            {
                EffectSoundPlay[i].PlayOneShot(SFX);
                return;
            }
        }

        // 전부 재생 중이면 첫 번째 AudioSource를 강제로 재생 (덮어쓰기)
        EffectSoundPlay[0].Stop();
        EffectSoundPlay[0].PlayOneShot(SFX);
    }

}
