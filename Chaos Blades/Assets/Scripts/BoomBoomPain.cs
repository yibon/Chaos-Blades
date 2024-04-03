using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBoomPain : MonoBehaviour
{
    public Collider2D Collider;

    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //collision not player and is king
        if (collision.gameObject.CompareTag("King"))
        {
            if (!collision.gameObject.GetComponentInChildren<KingAI>().kingProtected) 
            { 
                collision.gameObject.GetComponentInChildren<KingAI>().hp -= 3;
                collision.gameObject.GetComponent<KingAI>().kingIsHit = true;
            }

        }

        //collision not player and is enemy (both)
        if (collision.gameObject.CompareTag("Enemy Support") || collision.gameObject.CompareTag("Enemy"))
        {
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

                collision.gameObject.GetComponent<EnemyAI>().hp -= 3;
                collision.gameObject.GetComponent<EnemyAI>().enemyIsHit = true;
                collision.gameObject.GetComponent<EnemyAI>().healthBar.UpdateHealthBar(collision.gameObject.GetComponent<EnemyAI>().hp, collision.gameObject.GetComponent<EnemyAI>().maxhp);
            }
        }

    }
}
