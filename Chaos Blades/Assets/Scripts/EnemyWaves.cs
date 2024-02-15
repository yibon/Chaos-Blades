using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaves : MonoBehaviour
{
    public static EnemyWaves instance { get; private set; }

    public List<eWaves> WavesList = new List<eWaves>();

    public int currWaveIndex;

    int currMelee;
    int currRanged;
    int currSupport;
    int currTank;
    int totalEnemiesSpawn;

    [SerializeField] Transform[] spawnPoints = new Transform[6];
    [SerializeField] GameObject[] enemyPF = new GameObject[4];

    float waveTimer;
    float enemyTimer;

    bool enemySpawning;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }

        #region DATA STUFF
        // index, number of melee, number of ranged, number of support, number of tanks, enemy time interval, waves time interval
        eWaves e1 = new eWaves(0, 2, 0, 0, 0, 7, 25, 2);
        eWaves e2 = new eWaves(1, 2, 1, 0, 0, 7, 30, 3);
        eWaves e3 = new eWaves(2, 3, 1, 0, 0, 7, 35, 4);
        eWaves e4 = new eWaves(3, 3, 1, 1, 0, 6.5f, 40, 5);
        eWaves e5 = new eWaves(4, 4, 2, 1, 0, 6.5f, 50, 7);
        eWaves e6 = new eWaves(5, 5, 2, 1, 0, 6.5f, 55, 8);
        eWaves e7 = new eWaves(6, 5, 3, 1, 0, 6, 65, 9);
        eWaves e8 = new eWaves(7, 4, 3, 1, 1, 6, 70, 9);
        eWaves e9 = new eWaves(8, 5, 3, 2, 1, 6, 80, 11);
        eWaves e10 = new eWaves(9, 6, 3, 2, 1, 5.5f, 85, 12);
        eWaves e11 = new eWaves(10, 6, 3, 2, 2, 5.5f, 90, 13);
        eWaves e12 = new eWaves(11, 7, 4, 2, 2, 5.5f, 95, 15);
        eWaves e13 = new eWaves(12, 8, 6, 3, 3, 5, 30, 20);
        eWaves e14 = new eWaves(13, 8, 7, 3, 4, 4, 25, 22);
        eWaves e15 = new eWaves(14, 8, 8, 3, 5, 3, 20, 24);

        WavesList.Add(e1);
        WavesList.Add(e2);
        WavesList.Add(e3);
        WavesList.Add(e4);
        WavesList.Add(e5);
        WavesList.Add(e6);
        WavesList.Add(e7);
        WavesList.Add(e8);
        WavesList.Add(e9);
        WavesList.Add(e10);
        WavesList.Add(e11);
        WavesList.Add(e12);
        WavesList.Add(e13);
        WavesList.Add(e14);
        WavesList.Add(e15);
        #endregion
        
        currWaveIndex = 0; 
        Spawning(currWaveIndex);

        enemySpawning = true;
    }

    // Update is called once per frame
    void Update()
    {
        waveTimer += Time.deltaTime;
        enemyTimer += Time.deltaTime;

        // When wave timer goes off,
        if (waveTimer > WavesList[currWaveIndex].waveSpawnInterval)
        {
            // current wave index increases
            ++currWaveIndex;
            Debug.Log("Wave Timer ran out: " + waveTimer + " || Current Wave is: " + currWaveIndex);
            waveTimer = 0;
            enemyTimer = 0;
            totalEnemiesSpawn = 0;
        }

        // When enemy timer goes off,
        if (enemyTimer > WavesList[currWaveIndex].enemySpawnInterval)
        {
            Debug.Log("Enemy Timer ran out: " + enemyTimer);
            // Spawns enemy
            enemySpawning = true;
            // Resets
            enemyTimer = 0;
        }

        if (enemySpawning)
        {
            //Debug.Log("Spawning!");
            // Re-rolling the odds untill an enemy is spawned
            if(Spawning(currWaveIndex))
            {
                // Spawn once
                enemySpawning = false;
            }
        }
    }

    bool Spawning(int waveIndex)
    {
        eWaves currWave = WavesList[waveIndex];

        currMelee = currWave.numMelee;
        currRanged = currWave.numRanged;
        currSupport = currWave.numSupport;
        currTank = currWave.numTank;

        int randomEnemy = Random.Range(0, 4);
        // Checking Spawn Interval
        // If there are more enemies more to spawn
        if (totalEnemiesSpawn < currWave.maxEnemies)
        {
            Debug.Log("Checking for Enemies");
            // Rolling enemy
            switch (randomEnemy)
            {
                // Rolled Melee
                case 0:
                    Debug.Log("Rolled Melee");
                    // Checking if there are anymore melee to spawn
                    if (currMelee > 0)
                    {
                        Debug.Log("Spawned Melee");
                        Instantiate(enemyPF[0], GetRandomPosition(), Quaternion.identity);
                        --currMelee;
                        ++totalEnemiesSpawn;
                        return true;
                    }
                    return false;
                // Rolled Ranged
                case 1:
                    Debug.Log("Rolled Ranged");
                    if (currRanged > 0)
                    {
                        Debug.Log("Spawned Ranged");
                        Instantiate(enemyPF[1], GetRandomPosition(), Quaternion.identity);
                        --currRanged;
                        ++totalEnemiesSpawn;
                        return true;
                    }
                    return false;
                // Rolled Support
                case 2:
                    Debug.Log("Rolled Support");
                    if (currSupport > 0)
                    {
                        Debug.Log("Spawned Support");
                        Instantiate(enemyPF[2], GetRandomPosition(), Quaternion.identity);
                        --currSupport;
                        ++totalEnemiesSpawn;
                        return true;
                    }
                    return false;
                // Rolled Tank
                case 3:
                    Debug.Log("Rolled Tank");
                    if (currTank > 0)
                    {
                        Debug.Log("Spawned Tank");
                        Instantiate(enemyPF[3], GetRandomPosition(), Quaternion.identity);
                        --currTank;
                        ++totalEnemiesSpawn;
                        return true;
                    }
                    return false;
                default: 
                    return false;
            }
        }
        return false;
    }

    public struct eWaves
    {
        public int index;

        public int numMelee;
        public int numRanged;
        public int numSupport;
        public int numTank;

        public float enemySpawnInterval;
        public float waveSpawnInterval;

        public int maxEnemies;
        public eWaves (int _index , int _numMelee, int _numRanged, int _numSupport, int _numTank, 
                       float _enemySpawnInterval, float _waveSpawnInterval, int _maxEnemies)
        {
            this.index = _index;
            this.numMelee = _numMelee;
            this.numRanged = _numRanged;
            this.numSupport = _numSupport; 
            this.numTank = _numTank;
            this.enemySpawnInterval = _enemySpawnInterval;
            this.waveSpawnInterval = _waveSpawnInterval;
            this.maxEnemies = _maxEnemies;
        }
    }

    Vector2 GetRandomPosition()
    {
        int randomPos = Random.Range(0, 6);
        switch (randomPos)
        {
            case 0:
                return spawnPoints[0].position;
            case 1:
                return spawnPoints[1].position;
            case 2:
                return spawnPoints[2].position;
            case 3:
                return spawnPoints[3].position;
            case 4:
                return spawnPoints[4].position;
            case 5:
                return spawnPoints[5].position;

            default:
                return Vector2.zero;
        }
    }
}
