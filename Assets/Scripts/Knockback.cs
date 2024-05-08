using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public static KnockBack Instance;

    [SerializeField] private float knockbackAmmount;
    [SerializeField] private Transform knockbackDirection;
    [SerializeField] private Transform enemyKnockbackDirection;
    private void Awake()
    {
        Instance = this;
    }

    public void KnockBackEntity(GameObject entity, Vector3 direction)
    {
        entity.transform.position += direction;
    }

    public bool HasBeenKnockedBack(GameObject entity)
    {
        if (AttackDistanceCheck.instance.EntitiesThatAttacked.ContainsKey(entity) || AttackDistanceCheck.instance.EntitiesThatAttacked.ContainsValue(entity))
        {
            return true;
        }

        return false;
    }

    public Vector3 GetCharacterKnockbackDirection()
    {
        return knockbackDirection.position;
    }

    public Vector3 GetEnemyKnockbackDirection()
    {
        return enemyKnockbackDirection.position;
    }
}
