
using UnityEngine;

public class Boolet : MonoBehaviour
{
    // Start is called before the first frame update
    // 0- smoll pew, 1- beeg boom, 2- helf, 3- protecc
    public int currSpell;

    float bulletAmt;
    // positive for healing, negative for damage, 0 for nullifying
    int healthMultiplier;

    void Start()
    {
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
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("knock knock who's there" +  collision.gameObject.tag);
        // * note need to use GetComponentInParent() * // 
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyAI _enemy = collision.gameObject.GetComponentInParent<EnemyAI>();
            _enemy.hp = _enemy.hp + (healthMultiplier * bulletAmt);
            Debug.Log("Enemy Helf: " + _enemy.hp);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("King"))
        {
            KingAI _king = collision.gameObject.GetComponentInParent<KingAI>();
            _king.hp = _king.hp + (healthMultiplier * bulletAmt);
            Debug.Log("Enemy Helf: " + _king.hp);
            Destroy(this.gameObject);
        }
    }
}
