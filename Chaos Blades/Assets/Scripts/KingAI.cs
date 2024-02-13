using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KingAI : MonoBehaviour
{
    [SerializeField] public float hp;
    [SerializeField] float speed;
    [SerializeField] public float attack;
    [SerializeField] float attackSpeed;
    [SerializeField] float range;

    EnemyAI enemyAI;
    Rigidbody2D rb2D;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //constantly attacking

        //constantly looking for enemy
        //replace the obj searching with a list that is genereated from spawner.cs
        if (enemyAI.nonSupportEnemies.Length > 0)
        {
            Vector3 forceApplied = enemyAI.nonSupportEnemies[enemyAI.nonSupportEnemies.Length - 1].transform.position - this.transform.position;
            forceApplied = forceApplied.normalized;
            forceApplied = forceApplied * 1f;
            rb2D.AddForce(forceApplied);
        }
        else
        {
            GameObject enemySupport = GameObject.FindWithTag("EnemySupport");
            Vector3 forceApplied = enemySupport.transform.position - this.transform.position;
            forceApplied = forceApplied.normalized;
            forceApplied = forceApplied * 1f;
            rb2D.AddForce(forceApplied);
        }
    }

   /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyAI>() != null)
        {
            enemyAI = collision.GetComponent<EnemyAI>();
            enemyAI.hp -= attack;
        }
    }*/
}
