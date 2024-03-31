using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotScriprt : MonoBehaviour
{
    public Collider2D collider;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().currMana < 100)
            {
                collision.gameObject.GetComponent<PlayerMovement>().currMana += 10;
                if (collision.gameObject.GetComponent<PlayerMovement>().currMana > 100)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().currMana = 100;
                }
                Destroy(this.gameObject);
            }
        }
    }
}
