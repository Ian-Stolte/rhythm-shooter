using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float spawnTimer = 3;
    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;
    
    [SerializeField] private GameObject enemyPrefab;
    private RhythmManager r;

    void Start()
    {
        r = GameObject.Find("Rhythm Manager").GetComponent<RhythmManager>();
    }

    void Update()
    {
        if (r.songNum < r.songs.Length)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer < 0 && r.timesRepeated < r.songs[r.songNum].repeats-1) //TODO: better way to cut off as the song is ending
            {
                spawnTimer = Random.Range(minDelay, maxDelay);
                Vector3 spawnLoc = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
                Instantiate(enemyPrefab, 10*Vector3.Normalize(spawnLoc), Quaternion.identity, GameObject.Find("Enemies").transform);
            }
        }
    }
}
