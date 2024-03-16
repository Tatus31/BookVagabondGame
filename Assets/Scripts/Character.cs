using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{
    public static Character Instance;

    private GridSystem gridSystem;

    private void Awake()
    {
        Instance = this;

        //Make a script to hold them all!!!
        gridSystem = GameObject.FindGameObjectWithTag("Grid").gameObject.GetComponent<GridSystem>();

        if (gridSystem != null)
        {
            SetCharacterOffset();
        }
        else
        {
            Debug.LogError("GridSystem is not assigned to Character.");
        }
    }

    private void Start()
    {
        SetCharacterOffset();
    }

    public Vector3 GetCharacterPosition()
    {
        Vector3 characterPosition = transform.position;
        return characterPosition;
    }

    private void SetCharacterOffset()
    {
        transform.position = new Vector3(transform.position.x, gridSystem.TileOffset, transform.position.z);
    }

    //Count Characters in Scene
    public int CountCharactersInScene(string tag)
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag(tag);
        return characters.Length;
    }
}
