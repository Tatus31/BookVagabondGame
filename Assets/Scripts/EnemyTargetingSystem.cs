using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetingSystem : MonoBehaviour
{
    public static EnemyTargetingSystem Instance;

    private Dictionary<GameObject, GameObject> _enemyTargets = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, GameObject> _originalEnemyTargets = new Dictionary<GameObject, GameObject>();

    public Dictionary<GameObject, GameObject> EnemyTargets { get { return _enemyTargets; } }
    public Dictionary<GameObject,GameObject> OriginalEnemyTargets {  get { return _originalEnemyTargets; } }

    private ArrowDragIndicator arrowDragIndicator;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        arrowDragIndicator = ArrowDragIndicator.Instance;
    }

    public void AssignTargetsToEnemies()
    {
        List<GameObject> characters = CharactersAndEnemiesList.Instance.characters;
        List<GameObject> enemies = CharactersAndEnemiesList.Instance.enemies;

        foreach (GameObject enemy in arrowDragIndicator.EnemyLines.Keys)
        {
            if (!enemies.Contains(enemy))
            {
                Destroy(arrowDragIndicator.EnemyLines[enemy].gameObject);
                arrowDragIndicator.EnemyLines.Remove(enemy);
            }
        }

        if (characters.Count > 0 && enemies.Count > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                int randomCharacterIndex = Random.Range(0, characters.Count);
                GameObject targetCharacter = characters[randomCharacterIndex];

                if (_enemyTargets.ContainsKey(enemy))
                {
                    _enemyTargets[enemy] = targetCharacter;
                    _originalEnemyTargets[enemy] = targetCharacter;
                }
                else
                {
                    _enemyTargets.Add(enemy, targetCharacter);
                    _originalEnemyTargets.Add(enemy, targetCharacter);
                }

                arrowDragIndicator.DrawOrUpdateEnemyTargetingLineRenderer(enemy, targetCharacter);
            }
        }
    }

    public void SetTarget(GameObject enemy, GameObject newTarget)
    {
        if (_enemyTargets.ContainsKey(enemy))
        {
            _enemyTargets[enemy] = newTarget;
            arrowDragIndicator.DrawOrUpdateEnemyTargetingLineRenderer(enemy, newTarget);
        }
    }

    public GameObject GetTarget(GameObject enemy)
    {
        GameObject target = null;

        if (_enemyTargets.ContainsKey(enemy))
        {
            target = _enemyTargets[enemy];
        }

        return target;
    }

    public GameObject GetOriginalTarget(GameObject enemy)
    {
        GameObject originalTarget = null;

        if (_originalEnemyTargets.ContainsKey(enemy))
        {
            originalTarget = _originalEnemyTargets[enemy];
        }

        return originalTarget;
    }
}
