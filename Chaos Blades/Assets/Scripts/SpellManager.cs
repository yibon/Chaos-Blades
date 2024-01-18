using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager instance { get; private set; }

    public List<Spells> attackSpells = new List<Spells>();
    public List<Spells> proteccSpells = new List<Spells>();

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
        Spells healing = new Spells(5, 0, 0, 1, 1, 1, 5, "string");
        Spells proteccTheAttacc = new Spells(0, 5, 0, 1, 1, 2, 5, "string");
        Spells smolPewPew = new Spells(0, 0, 5, 1, 1, 1, 5, "string");
        Spells beegBoomBoom = new Spells(0, 0, 5, 1, 1, 1, 5, "string");

        attackSpells.Add(smolPewPew);
        attackSpells.Add(beegBoomBoom);
        proteccSpells.Add(healing);
        proteccSpells.Add(proteccTheAttacc);
        #endregion
    }

}

//Mainly used to compare original value and set the dynamic values for the spell used. rarely will use these numbers directly.
public struct Spells
{
    public float healthGain;
    public float damageReduce;
    public float damageDealt;
    public float radius;
    public float fireRate;
    public float manaCost;
    public float cooldown;
    public string flavourText;

    public Spells(float healthGain, float damageReduce, float damageDealt, float radius, float fireRate, float manaCost, float cooldown, string flavourText)
    {
        this.healthGain = healthGain;
        this.damageReduce = damageReduce;
        this.damageDealt = damageDealt;
        this.radius = radius;
        this.fireRate = fireRate;
        this.manaCost = manaCost;
        this.cooldown = cooldown;
        this.flavourText = flavourText;
    }
}
