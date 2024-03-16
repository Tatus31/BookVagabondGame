using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMG_Kalk : MonoBehaviour
{
    [SerializeField]
    int DMG_Gracz;
    [SerializeField]
    int DMG_Wrog;
    [SerializeField]
    int Hp_Gracz;
    [SerializeField]
    int Hp_Wrog;
    [SerializeField]
    GameObject Sfera;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame

    void Update()
    {
        if (Hp_Gracz > 0 & Hp_Wrog > 0 & DMG_Gracz > DMG_Wrog)
        {
            //Hp_Wrog = Hp_Wrog - DMG_Gracz;
            transform.position = new Vector3(11, 3, 5);

        }
        else if (Hp_Gracz > 0 & Hp_Wrog > 0 & DMG_Wrog > DMG_Gracz)
        {
            //Hp_Gracz = Hp_Gracz - DMG_Wrog;
            transform.position = new Vector3(11, 3, 12);
        }

    }
}
