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
        Spells healing = new Spells("Helt", 5, 0,0,4,8,2.5f, "string");
        Spells proteccTheAttacc = new Spells("Protecc the Attacc" ,0,1,0,1,4,2.5f , "string");
        Spells smolPewPew = new Spells("Smol Pew Pew" ,0,0,8,1,3,0.8f, "string");
        Spells beegBoomBoom = new Spells("Beeg Boom Boom", 0,0,3,4,3,0.8f, "string");

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
    public string Name;
    public float healthGain;
    public float damageReduce;
    public float damageDealt;
    public float radius;
    public float manaCost;
    public float cooldown;
    public string flavourText;

    public Spells(string Name, float healthGain, float damageReduce, float damageDealt, float radius, float manaCost, float cooldown, string flavourText)
    {
        this.Name = Name;
        this.healthGain = healthGain;
        this.damageReduce = damageReduce;
        this.damageDealt = damageDealt;
        this.radius = radius;
        this.manaCost = manaCost;
        this.cooldown = cooldown;
        this.flavourText = flavourText;
    }
}
