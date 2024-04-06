using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSpeed : MonoBehaviour
{
    public static CharacterSpeed Instance;

    public int characterSpeed;

    private int minSpeed = 1;
    private int maxSpeed = 5;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SetCharacterSpeed();
    }

    public void SetCharacterSpeed()
    {
        characterSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed + 1);
        Debug.Log($"Character Speed set to: {characterSpeed}");
    }

    public static void UpdateSpeedsOnAllObjects()
    {
        CharacterSpeed[] characterSpeedScripts = FindObjectsOfType<CharacterSpeed>();

        foreach (CharacterSpeed script in characterSpeedScripts)
        {
            script.SetCharacterSpeed();
        }
    }

    public void OnButtonClick()
    {
        UpdateSpeedsOnAllObjects();
    }

    public int GetCharacterSpeed()
    {
        return characterSpeed;
    }
}
