using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Camera mainCam;
    Vector3 mousePos;

    float horizontalIP;
    float verticalIP;

    //Player Stats
    public float currMana = 100;
    float manaRegenRate = 0.5f;
    public List<float> attackSpellCooldownList = new List<float>();
    public List<float> proteccSpellCooldownList = new List<float>();
    float attackSpell1CD = 0;
    float attackSpell2CD = 0;
    float proteccSpell1CD = 0;
    float proteccSpell2CD = 0;
    bool castingAttack = false;
    bool castingProtecc = false;

    public Animator animator;

    float moveSpeed = 5f;

    public float playerSpeed;

    Vector2 direction;

    Rigidbody2D rb;

    // 0 - small pew, 1- big boom
    public int attackSpellIndex = 0;
    // o - helf, 1- protecc
    public int protectionSpellIndex = 0;

    float spellChargeTimer = 0;
    float maxspellchargetimer = 3;
    public Image barImage;

    Shooting _shooting;

    SpriteRenderer _sr;
    public static bool playerFlipped;

    //[Header("SHOOTING")] 
    //// Singleton this? 
    //public Transform firePoint;
    //public float bulletForce = 20f;
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
        _shooting = gameObject.GetComponentInChildren<Shooting>();
        _sr = GetComponent<SpriteRenderer>();

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalIP = Input.GetAxisRaw("Horizontal");
        verticalIP = Input.GetAxisRaw("Vertical");

        direction = new Vector2(horizontalIP, verticalIP);

        animator.SetFloat("Speed", direction.magnitude);


        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < transform.position.x)
        {
            _sr.flipX = true;
            playerFlipped = true;
        }

        else
        {
            _sr.flipX = false;
            playerFlipped = false;
        }

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
                #region Playing Sounds
                if (proteccIndex == 0)
                {
                    AudioManager.instance.Play("HelfSFX");
                }

                else
                {
                    AudioManager.instance.Play("ShieldSFX");
                }
                #endregion

                spellChargeTimer += Time.deltaTime;
                SpellChargeBar.Instance.UpdateChargeBar(spellChargeTimer, maxspellchargetimer);

                castingProtecc = true;
                Debug.Log("protecc Holding");
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Shooting.canProtect = true;
                //setting CD
                proteccSpellCooldownList[proteccIndex] = SpellManager.instance.proteccSpells[proteccIndex].cooldown;
                if (spellChargeTimer >= 0 && spellChargeTimer <= 1) //if timer more than 0 and less than equal 1, basic (Smol) charge
                {
                    _shooting.Shoot(1, proteccIndex);
                    Debug.Log("Smol " + SpellManager.instance.proteccSpells[proteccIndex].Name); //replace with instantiation code
                   
                }
                else if (spellChargeTimer > 1 && spellChargeTimer <= 2) //if timer more than 1 and less than equal 2, med charge
                {
                    _shooting.Shoot(1, proteccIndex);
                    Debug.Log("Med " + SpellManager.instance.proteccSpells[proteccIndex].Name); //replace with instantiation code
                    proteccSpellCooldownList[proteccIndex] += 1;
                    ManaCost += 1;
                }
                else // timer more than 2, beeg charge
                {
                    _shooting.Shoot(1, proteccIndex);
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

        SpellChargeBar.Instance.UpdateChargeBar(spellChargeTimer, maxspellchargetimer);
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
                SpellChargeBar.Instance.UpdateChargeBar(spellChargeTimer, maxspellchargetimer);
                Debug.Log("Attack Holding");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //setting cooldown
                attackSpellCooldownList[attackIndex] = SpellManager.instance.attackSpells[attackIndex].cooldown;
                Shooting.canAttack = true;

                #region Playing Sounds
                if (attackIndex == 0)
                {
                    AudioManager.instance.Play("PewPewSFX");
                }

                else
                {
                    AudioManager.instance.Play("BoomBoomSFX");
                }
                #endregion

                //if timer more than 0 and less than equal 1, basic (Smol) charge
                if (spellChargeTimer >= 0 && spellChargeTimer < 1) 
                {
                    _shooting.Shoot(0, attackIndex);
                    Debug.Log("Smol " + SpellManager.instance.attackSpells[attackIndex].Name); 
                }
                
                //if timer more than 1 and less than equal 2, med charge
                else if (spellChargeTimer >= 1 && spellChargeTimer < 2) 
                {
                    //replace with instantiation code
                    Debug.Log("Med " + SpellManager.instance.attackSpells[attackIndex].Name);
                    _shooting.Shoot(0, attackIndex);
                    attackSpellCooldownList[attackIndex] += 1;
                    ManaCost += 1;
                }
                else // timer more than 2, beeg charge
                {
                    //replace with instantiation code
                    Debug.Log("Beeg " + SpellManager.instance.attackSpells[attackIndex].Name);

                    _shooting.Shoot(0, attackIndex);
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

        SpellChargeBar.Instance.UpdateChargeBar(spellChargeTimer, maxspellchargetimer);
    }
}