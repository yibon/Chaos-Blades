
using UnityEngine;

public class Boolet : MonoBehaviour
{
    // Start is called before the first frame update
    // 0- smoll pew, 1- beeg boom, 2- helf, 3- protecc
    public int currSpell;

    float bulletAmt;
    // positive for healing, negative for damage, 0 for nullifying
    int healthMultiplier;

    bool isProtected;

    Vector3 mousePos;
    Camera mainCam;

    private Rigidbody2D rb;
    public float force;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;

        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0 ,rot + 90);


        Debug.Log("bullet kaboomz" + currSpell);
        switch (currSpell)
        {
            // smoll pew pew
            case 0:
                healthMultiplier = -1;
                bulletAmt = SpellManager.instance.attackSpells[0].damageDealt;
                break;
            // beeg boom
            case 1:
                healthMultiplier = -1;
                bulletAmt = SpellManager.instance.attackSpells[1].damageDealt;
                break;
            
            // helf
            case 2:
                healthMultiplier = 1;
                bulletAmt = SpellManager.instance.proteccSpells[0].healthGain;
                break;
            
            // protecc
            case 3:
                healthMultiplier = 0;
                bulletAmt = SpellManager.instance.proteccSpells[1].damageReduce;
                break;
        }
        Debug.Log("i need more bulletsssssss" + healthMultiplier);
        Destroy(this.gameObject,5f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("knock knock who's there" +  collision.gameObject.tag);
        // * note need to use GetComponentInParent() * // 
        if (collision.transform.root.CompareTag("Enemy"))
        {
            EnemyAI _enemy = collision.gameObject.GetComponentInParent<EnemyAI>();
            if (currSpell == 3)
            {
                _enemy.isProtected = true;
            }

            if (_enemy.isProtected)
            {
                healthMultiplier = 0;
            }

            _enemy.hp = _enemy.hp + (healthMultiplier * bulletAmt);
            Debug.Log("Enemy Helf: " + _enemy.hp);
            Destroy(this.gameObject);
            
        }

        if (collision.gameObject.CompareTag("King"))
        {
            KingAI _king = collision.gameObject.GetComponentInParent<KingAI>();

            if (currSpell == 3)
            {
                _king.kingProtected = true;
            }

            if (_king.kingProtected)
            {
                healthMultiplier = 0;
            }

            _king.hp = _king.hp + (healthMultiplier * bulletAmt);
            Debug.Log("Enemy Helf: " + _king.hp);
            Destroy(this.gameObject);
        }
    }
}
