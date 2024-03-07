using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    float horizontalIP;
    float verticalIP;

    //Player Stats
    public float currMana = 100;
    float manaRegenRate = 4;
    public List<float> attackSpellCooldownList = new List<float>();
    public List<float> proteccSpellCooldownList = new List<float>();
    float attackSpell1CD = 0;
    float attackSpell2CD = 0;
    float proteccSpell1CD = 0;
    float proteccSpell2CD = 0;
    bool castingAttack = false;
    bool castingProtecc = false;

    float moveSpeed = 5f;

    public float playerSpeed;

    Vector2 direction;

    Rigidbody2D rb;

    // 0 - small pew, 1- big boom
    public int attackSpellIndex = 0;
    // o - helf, 1- protecc
    public int protectionSpellIndex = 0;

    float spellChargeTimer = 0;
    public Image barImage;

    [Header("SHOOTING")] 
    // Singleton this? 
    public Transform firePoint;
    public float bulletForce = 20f;
    public GameObject[] attBulletPF;
    public GameObject[] defBulletPF;
    private void Awake()
    {
        attackSpellCooldownList.Add(attackSpell1CD);
        attackSpellCooldownList.Add(attackSpell2CD);
        proteccSpellCooldownList.Add(proteccSpell1CD);
        proteccSpellCooldownList.Add(proteccSpell2CD);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontalIP = Input.GetAxisRaw("Horizontal");
        verticalIP = Input.GetAxisRaw("Vertical");

        direction = new Vector2(horizontalIP, verticalIP);

        

        #region choosing spell
        if (Input.GetKeyDown(KeyCode.Q)) //attack
        {
            //change spell icon
            //change skill to other skill
            if (attackSpellIndex == 0) 
            {
                attackSpellIndex = 1;
                Debug.Log(attackSpellIndex);
            }
            else
            {
                attackSpellIndex = 0;
                Debug.Log(attackSpellIndex);
            }
        }
        if (Input.GetKeyDown(KeyCode.E)) //attack
        {
            //change spell icon
            //change skill to other skill
            if (protectionSpellIndex == 0)
            {
                protectionSpellIndex = 1;
                Debug.Log(protectionSpellIndex);
            }
            else
            {
                protectionSpellIndex = 0;
                Debug.Log(protectionSpellIndex);
            }
        }

        #endregion

        if (castingAttack == false && castingProtecc == false) //no casting taking place
        {
            ChargeProteccSpell(protectionSpellIndex);
            ChargeAttaccSpell(attackSpellIndex);
        }
        else if (castingAttack == true && castingProtecc == false)
        {
            ChargeAttaccSpell(attackSpellIndex);
        }
        else if (castingProtecc == true && castingAttack == false)
        {
            ChargeProteccSpell(protectionSpellIndex);
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        
        #region mana regen per secon
        currMana += manaRegenRate * Time.deltaTime;
        if (currMana > 100)
        {
            currMana = 100;
        }
        #endregion
    }

    public void ChargeProteccSpell(int proteccIndex)
    {
        float ManaCost = SpellManager.instance.proteccSpells[proteccIndex].manaCost;
        if (ManaCost + 2 <= currMana && proteccSpellCooldownList[proteccIndex] == 0) //checking spell is castable
        {
            #region Checking Mouse press
            if (Input.GetMouseButton(1)) //healing with RMB
            {
                spellChargeTimer += Time.deltaTime;
                barImage.fillAmount = spellChargeTimer / 3;
                
                castingProtecc = true;
                Debug.Log("protecc Holding");
            }
            else if (Input.GetMouseButtonUp(1))
            {
                //setting CD
                proteccSpellCooldownList[proteccIndex] = SpellManager.instance.proteccSpells[proteccIndex].cooldown;
                if (spellChargeTimer >= 0 && spellChargeTimer <= 1) //if timer more than 0 and less than equal 1, basic (Smol) charge
                {
                    Shoot(1, proteccIndex);
                    Debug.Log("Smol " + SpellManager.instance.proteccSpells[proteccIndex].Name); //replace with instantiation code
                }
                else if (spellChargeTimer > 1 && spellChargeTimer <= 2) //if timer more than 1 and less than equal 2, med charge
                {
                    Shoot(1, proteccIndex);
                    Debug.Log("Med " + SpellManager.instance.proteccSpells[proteccIndex].Name); //replace with instantiation code
                    proteccSpellCooldownList[proteccIndex] += 1;
                    ManaCost += 1;
                }
                else // timer more than 2, beeg charge
                {
                    Shoot(1, proteccIndex);
                    Debug.Log("Beeg " + SpellManager.instance.proteccSpells[proteccIndex].Name); //replace with instantiation code
                    proteccSpellCooldownList[proteccIndex] += 2;
                    ManaCost += 2;
                }
                spellChargeTimer = 0;
                castingProtecc = false;
                currMana -= ManaCost;
                Debug.Log(SpellManager.instance.proteccSpells[proteccIndex].Name + " casted, left Mana: " + currMana);
                
            }
            #endregion
        }
        else if (proteccSpellCooldownList[proteccIndex] > 0) //if spell is casted
        {
            proteccSpellCooldownList[proteccIndex] -= Time.deltaTime;

            if (proteccSpellCooldownList[proteccIndex]<=0)
                proteccSpellCooldownList[proteccIndex] = 0;
            
            if (Input.GetMouseButtonDown(1))
            {
                //for now debug, later change this to visual prompt
                Debug.Log(SpellManager.instance.proteccSpells[proteccIndex].Name + " on CD, wait " + proteccSpellCooldownList[proteccIndex]);
            }
        }
        else if (ManaCost + 2 > currMana) //if mana not enough
        {
            if (Input.GetMouseButtonDown(1))
            {
                //for now debug, later change this to visual prompt
                Debug.Log("No Mana!");
            }
        }
    }

    public void ChargeAttaccSpell(int attackIndex)
    {
        float ManaCost = SpellManager.instance.attackSpells[attackIndex].manaCost;
        if (ManaCost + 2 <= currMana && attackSpellCooldownList[attackIndex] == 0) //checking spell CD
        {
            #region Checking Mouse pres
            if (Input.GetMouseButton(0)) //attacking with LMB
            {
                spellChargeTimer += Time.deltaTime;
                castingAttack = true;
                Debug.Log("Attack Holding");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //setting cooldown
                attackSpellCooldownList[attackIndex] = SpellManager.instance.attackSpells[attackIndex].cooldown;
                
                //if timer more than 0 and less than equal 1, basic (Smol) charge
                if (spellChargeTimer >= 0 && spellChargeTimer < 1) 
                {
                    //replace with instantiation code
                    Shoot(0, attackIndex);
                    Debug.Log("Smol " + SpellManager.instance.attackSpells[attackIndex].Name); 
                }
                
                //if timer more than 1 and less than equal 2, med charge
                else if (spellChargeTimer >= 1 && spellChargeTimer < 2) 
                {
                    //replace with instantiation code
                    Debug.Log("Med " + SpellManager.instance.attackSpells[attackIndex].Name);
                    Shoot(0, attackIndex);
                    attackSpellCooldownList[attackIndex] += 1;
                    ManaCost += 1;
                }
                else // timer more than 2, beeg charge
                {
                    //replace with instantiation code
                    Debug.Log("Beeg " + SpellManager.instance.attackSpells[attackIndex].Name);
                    Shoot(0, attackIndex);
                    attackSpellCooldownList[attackIndex] += 2;
                    ManaCost += 2;
                }
                spellChargeTimer = 0;
                castingAttack = false;
                currMana -= ManaCost;
                Debug.Log(SpellManager.instance.attackSpells[attackIndex].Name + " casted, left Mana: " + currMana);
            }
            #endregion
        }

        else if (attackSpellCooldownList[attackIndex]>0) //if spell is casted
        {
            attackSpellCooldownList[attackIndex] -= Time.deltaTime;

            if (attackSpellCooldownList[attackIndex] <= 0)
                attackSpellCooldownList[attackIndex] = 0;
            
            if (Input.GetMouseButton(0))
            {
                //for now debug, later change this to visual prompt
                Debug.Log(SpellManager.instance.attackSpells[attackIndex].Name + " on CD, wait " + attackSpellCooldownList[attackIndex]);
            }
            
        }
        else if (ManaCost + 2 > currMana) //if mana not enough
        {
            if (Input.GetMouseButtonDown(0))
            {
                //for now debug, later change this to visual prompt
                Debug.Log("No Mana!");
            }
        }
    }

    // SHOOTING 
    //               0- attack, 1- defence
    public void Shoot(int attOrDef, int spellIndex)
    {
        if (attOrDef == 0)
        {
            //spawn bullet as attack spell
            GameObject bullet = Instantiate(attBulletPF[spellIndex], firePoint.position, firePoint.rotation);
            
            //set attackspell bullet rb and launch it
            Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
            bullet_rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            
            //setting the bullet script
            Boolet _boolet = bullet.GetComponent<Boolet>();
            
            //checking which spell
            switch (spellIndex)
            {
                case 0:
                    _boolet.currSpell = 0;
                    break;
                case 1:
                    _boolet.currSpell = 1;
                    break;
            }
        }
        else if(attOrDef == 1) 
        {
            //spawn bullet as def spell
            GameObject bullet = Instantiate(defBulletPF[spellIndex], firePoint.position, firePoint.rotation);
            
            //set defspell bullet rb and launch it
            Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
            bullet_rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            
            //setting the bullet script
            Boolet _boolet = bullet.GetComponent<Boolet>();
            
            //checking which spell
            switch (spellIndex)
            {
                case 0:
                    _boolet.currSpell = 2;
                    break;
                case 1:
                    _boolet.currSpell = 3;
                    break;
            }
            
        }
    }
}