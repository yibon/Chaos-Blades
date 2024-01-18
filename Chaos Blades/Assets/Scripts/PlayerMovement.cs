using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontalIP;
    float verticalIP;

    public float playerSpeed;

    Vector2 direction;

    Rigidbody2D rb;

    int attackSpellIndex = 0;
    int protectionSpellIndex = 0;

    float spellChargeTimer = 0;

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
            }
            else
            {
                attackSpellIndex = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.E)) //attack
        {
            //change spell icon
            //change skill to other skill
            if (protectionSpellIndex == 0)
            {
                protectionSpellIndex = 1;
            }
            else
            {
                protectionSpellIndex = 0;
            }
        }

        #endregion

        ChargeSpell();
    }

    private void FixedUpdate()
    {
        this.rb.AddForce(direction.normalized * playerSpeed);
    }
    #region To be Removed
    private void Spell1() // attackspellindex 0
    {
        //reduces HP, Start cooldown
    }
    private void Spell2() // attackspellindex 1
    {
        //reduces HP, Start cooldown
    }
    private void Spell3() // protectspellindex 0
    {
        //increase HP, Start cooldown
    }
    private void Spell4() //  protectspellindex 1
    {
        //create barrier, Start cooldown
    }
    #endregion

    public void ChargeSpell()
    {
        if (Input.GetMouseButton(0)) //attacking with LMB
        {
            spellChargeTimer += Time.deltaTime;
            if (Input.GetMouseButtonUp(0))
            {
                if (spellChargeTimer >= 0 && spellChargeTimer <= 1) //if timer more than 0 and less than equal 1, smol charge
                {
                    //mana, damage and range up
                }
                else if (spellChargeTimer > 1 && spellChargeTimer <= 2) //if timer more than 1 and less than equal 2, med charge
                {
                    //mana, damage and range up
                }
                else // timer more than 2, beeg charge
                {
                    //mana, damage and range up
                }
                spellChargeTimer = 0;
            }
        }
        else if (Input.GetMouseButton(1)) //healing with RMB
        {
            spellChargeTimer += Time.deltaTime;
            if (Input.GetMouseButtonUp(1))
            {
                spellChargeTimer = 0;
            }
        }
    }
}