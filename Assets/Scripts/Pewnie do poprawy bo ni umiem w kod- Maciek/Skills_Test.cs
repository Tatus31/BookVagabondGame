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
    int def_Add = 5;
    int block = 7;
    //Defensywne
    public void Defend()
    {
        hp = hp + def_Add;
    }

    public void Block_Atk()
    {

    }
    public void Doge_Atk() 
    {
    
    }
    //Ofensywne
    public void Atack()
    {

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
