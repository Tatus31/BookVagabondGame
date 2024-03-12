using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class ClassCharacter : ScriptableObject
{
    public string Name;
    public string Description;

    public int Mana;
    public int Health;
    public int AttackSpeed;
}
