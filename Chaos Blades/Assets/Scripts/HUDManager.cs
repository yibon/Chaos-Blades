
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    [SerializeField] TMP_Text attackCD_text;
    [SerializeField] TMP_Text defCD_text;

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
        attackCD_text.text = "Attack CD: " + _player.attackSpellCooldownList[_player.attackSpellIndex].ToString("#.00");
        defCD_text.text = "Defence CD: " + _player.proteccSpellCooldownList[_player.protectionSpellIndex].ToString("#.00");

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
