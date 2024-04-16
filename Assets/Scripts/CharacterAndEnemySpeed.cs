using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterAndEnemySpeed : MonoBehaviour
{
    public static CharacterAndEnemySpeed Instance;

    [SerializeField] private int speed;

    private int minSpeed = 1;
    private int maxSpeed = 5;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SetEntitySpeed();
    }

    public void SetEntitySpeed()
    {
        speed = UnityEngine.Random.Range(minSpeed, maxSpeed + 1);
        //Debug.Log($"Character Speed set to: {speed}");
    }

    public static void UpdateSpeedsOnAllObjects()
    {
        CharacterAndEnemySpeed[] characterSpeedScripts = FindObjectsOfType<CharacterAndEnemySpeed>();

        foreach (CharacterAndEnemySpeed script in characterSpeedScripts)
        {
            script.SetEntitySpeed();
        }
    }

    public void OnButtonClick()
    {
        UpdateSpeedsOnAllObjects();
    }

    public int GetEntitySpeed()
    {
        return speed;
    }
}
