using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMG_Kalk : MonoBehaviour
{
    [SerializeField]
    int dmg_Gracz;
    [SerializeField]
    int dmg_Wrog;
    [SerializeField]
    int hp_Gracz;
    [SerializeField]
    int hp_Wrog;
    [SerializeField]
    GameObject sfera;

    [SerializeField]
    HealthManager healthManager;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame

    void Update()
    {
        if (hp_Gracz > 0 && hp_Wrog > 0 && dmg_Gracz > dmg_Wrog)
        {
            //Hp_Wrog = Hp_Wrog - DMG_Gracz;
            transform.position = new Vector3(11, 3, 5);
            

        }
        else if (hp_Gracz > 0 && hp_Wrog > 0 & dmg_Wrog > dmg_Gracz)
        {
            //Hp_Gracz = Hp_Gracz - DMG_Wrog;
            transform.position = new Vector3(11, 3, 12);
            //healthManager.TakeDamage();

        }

    }
}
