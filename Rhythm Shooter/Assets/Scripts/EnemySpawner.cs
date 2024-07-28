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

    private float spawnTimer = 3;
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
        if (r.songNum < r.songs.Length && !GameObject.Find("Player").GetComponent<PlayerController>().paused)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer < 0 && r.timesRepeated < r.songs[r.songNum].repeats) //TODO: better way to cut off as the song is ending
            {
                spawnTimer = Random.Range(enemies[r.songNum].minDelay, enemies[r.songNum].maxDelay);
                Vector3 spawnLoc = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
                float typeVal = Random.Range(0, 1.0f);
                float runningPct = 0;
                for (int i = 0; i < enemies[r.songNum].enemyPcts.Length; i++)
                {
                    runningPct += enemies[r.songNum].enemyPcts[i];
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
