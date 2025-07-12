using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public int exp = 0;
    public int level = 0;
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    public int enemyCount = 0;
    public List<GameObject> enemys = new List<GameObject>();
    public int enemySpawnCount = 0;
    public List<GameObject> Spawnpoints;
    int enemySpawnMaxCount = 1;
    float spawnTimer;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Spawnpoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("SpawnPoint"));
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void levelReset(int newLevel)
    {
        level = newLevel;
        for (int i = enemys.Count - 1; i >= 0; i--)
        {
            Destroy(enemys[i]);
        }
        enemyCount = 0;
        enemySpawnCount = 0;

    }
}
