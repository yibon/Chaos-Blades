
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KingAI : MonoBehaviour
{
    [SerializeField] public float hp;
    [SerializeField] float speed;
    [SerializeField] public float attack;
    [SerializeField] float attackSpeed;
    [SerializeField] float range;

    float maxHP = 100;
    [SerializeField] public Slider HPSlider;

    [HideInInspector]public bool kingIsHit = false;

    public Rigidbody2D rb2D;
    public Collider2D attackCollider2D;

    public bool kingProtected;
    public GameObject protectedSprite;

    float protectionTimer;
    AudioManager _am;

    //private bool halfPlayedOnce;
    private bool quarterPlayedOnce;

    // Update is called once per frame
    void Update()
    {
        hp = Mathf.Clamp(hp, 0, 100);
        HPSlider.value = hp / maxHP;

        // KING HEALTH CHEATCODE
        //if (Input.GetKeyDown(KeyCode.O)) {
        //    hp += 10;
        //}

        #region PLAYING SOUNDS

        //if (hp < 50 && !halfPlayedOnce)
        //{
        //    AudioManager.instance.Play("King50Health");
        //    halfPlayedOnce = true;
        //}

        // If king is healed > 25%
        if (hp > 25)
        {
            quarterPlayedOnce = false;
        }    


        if (hp < 25 && !quarterPlayedOnce)
        {
            AudioManager.instance.Play("King10Health");
            quarterPlayedOnce = true;
        }


        #endregion

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
            AudioManager.instance.Play("KingHit");
            StartCoroutine("HitFlash", this.gameObject);
            kingIsHit= false;
        }


        //constantly attacking

        //constantly looking for enemy
        //replace the obj searching with a list that is genereated from spawner.cs
        if (GameObject.FindGameObjectWithTag("Enemy") == true)
        {
            Vector3 forceApplied = GameObject.FindGameObjectWithTag("Enemy").transform.position - this.transform.position;
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

            // put in the game over screen here
            // can start the calculations for score now
            SceneManager.LoadScene(2);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag ("Enemy Support"))
        {
            //AudioManager.instance.Play("KingSwing");
            if (!collision.gameObject.GetComponent<EnemyAI>().isProtected)
            {
                #region PLAY SOUNDS

                if (collision.transform.root.name == "EnemyMelee(Clone)")
                {
                    AudioManager.instance.Play("SlimeHurt");
                }
                if (collision.transform.root.name == "EnemyRanged(Clone)")
                {
                    AudioManager.instance.Play("WispHurt");
                }

                if (collision.transform.root.name == "EnemySupport(Clone)")
                {
                    AudioManager.instance.Play("ShamanHurt");
                }

                if (collision.transform.root.name == "EnemyTank(Clone)")
                {
                    AudioManager.instance.Play("GolemHurt");
                }
                #endregion

                collision.gameObject.GetComponent<EnemyAI>().hp -= attack;
                collision.gameObject.GetComponent<EnemyAI>().enemyIsHit = true;
                collision.gameObject.GetComponent<EnemyAI>().healthBar.UpdateHealthBar(collision.gameObject.GetComponent<EnemyAI>().hp, collision.gameObject.GetComponent<EnemyAI>().maxhp);
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
        go.GetComponentInChildren<SpriteRenderer>().color = originalColour;
        StopCoroutine("HitFlash");
    }
}
