using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Movement Instance;

    public event Action AllTargetsReached;

    private bool _isMoving = false;
    public bool IsMoving { get { return _isMoving; } }

    private PlayerTargetingSystem playerTargetingSystem;
    private EnemyTargetingSystem enemyTargetingSystem;

    [SerializeField] private float nextCharacterMove;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stoppingDistance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerTargetingSystem = PlayerTargetingSystem.Instance;
        enemyTargetingSystem = EnemyTargetingSystem.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerTargetingSystem.CharacterTargets.Count > 0 && !_isMoving)
        {
            OnAllTargetsReached();
            StartCoroutine(MoveTowardsTargets());
        }
    }

    private IEnumerator MoveTowardsTargets()
    {
        _isMoving = true;

        List<EntityWithSpeed> entitiesWithSpeed = new List<EntityWithSpeed>();

        foreach (var pair in playerTargetingSystem.CharacterTargets)
        {
            GameObject character = pair.Key;
            GameObject enemy = pair.Value;

            if (character != null && enemy != null)
            {
                int characterSpeed = GetCharacterAndEnemySpeed(character);

                entitiesWithSpeed.Add(new EntityWithSpeed(character, characterSpeed));
            }
        }

        foreach (var pair in enemyTargetingSystem.EnemyTargets)
        {
            GameObject enemy = pair.Key;
            GameObject character = pair.Value;

            if (character != null && enemy != null)
            {
                int enemySpeed = GetCharacterAndEnemySpeed(enemy);

                entitiesWithSpeed.Add(new EntityWithSpeed(enemy, enemySpeed));
            }
        }

        entitiesWithSpeed.Sort((a, b) => a.Speed.CompareTo(b.Speed));

        foreach (var entityWithSpeed in entitiesWithSpeed)
        {
            GameObject currentEntity = entityWithSpeed.Entity;
            GameObject target = GetTargetForEntity(currentEntity);

            if (target != null)
            {
                Transform entityTransform = currentEntity.transform;
                Transform targetTransform = target.transform;

                //Debug.Log($"starting position: {entityTransform.transform.position.x},{entityTransform.transform.position.y}, {entityTransform.transform.position.z}");
                while (targetTransform != null)
                {
                    float distanceToTarget = Vector3.Distance(entityTransform.position, targetTransform.position);

                    if (distanceToTarget <= stoppingDistance)
                    {
                        break;
                    }

                    Vector3 direction = (targetTransform.position - entityTransform.position).normalized;

                    float movementStep = moveSpeed * Time.deltaTime;

                    if (movementStep > distanceToTarget)
                    {
                        entityTransform.position = targetTransform.position;
                    }
                    else
                    {
                        entityTransform.position += direction * movementStep;
                    }

                    yield return null;
                    targetTransform = target.transform;

                    if (currentEntity != null && target != null)
                    {
                        entityTransform = currentEntity.transform;
                        targetTransform = target.transform;
                        //Debug.Log($"current position: {entityTransform.transform.position.x},{entityTransform.transform.position.y}, {entityTransform.transform.position.z}");
                    }
                    else
                    {
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(nextCharacterMove);
        }

        _isMoving = false;
        Debug.Log("All targets reached");
    }


    private GameObject GetTargetForEntity(GameObject entity)
    {
        if (playerTargetingSystem.CharacterTargets.ContainsKey(entity))
        {
            return playerTargetingSystem.CharacterTargets[entity];
        }
        else if (enemyTargetingSystem.EnemyTargets.ContainsKey(entity))
        {
            return enemyTargetingSystem.EnemyTargets[entity];
        }

        return null;
    }

    private int GetCharacterAndEnemySpeed(GameObject obj)
    {
        CharacterAndEnemySpeed characterSpeedScript = obj.GetComponent<CharacterAndEnemySpeed>();

        if (characterSpeedScript != null)
        {
            return characterSpeedScript.GetEntitySpeed();
        }
        else
        {
            Debug.LogWarning("CharacterSpeed not found on object: " + obj.name);
            return 0;
        }
    }

    private void OnAllTargetsReached()
    {
        AllTargetsReached?.Invoke();
    }

    private class EntityWithSpeed
    {
        public GameObject Entity { get; private set; }
        public int Speed { get; private set; }

        public EntityWithSpeed(GameObject entity, int speed)
        {
            Entity = entity;
            Speed = speed;
        }
    }

}