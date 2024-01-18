using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager instance { get; private set; }

    List<Spells> attackSpells = new List<Spells>();
    List<Spells> proteccSpells = new List<Spells>();

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }

        #region Creating Spells and Adding to list
        Spells healing = new Spells(5, 0, 0, 1, 1, 1, "string");
        Spells proteccTheAttacc = new Spells(0, 5, 0, 1, 1, 2, "string");
        Spells smolPewPew = new Spells(0, 0, 5, 1, 1, 1, "string");
        Spells beegBoomBoom = new Spells(0, 0, 5, 1, 1, 1, "string");

        attackSpells.Add(smolPewPew);
        attackSpells.Add(beegBoomBoom);
        proteccSpells.Add(healing);
        proteccSpells.Add(proteccTheAttacc);
        #endregion
    }

}

struct Spells
{
    public float healthGain;
    public float damageReduce;
    public float damageDealt;
    public float radius;
    public float fireRate;
    public float manaCost;
    public string flavourText;

    public Spells(float healthGain, float damageReduce, float damageDealt, float radius, float fireRate, float manaCost, string flavourText)
    {
        this.healthGain = healthGain;
        this.damageReduce = damageReduce;
        this.damageDealt = damageDealt;
        this.radius = radius;
        this.fireRate = fireRate;
        this.manaCost = manaCost;
        this.flavourText = flavourText;
    }
}