using System;
using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] public float hp;
    [SerializeField] float speed;
    [SerializeField] public float attack;
    [SerializeField] float attackSpeed;
    [SerializeField] float range;
    [SerializeField] int scoreAwardedOnDeath;

    public float force;

    public Rigidbody2D rb2D;

    public Collider2D c2D;

    public bool isHit = false;


    GameObject king;
    public GameObject[] nonSupportEnemies;

    // Start is called before the first frame update
    void Start()
    {
        king = GameObject.FindWithTag("King");
        c2D = this.GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //destory if ded
        if (hp <= 0)
        {
            Destroy(this.gameObject); 
        }

        nonSupportEnemies = GameObject.FindGameObjectsWithTag("Enemy"); //constantly updating list
        //Debug.Log(nonSupportEnemies.Length);
        //look for tag based on enemy type
        if (name != "Support") // enemy type is not support 
        {
            //checking distance from king and enemy by minusing the game object current pos and king current pos
            if ((this.gameObject.transform.position - king.transform.position).magnitude > range) // not in range
            {
                Run(king);
            }
            else //in range
            {

            }
        }
        else //enemy type is support
        {
            if (nonSupportEnemies.Length > 0) //still have other types of enemies around
            {
                if ((this.gameObject.transform.position - nonSupportEnemies[0].transform.position).magnitude > range) // not in range
                {
                    Run(nonSupportEnemies[0]);
                }
                else //in range
                {
                    SupportBuff();
                }
            }
            else //only left support
            {
                if ((this.gameObject.transform.position - king.transform.position).magnitude > range) // not in range
                {
                    Run(king);
                }
                else
                {
                    return;
                }
            }
            
        }

        Ded();

    }

    void Run(GameObject go) //function used when not in range
    {
        //this.transform.position = Vector2.MoveTowards(this.transform.position, king.transform.position, speed);
        Vector3 forceApplied = go.transform.position - this.transform.position;
        forceApplied = forceApplied.normalized;
        forceApplied = forceApplied * 1f;
        rb2D.AddForce(forceApplied);
    }

    void Attack() //used when in range of king
    {
        king.GetComponentInChildren<KingAI>().hp -= attack;
        //Debug.Log("king ouch");
    }

    void SupportBuff() //special class used for support enemy
    {

    }

    void Ded()
    {
        if (this.hp < 0)
        {
            // replace this with death animation
            Destroy(this.gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("King"))
        {
            Attack();
            //Debug.Log("cooling down");
            StartCoroutine(Cooldowntimer(attackSpeed));
            //Debug.Log("ready");
        }
        
    }

    IEnumerator Cooldowntimer(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
