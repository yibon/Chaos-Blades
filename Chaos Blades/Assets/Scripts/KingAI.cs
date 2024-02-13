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

    public Rigidbody2D rb2D;
    public Collider2D attackCollider2D;
    
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
        if (GameObject.FindGameObjectWithTag("Enemy") == true)
        {
            Vector3 forceApplied = GameObject.FindGameObjectWithTag("Enemy").transform.position - this.transform.position; //EnemyAI.instance.nonSupportEnemies[EnemyAI.instance.nonSupportEnemies.Length - 1].transform.position - this.transform.position;
            forceApplied = forceApplied.normalized;
            forceApplied = forceApplied * 1f;
            rb2D.AddForce(forceApplied);
        }
        else if (GameObject.FindGameObjectWithTag("Enemy Support") == true)
        {
            GameObject enemySupport = GameObject.FindWithTag("Enemy Support");
            Vector3 forceApplied = enemySupport.transform.position - this.transform.position;
            forceApplied = forceApplied.normalized;
            forceApplied = forceApplied * 1f;
            rb2D.AddForce(forceApplied);
        }
        else
        {
            return;
        }

        if (hp <= 0)
        {
            Destroy(this.gameObject);
            // put in the game over screen here
            // can start the calculations for score now
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag ("Enemy Support"))
        {
            EnemyAI.FindObjectOfType<EnemyAI>().hp -= attack;
            Debug.Log("ouch");
        }
        
    }
}
