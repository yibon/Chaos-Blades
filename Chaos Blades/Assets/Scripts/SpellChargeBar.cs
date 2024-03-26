using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellChargeBar : MonoBehaviour
{
    [SerializeField] public Slider ChargeBar;

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

    public void UpdateChargeBar(float currCharge, float maxCharge)
    {
        ChargeBar.value = currCharge / maxCharge;
    }

}
