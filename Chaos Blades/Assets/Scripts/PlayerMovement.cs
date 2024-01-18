using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontalIP;
    float verticalIP;

    //Player Stats
    float currMana = 10;
    public List<float> attackSpellCooldownList = new List<float>();
    public List<float> proteccSpellCooldownList = new List<float>();
    float attackSpell1CD = 0;
    float attackSpell2CD = 0;
    float proteccSpell1CD = 0;
    float proteccSpell2CD = 0;
    bool castingAttack = false;
    bool castingProtecc = false;

    public float playerSpeed;

    Vector2 direction;

    Rigidbody2D rb;

    int attackSpellIndex = 0;
    int protectionSpellIndex = 0;

    float spellChargeTimer = 0;
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

        #region temp mana increase
        if (Input.GetKeyDown(KeyCode.M)) { currMana = 10; Debug.Log(currMana); }
        #endregion

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
        this.rb.AddForce(direction.normalized * playerSpeed);
    }

    public void ChargeProteccSpell(int proteccIndex)
    {
        float ManaCost = SpellManager.instance.attackSpells[proteccIndex].manaCost;
        if (ManaCost + 2 <= currMana && proteccSpellCooldownList[proteccIndex] == 0) //checking spell is castable
        {
            #region Checking Mouse press
            if (Input.GetMouseButton(1)) //healing with RMB
            {
                spellChargeTimer += Time.deltaTime;
                castingProtecc = true;
                Debug.Log("protecc Holding");
            }
            else if (Input.GetMouseButtonUp(1))
            {
                //setting CD
                proteccSpellCooldownList[proteccIndex] = SpellManager.instance.proteccSpells[proteccIndex].cooldown;
                if (spellChargeTimer >= 0 && spellChargeTimer <= 1) //if timer more than 0 and less than equal 1, basic (Smol) charge
                {   
                    Debug.Log(proteccIndex + "smol");
                }
                else if (spellChargeTimer > 1 && spellChargeTimer <= 2) //if timer more than 1 and less than equal 2, med charge
                {
                    Debug.Log(proteccIndex + "Meed");
                    proteccSpellCooldownList[proteccIndex] += 1;
                    ManaCost += 1;
                }
                else // timer more than 2, beeg charge
                {
                    Debug.Log(proteccIndex + "Beeg");
                    proteccSpellCooldownList[proteccIndex] += 2;
                    ManaCost += 2;
                }
                spellChargeTimer = 0;
                castingProtecc = false;
                currMana -= ManaCost;
                Debug.Log("Protect casted, left Mana: " + currMana);
                
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
                Debug.Log(SpellManager.instance.attackSpells[proteccIndex].ToString() +" on CD, wait " + proteccSpellCooldownList[proteccIndex]);
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
                if (spellChargeTimer >= 0 && spellChargeTimer < 1) //if timer more than 0 and less than equal 1, basic (Smol) charge
                {
                    Debug.Log(attackIndex + "smol");
                }
                else if (spellChargeTimer >= 1 && spellChargeTimer < 2) //if timer more than 1 and less than equal 2, med charge
                {
                    Debug.Log(attackIndex + "Meed");
                    attackSpellCooldownList[attackIndex] += 1;
                    ManaCost += 1;
                }
                else // timer more than 2, beeg charge
                {
                    Debug.Log(attackIndex + "Beeg");
                    attackSpellCooldownList[attackIndex] += 2;
                    ManaCost += 2;
                }
                spellChargeTimer = 0;
                castingAttack = false;
                currMana -= ManaCost;
                Debug.Log("Attack casted, left Mana: " + currMana);
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
                Debug.Log(SpellManager.instance.attackSpells[attackIndex].ToString() + " on CD, wait " + attackSpellCooldownList[attackIndex]);
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

   
}