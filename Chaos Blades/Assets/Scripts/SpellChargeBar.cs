using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellChargeBar : MonoBehaviour
{
    [SerializeField] public Slider AttackChargeBar;
    [SerializeField] public Slider ProtectChargeBar;


    #region Singleton Code
    public static SpellChargeBar Instance { get; private set; }


    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    public void UpdateAttackChargeBar(float currCharge, float maxCharge)
    {
        AttackChargeBar.value = currCharge / maxCharge;
    }
    
    public void UpdateProtectChargeBar(float currCharge, float maxCharge)
    {
        ProtectChargeBar.value = currCharge / maxCharge;
    }

}
