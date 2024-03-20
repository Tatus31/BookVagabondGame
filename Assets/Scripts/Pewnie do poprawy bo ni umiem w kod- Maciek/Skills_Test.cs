using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills_Test : MonoBehaviour
{
    //Dane zależne od postaci
    [SerializeField]
    int hp;
    [SerializeField]
    int dmg;
    [SerializeField]
    int energia;
    //Ikonki??? Nwm pwenie da sie lepiej
    [SerializeField]
    GameObject Ikona_Atk;
    [SerializeField]
    GameObject Ikona_Def;
    [SerializeField]
    GameObject Ikona_Dge;
    [SerializeField]
    GameObject Ikona_Bck;
    [SerializeField]
    GameObject Ikona_ASp;

    //Dane niezmienne
    int hp_Wroga_TestOnly = 10;
    int atak_Wroga_TestOnly= 4;
    int def_Add = 2;
    int block = 3;
    //Defensywne
    public void Defend()
    {
        hp = hp + def_Add;
        energia -= 2;
        
    }

    public void Block_Atk()
    {
        hp = hp - atak_Wroga_TestOnly + block;
        energia -= 3;
    }
    //po huj to robie i tak nie umiem
    public void Doge_Atk() 
    {
        energia -= 1;
        int los = Random.Range(1,10);
        if (los % 2 == 1)
        {
            hp = hp - atak_Wroga_TestOnly;
        }
    }
    //Ofensywne
    public void Atack()
    {
        hp_Wroga_TestOnly = hp_Wroga_TestOnly - dmg;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
