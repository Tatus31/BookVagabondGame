using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetSelectedEnemyPosition()
    {
        return transform.position;
    }
}
