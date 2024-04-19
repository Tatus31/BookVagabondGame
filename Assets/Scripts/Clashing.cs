using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Clashing : MonoBehaviour
{
    public static Clashing Instance;

    private PlayerTargetingSystem playerTargetingSystem;
    private EnemyTargetingSystem enemyTargetingSystem;
    private CharacterAndEnemySpeed characterAndEnemySpeed;
    private ArrowDragIndicator arrowDragIndicator;

    private Dictionary<GameObject, GameObject> _clashingEntities = new Dictionary<GameObject, GameObject>();
    public Dictionary<GameObject, GameObject> ClashingEntities { get { return _clashingEntities; } }

    private Dictionary<GameObject, GameObject> originalEnemyTargets = new Dictionary<GameObject, GameObject>();
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
                //Debug.Log($"{character.name}'s enemy in PlayerTargetingSystem matches their target in EnemyTargetingSystem.");

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
                //Debug.Log($"{enemy.name}'s target in EnemyTargetingSystem matches their character in PlayerTargetingSystem.");

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

            GameObject originalEnemyTargetEntity = enemyTargetingSystem.EnemyTargets[target];
            Debug.Log(originalEnemyTargetEntity.name);

            if (characterAndEnemySpeed.GetEntitySpeed(entity) > characterAndEnemySpeed.GetEntitySpeed(target))
            {
                Debug.Log($"{entity.name} is changing clash target for {target.name}");

                enemyTargetingSystem.SetTarget(target, entity);

                arrowDragIndicator.DrawOrUpdateEnemyTargetingLineRenderer(target, entity);
            }          
        }
    }

    public void SetOriginalTarget(GameObject enemy, GameObject originalTarget)
    {
        if (!originalEnemyTargets.ContainsKey(enemy))
        {
            originalEnemyTargets.Add(enemy, originalTarget);
        }
        else
        {
            originalEnemyTargets[enemy] = originalTarget;
        }
    }

    public GameObject GetOriginalTarget(GameObject enemy)
    {
        if (originalEnemyTargets.ContainsKey(enemy))
        {
            return originalEnemyTargets[enemy];
        }
        return null;
    }

    public void DeselectEnemy(GameObject enemy)
    {
        GameObject originalTarget = enemyTargetingSystem.GetOriginalTarget(enemy);
        if (originalTarget != null)
        {
            enemyTargetingSystem.SetTarget(enemy, originalTarget);
        }
    }
}
