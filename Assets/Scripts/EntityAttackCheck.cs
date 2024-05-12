using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackDistanceCheck : MonoBehaviour
{
    public static AttackDistanceCheck instance;

    private Dictionary<GameObject, GameObject> _entitiesThatAttacked = new Dictionary<GameObject, GameObject>();
    public Dictionary<GameObject, GameObject> EntitiesThatAttacked { get { return _entitiesThatAttacked; } }

    private bool attackEnd = false;
    public bool AttackEnd { get { return attackEnd; } set { attackEnd = value; } }

    private void Awake()
    {
        instance = this;
    }

    public void EntityAttack(GameObject entity, GameObject target)
    {
        _entitiesThatAttacked.Add(entity, target);
    }

    public void ResetEntityAttackCooldown(GameObject entity)
    {
        _entitiesThatAttacked.Remove(entity);
    }

    public void ResetAllEntityAttackCooldown()
    {
        _entitiesThatAttacked.Clear();
    }

    public bool HasEntityAttacked(GameObject entity, GameObject target)
    {
        if (_entitiesThatAttacked.ContainsKey(entity) & _entitiesThatAttacked.ContainsValue(target))
        {
            return true;
        }

        return false;
    }
}