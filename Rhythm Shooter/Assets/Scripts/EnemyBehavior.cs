using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float health;
    [SerializeField] private float damage;
    [SerializeField] private float attackDelay;
    private float attackTimer;
    [SerializeField] private float speed;
    public Color32 dmgColor;
    
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            //and maybe play a sound or do a particle effect
        }

        attackTimer -= Time.deltaTime;
        if (Physics2D.OverlapCircle(transform.position, 0.6f, LayerMask.GetMask("Player")) && attackTimer < 0)
        {
            attackTimer = attackDelay;
            player.GetComponent<PlayerController>().TakeDamage(gameObject, damage);
        }
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(player.transform.position - transform.position)*speed;
    }
}
