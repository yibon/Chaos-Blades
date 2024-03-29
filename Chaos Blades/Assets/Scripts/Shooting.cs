
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    Vector3 mousePos;
    //public GameObject bullet;
    public Transform bulletTransform;
    public static bool canAttack;
    public static bool canProtect;


    public GameObject[] attBulletPF;
    public GameObject[] defBulletPF;

    SpriteRenderer bullet_sr;

    //public static int attOrDef;
    //public static int spellIndex;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        bullet_sr = bulletTransform.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("faiyahhh" + canAttack);
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (PlayerMovement.playerFlipped)
        {
            bullet_sr.flipY = true;
        }

        else
        {
            bullet_sr.flipY = false;
        }
    }

    //SHOOTING
    //               0- attack, 1- defence
    public void Shoot(int attOrDef, int spellIndex)
    {
        if (attOrDef == 0)
        {
            //spawn bullet as attack spell
            GameObject bullet = Instantiate(attBulletPF[spellIndex], bulletTransform.position, Quaternion.identity);
            //GameObject bullet = Instantiate(attBulletPF[spellIndex], firePoint.position, firePoint.rotation);

            //set attackspell bullet rb and launch it

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
        else if (attOrDef == 1)
        {
            //spawn bullet as def spell
            GameObject bullet = Instantiate(defBulletPF[spellIndex], bulletTransform.position, Quaternion.identity);
            //GameObject bullet = Instantiate(defBulletPF[spellIndex], firePoint.position, firePoint.rotation);

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
