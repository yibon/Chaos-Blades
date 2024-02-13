using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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


    GameObject king;
    public GameObject[] nonSupportEnemies;

    // Start is called before the first frame update
    void Start()
    {
        king = GameObject.FindWithTag("King");
    }

    // Update is called once per frame
    void Update()
    {
        nonSupportEnemies = GameObject.FindGameObjectsWithTag("Enemy"); //constantly updating list
        Debug.Log(nonSupportEnemies.Length);
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
                Attack();
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

    }

    void SupportBuff() //special class used for support enemy
    {

    }
}
