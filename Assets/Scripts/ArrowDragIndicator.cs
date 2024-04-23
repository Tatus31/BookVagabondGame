using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ArrowDragIndicator : MonoBehaviour
{
    public static ArrowDragIndicator Instance;

    [Header("Enemy Line")]
    [SerializeField] AnimationCurve enemyAnimationCurve;
    [SerializeField] Material enemyLineMaterial;
    [Space(20)]
    [Header("Player Line")]
    [SerializeField] AnimationCurve characterAnimationCurve;
    [SerializeField] Material characterLineMaterial;

    private GameObject lineObject;
    private LineRenderer lineRenderer;
    private GameObject currentCharacter;
    private GameObject currentEnemy;

    private Dictionary<GameObject, LineRenderer> _enemyLines = new Dictionary<GameObject, LineRenderer>();
    public Dictionary<GameObject, LineRenderer> EnemyLines { get { return _enemyLines; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Movement.Instance.AllTargetsReached += OnAllTargetsReached;
    }

    private void OnDestroy()
    {
        Movement.Instance.AllTargetsReached -= OnAllTargetsReached;
    }

    private void Update()
    {
        if (CharacterSelection.Instance.CurrentCharacterSelected != null && PlayerTargetingSystem.Instance.CharacterTargets != null)
        {

            GameObject selectedCharacter = CharacterSelection.Instance.CurrentCharacterSelected;
            GameObject selectedEnemy = PlayerTargetingSystem.Instance.GetTargetForCharacter(selectedCharacter);

            if (selectedCharacter != null && selectedEnemy != null)
            {
                if (selectedCharacter != currentCharacter || selectedEnemy != currentEnemy)
                {
                    currentCharacter = selectedCharacter;
                    currentEnemy = selectedEnemy;

                    CreateOrUpdateLineRenderer(selectedCharacter, selectedEnemy);
                }
            }
            else
            {
                RemoveLineRenderer();
                currentCharacter = null;
                currentEnemy = null;
            }
        }
    }

    private void CreateOrUpdateLineRenderer(GameObject characterPosition, GameObject enemyPosition)
    {
        if (this.lineRenderer == null)
        {
            lineObject = new GameObject("LineRenderer");
            lineObject.transform.SetParent(transform);
            this.lineRenderer = lineObject.AddComponent<LineRenderer>();
            this.lineRenderer.useWorldSpace = true;
            this.lineRenderer.widthCurve = characterAnimationCurve;
            this.lineRenderer.numCapVertices = 10;

            if (enemyLineMaterial != null)
            {
                this.lineRenderer.material = characterLineMaterial;
            }
            else
            {
                Debug.LogError("Enemy Line Material is not assigned.");
            }
        }

        CurvePointCalculator(characterPosition, enemyPosition, this.lineRenderer);

        currentCharacter = characterPosition;
        currentEnemy = enemyPosition;
    }




    public void CreateOrUpdateEnemyLineRenderer(GameObject enemyPosition, GameObject characterPosition)
    {
        LineRenderer lineRenderer;

        if (_enemyLines.ContainsKey(enemyPosition))
        {
            lineRenderer = _enemyLines[enemyPosition];
        }
        else
        {
            GameObject lineObject = new GameObject("LineRenderer");
            lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineObject.transform.SetParent(transform);
            lineRenderer.useWorldSpace = true;
            lineRenderer.widthCurve = enemyAnimationCurve;
            lineRenderer.numCapVertices = 10;

            if (enemyLineMaterial != null)
            {
                lineRenderer.material = enemyLineMaterial;
            }
            else
            {
                Debug.LogError("Enemy Line Material is not assigned.");
            }

            _enemyLines.Add(enemyPosition, lineRenderer);
        }

        CurvePointCalculator(enemyPosition, characterPosition, lineRenderer);
    }



    private void RemoveLineRenderer()
    {
        if (lineObject != null && !Movement.Instance.IsMoving)
        {
            Destroy(lineObject);
            lineObject = null;
            lineRenderer = null;
        }
    }

    private void OnAllTargetsReached()
    {
        DestroyAllLineRenderers();
    }

    private void DestroyAllLineRenderers()
    {
        LineRenderer[] renderers = FindObjectsOfType<LineRenderer>();
        foreach (LineRenderer renderer in renderers)
        {
            Destroy(renderer.gameObject);
        }
    }

    private void CurvePointCalculator(GameObject startPosition, GameObject endPosition, LineRenderer lineRenderer)
    {
        Vector3 start = startPosition.transform.position;
        Vector3 end = endPosition.transform.position;
        Vector3 midPoint = (start + end) / 2f;
        float yOffset = 2f;

        Vector3 controlPoint1 = start + Vector3.up * yOffset;
        Vector3 controlPoint2 = end + Vector3.up * yOffset;

        List<Vector3> curvePoints = new List<Vector3>();

        int numSegments = 20;

        for (int i = 0; i <= numSegments; i++)
        {
            float t = i / (float)numSegments;
            Vector3 point = Mathf.Pow(1 - t, 3) * start +
                            3 * Mathf.Pow(1 - t, 2) * t * controlPoint1 +
                            3 * (1 - t) * Mathf.Pow(t, 2) * controlPoint2 +
                            Mathf.Pow(t, 3) * end;
            curvePoints.Add(point);
        }

        lineRenderer.positionCount = curvePoints.Count;
        lineRenderer.SetPositions(curvePoints.ToArray());
    }
}
