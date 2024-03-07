
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

    public Animator animator;


    //spawn bullet
    public GameObject EnemyBulletPrefab;
    public Transform firePoint;
    float nextAttackTime;

    GameObject king;
    public GameObject protectedSprite;
    public GameObject[] nonSupportEnemies;
    
    
    public bool isProtected;
    float protectionTimer;

    // Start is called before the first frame update
    void Start()
    {
        king = GameObject.FindWithTag("King");
        c2D = this.GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isProtected)
        {
            protectedSprite.SetActive(true);
            protectionTimer += Time.deltaTime;
            if (protectionTimer > 3)
            {
                protectionTimer = 0;
                protectedSprite.SetActive(false);
                isProtected = false;
            }
        }

        //destory if 0 hp
        if (hp <= 0)
        {
            ScoreManager.instance.EnemyKilled(scoreAwardedOnDeath);
            Destroy(this.gameObject);
        }

        nonSupportEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        //look for tag based on enemy type
        #region General Enemy AI 
        if (name != "Support") 
        {
            //not in range
            if ((this.gameObject.transform.position - king.transform.position).magnitude > range) 
            {
                Run(king);
            }
            //in range
            else
            {
                //if enemy ranged
                if (range > 2)
                {
                    if (nextAttackTime < Time.time)
                    {
                        RangeAttack();
                    }
                }
                
            }
        }
        #endregion
        #region Support Enemy AI
        //Support Enemy
        else
        {
            //still have other types of enemies around
            if (nonSupportEnemies.Length > 0) 
            {
                // not in range
                if ((this.gameObject.transform.position - nonSupportEnemies[0].transform.position).magnitude > range) 
                {
                    Run(nonSupportEnemies[0]);
                }
                else //in range
                {
                    if (nextAttackTime < Time.time)
                    {
                        SupportBuff();
                    }
                        
                }
            }
            //only left support
            else
            {
                // not in range
                if ((this.gameObject.transform.position - king.transform.position).magnitude > range) 
                {
                    Run(king);
                }
                else
                {
                    return;
                }
            }
            
        }
        #endregion
    }

    //function used when not in range
    void Run(GameObject go)
    {
        Vector3 forceApplied = go.transform.position - this.transform.position;
        forceApplied = forceApplied.normalized;
        forceApplied = forceApplied * 1f;
        rb2D.AddForce(forceApplied);
    }

    void RangeAttack()
    {
        animator.Play("Wisp_Attack");

        //spawn bullet prefab
        GameObject EnemyProjectile = Instantiate(EnemyBulletPrefab, this.firePoint.position, this.firePoint.rotation);
        EnemyProjectile.transform.parent = this.transform;

        nextAttackTime = Time.time + attackSpeed;
    }

    void Attack() //used when in range of king
    {
        if (!king.GetComponentInChildren<KingAI>().kingProtected)
        {
            king.GetComponentInChildren<KingAI>().hp -= attack;
        }
    }

    void SupportBuff() //special class used for support enemy
    {
        animator.Play("Shaman_Cast");
        nonSupportEnemies[0].GetComponent<EnemyAI>().attack += 1;

        nextAttackTime = Time.time + attackSpeed;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("King"))
        {
            Attack();
            StartCoroutine(Cooldowntimer(attackSpeed));
        }
    }

    IEnumerator Cooldowntimer(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
