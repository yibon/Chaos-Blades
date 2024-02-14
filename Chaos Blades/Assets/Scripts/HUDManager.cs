using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] TMP_Text kinghp_text;
    [SerializeField] TMP_Text playerMana_text;

    [SerializeField] Sprite pewPewCard;
    [SerializeField] Sprite boomBoomCard;
    [SerializeField] Sprite helfCard;
    [SerializeField] Sprite proteccCard;

    [SerializeField] Image attaccCard;
    [SerializeField] Image defenceCard;

    [SerializeField] KingAI _king;
    [SerializeField] PlayerMovement _player;

    void Update()
    {
        kinghp_text.text = "King's HP: " + _king.hp;
        playerMana_text.text = "Player's Mana: " + _player.currMana;

        switch (_player.attackSpellIndex)
        {
            case 0:
                attaccCard.sprite = pewPewCard;
                break;

            case 1:
                attaccCard.sprite = boomBoomCard;
                break;
        }


        switch (_player.protectionSpellIndex)
        {
            case 0:
                defenceCard.sprite = helfCard;
                break;

            case 1:
                defenceCard.sprite = proteccCard;
                break;
        }

    }
}
