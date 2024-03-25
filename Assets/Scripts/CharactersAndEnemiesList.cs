using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersAndEnemiesList : MonoBehaviour
{
    public static CharactersAndEnemiesList Instance;

    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> characters = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }
}
