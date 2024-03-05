using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class EnemyBulletScript : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public Collider2D c2D;
    float attack;
    float speed = 10f;
    GameObject King;
    // Start is called before the first frame update
    void Start()
    {
        //setting the componets and values
        c2D = this.GetComponent<Collider2D>();
        rb2D = this.GetComponent <Rigidbody2D>();
        attack = this.GetComponentInParent<EnemyAI>().attack;

        //bullet travel to king
        King = GameObject.FindWithTag("King");
        Vector2 moveDirection = (King.transform.position - transform.position).normalized * speed;
        rb2D.velocity = new Vector2(moveDirection.x, moveDirection.y);
        
        //destroy after 1 second if bullet cannot find king
        Destroy(this.gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("King"))
        {
            //damage to king
            KingAI _king = collision.gameObject.GetComponentInParent<KingAI>();
            _king.hp = _king.hp - attack;

            //destroy after hitting king
            Destroy(this.gameObject);
        }
    }
}