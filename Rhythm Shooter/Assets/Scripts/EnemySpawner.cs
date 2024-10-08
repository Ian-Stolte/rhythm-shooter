using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyLayout
    {
        public float minDelay;
        public float maxDelay;
        public float[] enemyPcts;
    }

    public float spawnTimer = 3;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private EnemyLayout[] enemies;
    private int type;

    private RhythmManager r;

    void Start()
    {
        r = GameObject.Find("Rhythm Manager").GetComponent<RhythmManager>();
    }

    void Update()
    {
        if (r.songNum+r.diffLvl < r.songs.Length && !GameObject.Find("Player").GetComponent<PlayerController>().paused)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer < 0 && r.timesRepeated < r.songs[r.songNum+r.diffLvl].repeats) //TODO: better way to cut off as the song is ending
            {
                int numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
                if (numEnemies == 0)
                    numEnemies = 1;
                spawnTimer = Random.Range(enemies[r.songNum+r.diffLvl].minDelay * (numEnemies/5.0f), enemies[r.songNum+r.diffLvl].maxDelay * (numEnemies/5.0f));
                Vector3 spawnLoc = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
                float typeVal = Random.Range(0, 1.0f);
                float runningPct = 0;
                for (int i = 0; i < enemies[r.songNum+r.diffLvl].enemyPcts.Length; i++)
                {
                    runningPct += enemies[r.songNum+r.diffLvl].enemyPcts[i];
                    if (typeVal < runningPct)
                    {
                        type = i;
                        break;
                    }
                }
                Instantiate(enemyPrefabs[type], /*GameObject.Find("Player").transform.position + */10*Vector3.Normalize(spawnLoc), Quaternion.identity, GameObject.Find("Enemies").transform);
            }
        }
    }
}
