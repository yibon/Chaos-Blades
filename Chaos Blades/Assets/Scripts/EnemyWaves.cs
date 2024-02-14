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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawning()
    {
        float timer = 0;
        timer += Time.deltaTime;

        eWaves currWave = WavesList[currWaveIndex];
        currMelee = currWave.numMelee;
        currRanged = currWave.numRanged;
        currSupport = currWave.numSupport;
        currTank = currWave.numTank;

        int totalEnemiesSpawn = 0;
        int randomEnemy = Random.Range(0, 4);

        if (totalEnemiesSpawn < currWave.maxEnemies)
        {
            switch (randomEnemy)
            {
                // Rolled Melee
                case 0:
                    if (currMelee > 0)
                    {
                        --currMelee;
                    }
                    return;
                // Rolled Ranged
                case 1:
                    if (currRanged > 0)
                    {
                        --currRanged;
                    }
                    return;

                // Rolled Support
                case 2:
                    if (currSupport > 0)
                    {
                        --currSupport;
                    }
                    return;

                // Rolled Tank
                case 3:
                    if (currTank > 0)
                    {
                        --currTank;
                    }
                    return;
            }
        }

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
}
