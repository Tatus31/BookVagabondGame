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
    private Clashing clashing;

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
        clashing = Clashing.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerTargetingSystem.CharacterTargets.Count > 0 && !_isMoving)
        {
            clashing.CheckTargetingClashes();
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

        entitiesWithSpeed.Sort((a, b) => b.Speed.CompareTo(a.Speed));

        foreach (var entityWithSpeed in entitiesWithSpeed)
        {
            GameObject currentEntity = entityWithSpeed.Entity;
            GameObject target = GetTargetForEntity(currentEntity);

            if(clashing.DidEntitiesAlreadyClash(currentEntity, target))
            {
                continue;
            }

            if (target != null)
            {
                Transform entityTransform = currentEntity.transform;
                Transform targetTransform = target.transform;

                while (targetTransform != null)
                {
                    float distanceToTarget = Vector3.Distance(entityTransform.position, targetTransform.position);

                    if (distanceToTarget <= stoppingDistance)
                    {
                        Vector3 entityKnockbackDirection = (entityTransform.position - targetTransform.position).normalized;
                        Vector3 targetKnockbackDirection = (targetTransform.position - entityTransform.position).normalized;

                        if (PlayerInput.Instance.LeftClickClicked && !AttackDistanceCheck.instance.HasEntityAttacked(currentEntity, target) && !clashing.AreEntitiesClashing(currentEntity, target))
                        {
                            Debug.Log($"{currentEntity} Attacked!");

                            KnockBack.Instance.KnockBackEntity(target, targetKnockbackDirection);

                            AttackDistanceCheck.instance.EntityAttack(currentEntity, target);

                            PlayerInput.Instance.LeftClickClicked = false;
                        }
                        else if (PlayerInput.Instance.LeftClickClicked && clashing.AreEntitiesClashing(currentEntity, target))
                        {
                            Debug.Log($"{currentEntity} and {target} are attacking eachother!");

                            KnockBack.Instance.KnockBackEntity(currentEntity, entityKnockbackDirection);
                            KnockBack.Instance.KnockBackEntity(target, targetKnockbackDirection);

                            clashing.EntitiesThatAlreadyClashed.Add(currentEntity, target);

                            AttackDistanceCheck.instance.EntityAttack(currentEntity, target);
                            AttackDistanceCheck.instance.EntityAttack(target, currentEntity);
                        }


                        if (!AttackDistanceCheck.instance.HasEntityAttacked(currentEntity, target) && !AttackDistanceCheck.instance.HasEntityAttacked(target, currentEntity))
                        {
                            yield return null;
                            continue;
                        }

                        break;
                    }
                    float movementStep = moveSpeed * Time.deltaTime;

                    Vector3 targetPosition = targetTransform.position;
                    targetPosition.y = entityTransform.position.y;
                    Vector3 direction = (targetPosition - entityTransform.position).normalized;

                    if (movementStep > distanceToTarget)
                    {
                        entityTransform.position = targetPosition;
                    }
                    else
                    {
                        Vector3 newPosition = entityTransform.position + direction * movementStep;
                        newPosition.y = entityTransform.position.y;
                        entityTransform.position = newPosition;
                    }

                    if (clashing.AreEntitiesClashing(currentEntity, target))
                    {
                        //Debug.Log($"{currentEntity.name} is Clashing! with {target.name}");

                          Vector3 reverseDirection = (entityTransform.position - targetTransform.position).normalized;

                          float reverseMovementStep = moveSpeed * Time.deltaTime;

                          if (reverseMovementStep > distanceToTarget)
                          {
                                targetTransform.position = entityTransform.position;
                          }
                          else
                          {
                              Vector3 newTargetPosition = targetTransform.position + reverseDirection * reverseMovementStep;
                              newTargetPosition.y = targetTransform.position.y;
                              targetTransform.position = newTargetPosition;
                          }

                    }

                    yield return null;

                    if (currentEntity != null && target != null)
                    {
                        entityTransform = currentEntity.transform;
                        targetTransform = target.transform;
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
            return characterSpeedScript.GetEntitySpeed(obj);
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