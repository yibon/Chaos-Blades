
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class KingAI : MonoBehaviour
{
    [SerializeField] public float hp;
    [SerializeField] float speed;
    [SerializeField] public float attack;
    [SerializeField] float attackSpeed;
    [SerializeField] float range;

    [HideInInspector]public bool kingIsHit = false;

    public Rigidbody2D rb2D;
    public Collider2D attackCollider2D;

    public bool kingProtected;
    public GameObject protectedSprite;

    float protectionTimer;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (kingProtected)
        {
            protectedSprite.SetActive(true);
            protectionTimer += Time.deltaTime;
            if (protectionTimer > 3)
            {
                protectionTimer = 0;
                protectedSprite.SetActive(false);
                kingProtected = false;
            }
        }

        //if king is hit
        if (kingIsHit ==  true) 
        {
            StartCoroutine("HitFlash", this.gameObject);
            kingIsHit= false;
        }


        //constantly attacking

        //constantly looking for enemy
        //replace the obj searching with a list that is genereated from spawner.cs
        if (GameObject.FindGameObjectWithTag("Enemy") == true)
        {
            Vector3 forceApplied = GameObject.FindGameObjectWithTag("Enemy").transform.position - this.transform.position;
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
            
            // put in the game over screen here
            // can start the calculations for score now
            ScoreManager.instance.GameOver();
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag ("Enemy Support"))
        {
            if (!collision.gameObject.GetComponent<EnemyAI>().isProtected)
            {
                collision.gameObject.GetComponent<EnemyAI>().hp -= attack;
                collision.gameObject.GetComponent<EnemyAI>().enemyIsHit = true;
                //Debug.Log("ouch");
            }
        }
        
    }

    //flashing red
    public IEnumerator HitFlash(GameObject go)
    {
        Color originalColour = go.GetComponentInChildren<SpriteRenderer>().color;
        go.GetComponentInChildren<SpriteRenderer>().color = Color.red; 
        yield return new WaitForSeconds(0.1f);
        go.GetComponentInChildren<SpriteRenderer>().color= originalColour;
        StopCoroutine("EnemyFlash");
    }
}
