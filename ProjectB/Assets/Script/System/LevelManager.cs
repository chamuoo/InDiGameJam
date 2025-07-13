using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public int exp = 0;     //�÷��̾� ����ġ
    public int maxExp = 4;  //�÷��̾� �ִ� ����ġ
    public int level = 0;   //�÷��̾� ����
    public int wave = 0;    //���� ���̺� �ܰ�
    public bool isWave = false; //���� ���̺� ������ üũ
    public List<GameObject> enemys = new List<GameObject>(); //���� �����ִ� ���͵��� ����Ʈ
    public int willSpawnEnemy1 = 0; //�� ��������Ʈ���� ��ȯ�� 1������
    public int willSpawnEnemy2 = 0; //�� ��������Ʈ���� ��ȯ�� 2������
    public float notWaveTimer = 0; //����ð� Ÿ�̸�
    public float waveTimer = 0; //���̺�ð� Ÿ�̸�
    bool isWaveStart = false; //���̺� ���ۿ��� Ȯ��
    bool isGameStart = false; //���� ������


    public List<GameObject> Spawnpoints; //��������Ʈ���� ����Ʈ
    public ExpBar expBar;
    public PlayerController playerScript;
    //���Ŀ� �� �Ʒ��� UI�۾� �ʿ�

    public int wallCost = 0; //�÷��̾ ���� Wall ��ȯ �ڽ�Ʈ

    public TextMeshProUGUI TimerText;
    float total_noWaveTime = 60f;
    float total_WaveTime = 120f;
    public TextMeshProUGUI WaveText;
    public TextMeshProUGUI WatingText;
    public GameObject WatingBox;

    [SerializeField] private GameObject DiePanel;

    public TextMeshProUGUI systemText;
    public GameObject skipButton;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Spawnpoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("SpawnPoint"));
    }

    void Start()
    {
        exp = 0;
        maxExp = 4;
        level = 0;
        wave = 0;
        isWave = false;
        WaveText.text = "";
        systemText.text = "";

        for (int i = enemys.Count - 1; i >= 0; i--)
        {
            Destroy(enemys[i]);
        }

        willSpawnEnemy1 = 0;
        willSpawnEnemy2 = 0;
        waveTimer = 0;

        notWaveTimer = total_noWaveTime;
        isGameStart = true;

        expBar = FindObjectOfType<ExpBar>();
        playerScript = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneLoadManager.Instance.GoEnding();
        }

        if(isGameStart)
        {
            if (!isWave) //���� �ð��� ��
            {
                // �÷��̾�� ���� �浹 OFF
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Wall"), true);

                isWaveStart = false;
                if (notWaveTimer > 0f)
                {
                    notWaveTimer -= Time.deltaTime;

                    WatingText.text = "���� �ð�";
                    WatingBox.SetActive(true);

                    int minutes = Mathf.FloorToInt(notWaveTimer / 60f);
                    int seconds = Mathf.FloorToInt(notWaveTimer % 60f);
                    TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

                    if (notWaveTimer < 10f)
                    {
                        TimerText.color = Color.red;
                    }
                    else if (notWaveTimer < total_noWaveTime / 2)
                    {
                        TimerText.color = Color.yellow;
                    }
                    else
                    {
                        TimerText.color = Color.white;
                    }
                    
                }
                else
                {
                    isWave = true;
                    wave++;
                    waveTimer = total_WaveTime;

                    WatingText.text = "";
                    WatingBox.SetActive(false);
                    WaveText.text = "WAVE " + wave;
                    Invoke("invokeWaveTextshow", 2f);
                }
            }
            else //���̺� �������� ��
            {
                if(!isWaveStart)
                {
                    // �÷��̾�� ����� �浹 �ѱ�
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Wall"), false);

                    if (wave == 1)
                    {
                        willSpawnEnemy1 = 1;
                        willSpawnEnemy2 = 0;
                    }
                    else if(wave == 2)
                    {
                        willSpawnEnemy1 = 2;
                        willSpawnEnemy2 = 0;
                    }
                    else if (wave == 3)
                    {
                        willSpawnEnemy1 = 2;
                        willSpawnEnemy2 = 1;
                    }
                    else if (wave == 4)
                    {
                        willSpawnEnemy1 = 3;
                        willSpawnEnemy2 = 0;
                    }
                    else if (wave == 5)
                    {
                        willSpawnEnemy1 = 3;
                        willSpawnEnemy2 = 1;
                    }
                    else if (wave == 6)
                    {
                        willSpawnEnemy1 = 3;
                        willSpawnEnemy2 = 2;
                    }
                    else if (wave == 7)
                    {
                        willSpawnEnemy1 = 2;
                        willSpawnEnemy2 = 3;
                    }
                    else if (wave == 8)
                    {
                        willSpawnEnemy1 = 3;
                        willSpawnEnemy2 = 3;
                    }
                    else if (wave == 9)
                    {
                        willSpawnEnemy1 = 4;
                        willSpawnEnemy2 = 2;
                    }
                    else if (wave == 10)
                    {
                        willSpawnEnemy1 = 6;
                        willSpawnEnemy2 = 0;
                    }
                    else if (wave == 11)
                    {
                        willSpawnEnemy1 = 4;
                        willSpawnEnemy2 = 3;
                    }
                    else if (wave == 12)
                    {
                        willSpawnEnemy1 = 5;
                        willSpawnEnemy2 = 3;
                    }
                    else if (wave == 13)
                    {
                        willSpawnEnemy1 = 3;
                        willSpawnEnemy2 = 5;
                    }
                    else if (wave == 14)
                    {
                        willSpawnEnemy1 = 4;
                        willSpawnEnemy2 = 4;
                    }
                    else if (wave == 15)
                    {
                        willSpawnEnemy1 = 6;
                        willSpawnEnemy2 = 3;
                    }
                    else if (wave == 16)
                    {
                        willSpawnEnemy1 = 7;
                        willSpawnEnemy2 = 3;
                    }
                    else if (wave == 17)
                    {
                        willSpawnEnemy1 = 8;
                        willSpawnEnemy2 = 3;
                    }
                    else if (wave == 18)
                    {
                        willSpawnEnemy1 = 9;
                        willSpawnEnemy2 = 3;
                    }
                    else if (wave == 19)
                    {
                        willSpawnEnemy1 = 10;
                        willSpawnEnemy2 = 4;
                    }
                    else if (wave == 20)
                    {
                        willSpawnEnemy1 = 10;
                        willSpawnEnemy2 = 5;
                    }

                    for (int i = 0; i < Spawnpoints.Count; i++)
                    {
                        SpawnPoint spawnPointScript = Spawnpoints[i].GetComponent<SpawnPoint>();
                        if (spawnPointScript != null)
                        {
                            spawnPointScript.spawnableEnemy1 = willSpawnEnemy1;
                            spawnPointScript.spawnableEnemy2 = willSpawnEnemy2;
                            spawnPointScript.isWave = true;
                        }
                    }

                    isWaveStart = true;
                }

                if(isWaveStart && waveTimer > 0)
                {
                    
                    waveTimer -= Time.deltaTime;

                    int minutes = Mathf.FloorToInt(waveTimer / 60f);
                    int seconds = Mathf.FloorToInt(waveTimer % 60f);
                    TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

                    if (waveTimer < 10f)
                    {
                        TimerText.color = Color.red;
                    }
                    else if (waveTimer < total_WaveTime / 2)
                    {
                        TimerText.color = Color.yellow;
                    }
                    else
                    {
                        TimerText.color = Color.white;
                    }

                    bool isAllMonsterSpawned = true;
                    for(int i = 0; i < Spawnpoints.Count; i++)
                    {
                        if (Spawnpoints[i].GetComponent<SpawnPoint>().isWave)
                        {
                            isAllMonsterSpawned = false;
                            break;
                        }
                    }

                    if (isAllMonsterSpawned == true && enemys.Count == 0) //���̺� Ŭ���� ����
                    {
                        SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[12]);
                        notWaveTimer = total_noWaveTime; 
                        isWave = false;
                        playerScript.ApeendWallPoint(10);
                        playerScript.currentHP = 10;
                        playerScript.UpdateHPBar();

                        skipButton.SetActive(true);

                        if(wave == 20)
                        {
                            SceneLoadManager.Instance.GoEnding();
                        }
                    }

                }
                else if(isWaveStart && waveTimer <= 0)
                {
                    //�÷��̾� ���� ���ó��
                    //ó��(�޴���)���� ���ư��� UI�߻� ��Ű��
                    SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[5]);
                    waveTimer = 0f;
                    isGameStart = false;
                    Time.timeScale = 0f;
                    DiePanel.SetActive(true);
                }
            }

        }


    }

    void invokeWaveTextshow()
    {
        WaveText.text = "";
    }

    public void skipToGoWave()
    {
        notWaveTimer = 0.1f;
        skipButton.SetActive(false);
    }

    public void EnemyDie(GameObject obj)
    {
        if(enemys.Contains(obj))
        {
            enemys.Remove(obj);
        }
    }

    public void AddExp(int newExp)
    {
        exp += newExp;
        if (exp >= maxExp)
        {
            exp -= maxExp;
            maxExp += 2;
            level += 1;
            LevelUp(level);
            SoundManager.Instance.SFXPlay(SoundManager.Instance.SFXSounds[11]);

            
        }

        if (expBar != null)
        {
            expBar.RefreshExpUI();
        }
        else
        {
            print("�����Ŵ������� expBar�� ã�� ������.");
        }
    }

    void deleteSystemText()
    {
        systemText.text = "";
    }

    public void LevelUp(int newLevel)
    {
        if(level < newLevel)
        {
            if(level < 1 && newLevel >= 1)
            {
                //1�� ȿ��
            }
            if (level < 2 && newLevel >= 2)
            {
                //2�� ȿ��
                systemText.text = "������! ������ ���⸦ ����� �� �ֽ��ϴ�!";
                Invoke("deleteSystemText", 3f);
            }
            if (level < 3 && newLevel >= 3)
            {
                //3�� ȿ��
                systemText.text = "������! �ܴ��� �Ļ��� ����� �� �ֽ��ϴ�!";
                Invoke("deleteSystemText", 3f);
            }
            if (level < 4 && newLevel >= 4)
            {
                //4�� ȿ��
            }
            if (level < 5 && newLevel >= 5)
            {
                //5�� ȿ��
            }
            if (level < 6 && newLevel >= 6)
            {
                //6�� ȿ��
                systemText.text = "������! �ٰ�Ʈ ���⸦ ����� �� �ֽ��ϴ�!";
                Invoke("deleteSystemText", 3f);
            }
            if (level < 7 && newLevel >= 7)
            {
                //7�� ȿ��
            }
            if (level < 8 && newLevel >= 8)
            {
                //8�� ȿ��
                systemText.text = "������! Ÿ���� �Ļ��� ����� �� �ֽ��ϴ�!";
                Invoke("deleteSystemText", 3f);
            }
            if (level < 9 && newLevel >= 9)
            {
                //9�� ȿ��
            }
            if (level < 10 && newLevel >= 10)
            {
                //10�� ȿ��
            }
            if (level < 11 && newLevel >= 11)
            {
                //11�� ȿ��
                systemText.text = "������! ������ �Ļ��� ����� �� �ֽ��ϴ�!";
                Invoke("deleteSystemText", 3f);
            }
            if (level < 12 && newLevel >= 12)
            {
                //12�� ȿ��
            }
            if (level < 13 && newLevel >= 13)
            {
                //13�� ȿ��
                systemText.text = "������! ��ī�� ���⸦ ����� �� �ֽ��ϴ�!";
                Invoke("deleteSystemText", 3f);
            }
            if (level < 14 && newLevel >= 14)
            {
                //14�� ȿ��
            }
            if (level < 15 && newLevel >= 15)
            {
                //15�� ȿ��
                systemText.text = "������! ������ �Ļ��� ����� �� �ֽ��ϴ�!";
                Invoke("deleteSystemText", 3f);
            }
            if (level < 16 && newLevel >= 16)
            {
                //16�� ȿ��
            }
            if (level < 17 && newLevel >= 17)
            {
                //17�� ȿ��
            }
            if (level < 18 && newLevel >= 18)
            {
                //18�� ȿ��
            }
            if (level < 19 && newLevel >= 19)
            {
                //19�� ȿ��
            }
            if (level < 20 && newLevel >= 20)
            {
                //20�� ȿ��
            }
        }
    }
}
