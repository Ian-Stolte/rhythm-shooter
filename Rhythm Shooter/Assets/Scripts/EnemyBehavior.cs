using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float maxHealth;
    public float health;
    [SerializeField] private float damage;
    [SerializeField] private float attackDelay;
    private float attackTimer;
    [SerializeField] private bool ranged;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float speed;
    [SerializeField] private int score;
    
    public GameObject dmgTxt;

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        health = maxHealth;
    }

    void Update()
    {
        if (health <= 0)
        {
            RhythmManager rhythm = GameObject.Find("Rhythm Manager").GetComponent<RhythmManager>();
            rhythm.score += score*rhythm.multiplier;
            if (rhythm.multiplier < 4)
            {
                rhythm.multiplier += 0.2f;
            }
            Destroy(gameObject);
            //and maybe play a sound or do a particle effect
        }

        attackTimer -= Time.deltaTime;
        if (!ranged)
        {
            if (Physics2D.OverlapCircle(transform.position, 0.6f, LayerMask.GetMask("Player")) && attackTimer < 0)
            {
                attackTimer = attackDelay;
                player.GetComponent<PlayerController>().TakeDamage(gameObject, damage);
            }
        }
        else
        {
            if (attackTimer < 0 && Vector3.Distance(player.transform.position, transform.position) <= 6)
            {
                GetComponent<Animator>().Play("RangedEnemyAttack");
                attackTimer = attackDelay;
                Vector3 playerDir = Vector3.Normalize(player.transform.position - transform.position);
                GameObject obj = Instantiate(bulletPrefab, transform.position + playerDir, Quaternion.identity, GameObject.Find("Bullets").transform);
                obj.GetComponent<EnemyBullet>().direction = playerDir;
                obj.GetComponent<EnemyBullet>().dmg = damage;
                obj.transform.up = player.transform.position - transform.position;
            }
        }
    }

    void FixedUpdate()
    {
        if (!ranged || Vector3.Distance(player.transform.position, transform.position) > 6)
            GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(player.transform.position - transform.position)*speed;
        else
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}
