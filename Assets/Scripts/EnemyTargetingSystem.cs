using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetingSystem : MonoBehaviour
{
    public static EnemyTargetingSystem Instance;

    private Dictionary<GameObject, GameObject> _enemyTargets = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, LineRenderer> _enemyLines = new Dictionary<GameObject, LineRenderer>();

    public Dictionary<GameObject, GameObject> EnemyTargets { get { return _enemyTargets; } }

    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] Material lineMaterial;

    private void Awake()
    {
        Instance = this;
    }

    public void AssignTargetsToEnemies()
    {
        List<GameObject> characters = CharactersAndEnemiesList.Instance.characters;
        List<GameObject> enemies = CharactersAndEnemiesList.Instance.enemies;

        foreach (GameObject enemy in _enemyLines.Keys)
        {
            if (!enemies.Contains(enemy))
            {
                Destroy(_enemyLines[enemy].gameObject);
                _enemyLines.Remove(enemy);
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
                }
                else
                {
                    _enemyTargets.Add(enemy, targetCharacter);
                }

                //Debug.Log($"Enemy {enemy} targets character {_enemyTargets[enemy]}");

                DrawOrUpdateLine(enemy, targetCharacter);
            }
        }
    }

    private void DrawOrUpdateLine(GameObject enemy, GameObject target)
    {
        LineRenderer lineRenderer;

        if (_enemyLines.ContainsKey(enemy))
        {
            lineRenderer = _enemyLines[enemy];
        }
        else
        {
            GameObject lineObject = new GameObject("LineRenderer");
            lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = true;
            lineRenderer.widthCurve = animationCurve;
            lineRenderer.numCapVertices = 10;

            lineRenderer.material = lineMaterial;

            _enemyLines.Add(enemy, lineRenderer);
        }

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, enemy.transform.position);
        lineRenderer.SetPosition(1, target.transform.position);
    }
}