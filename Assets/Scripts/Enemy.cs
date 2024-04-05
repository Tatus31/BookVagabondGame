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
        GameObject enemy = CharacterSelection.Instance.SelectionCheck();
        Vector3 enemyPosition;

        if (enemy.CompareTag("Enemy") && enemy != null)
        {
            enemyPosition = enemy.transform.position;
        }
        else
        {
            enemyPosition = transform.position;
        }

        return enemyPosition;
    }
}
