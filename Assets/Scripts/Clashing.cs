using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Clashing : MonoBehaviour
{
    public static Clashing Instance;

    private PlayerTargetingSystem playerTargetingSystem;
    private EnemyTargetingSystem enemyTargetingSystem;
    private CharacterAndEnemySpeed characterAndEnemySpeed;
    private ArrowDragIndicator arrowDragIndicator;

    private List<GameObject> enemiesAfterTargetChange = new List<GameObject>();

    private Dictionary<GameObject, GameObject> _clashingEntities = new Dictionary<GameObject, GameObject>();
    public Dictionary<GameObject, GameObject> ClashingEntities { get { return _clashingEntities; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerTargetingSystem = PlayerTargetingSystem.Instance;
        enemyTargetingSystem = EnemyTargetingSystem.Instance;
        characterAndEnemySpeed = CharacterAndEnemySpeed.Instance;
        arrowDragIndicator = ArrowDragIndicator.Instance;
    }

    public void CheckTargetingClashes()
    {
        foreach (var playerPair in playerTargetingSystem.CharacterTargets)
        {
            GameObject character = playerPair.Key;
            GameObject enemy = playerPair.Value;

            if (enemyTargetingSystem.EnemyTargets.ContainsKey(character) &&
                enemyTargetingSystem.EnemyTargets[character] == enemy)
            {
                //Debug.Log($"{character.name} enemy in PlayerTargetingSystem matches their target");

                if (!_clashingEntities.ContainsKey(character))
                {
                    _clashingEntities.Add(character, enemy);
                }
            }
        }

        foreach (var enemyPair in enemyTargetingSystem.EnemyTargets)
        {
            GameObject enemy = enemyPair.Key;
            GameObject character = enemyPair.Value;

            if (playerTargetingSystem.CharacterTargets.ContainsKey(character) &&
                playerTargetingSystem.CharacterTargets[character] == enemy)
            {
                //Debug.Log($"{enemy.name} target in EnemyTargetingSystem matches their character");

                if (!_clashingEntities.ContainsKey(enemy))
                {
                    _clashingEntities.Add(enemy, character);
                }
            }
        }
    }

    public void ForceClashingTarget()
    {
        foreach (var PlayerPair in playerTargetingSystem.CharacterTargets)
        {
            GameObject entity = PlayerPair.Key;
            GameObject target = PlayerPair.Value;

            if (characterAndEnemySpeed.GetEntitySpeed(entity) > characterAndEnemySpeed.GetEntitySpeed(target))
            {
                //Debug.Log($"{entity.name} is changing clash target for {target.name}");

                if (!playerTargetingSystem.IsTargetLocked(target) || playerTargetingSystem.IsTargetLocked(target) && playerTargetingSystem.IsTargetLocked(entity))
                {
                    enemyTargetingSystem.SetTarget(target, entity);
                    enemiesAfterTargetChange.Add(target);
                    playerTargetingSystem.LockTarget(target, entity);

                    arrowDragIndicator.CreateOrUpdateEnemyLineRenderer(target, entity);
                }
            }          
        }
    }

    public void DeselectEnemy(GameObject enemy)
    {
        if (enemiesAfterTargetChange.Contains(enemy))
        {
            GameObject originalTarget = enemyTargetingSystem.GetOriginalTarget(enemy);

            if (originalTarget != null)
            {
                bool isOtherCharacterTargeting = IsOtherCharacterTargeting(enemy);

                if (!isOtherCharacterTargeting)
                {
                    enemiesAfterTargetChange.Remove(enemy);
                    playerTargetingSystem.UnlockTarget(enemy);
                    enemyTargetingSystem.SetTarget(enemy, originalTarget);
                }
            }
        }
    }

    public bool AreEntitiesClashing(GameObject character, GameObject enemy)
    {
        if (_clashingEntities.ContainsKey(character) && _clashingEntities[character] == enemy)
        {
            return true;
        }

        if (_clashingEntities.ContainsKey(enemy) && _clashingEntities[enemy] == character)
        {
            return true;
        }

        return false;
    }

    private bool IsOtherCharacterTargeting(GameObject enemy)
    {
        foreach (var pair in playerTargetingSystem.CharacterTargets)
        {
            if (pair.Key != enemy && pair.Value == enemy)
            {
                return true;
            }
        }

        return false;
    }

}
