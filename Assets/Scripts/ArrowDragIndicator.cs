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
        PlayerTargetingSystem.Instance.EnemySelected += OnEnemySelected;
        Movement.Instance.AllTargetsReached += OnAllTargetsReached;
    }

    private void OnDestroy()
    {
        PlayerTargetingSystem.Instance.EnemySelected -= OnEnemySelected;
        Movement.Instance.AllTargetsReached -= OnAllTargetsReached;
    }

    private void Update()
    {
        if (SkillSlotSelection.Instance.CurrentSkillSlotSelected != null && PlayerTargetingSystem.Instance.CharacterTargets != null)
        {

            GameObject selectedCharacterSkillSlot = SkillSlotSelection.Instance.CurrentSkillSlotSelected;
            GameObject selectedEnemy = PlayerTargetingSystem.Instance.GetTargetForCharacter(selectedCharacterSkillSlot.transform.parent.gameObject);

            if (selectedCharacterSkillSlot != null && selectedEnemy != null)
            {
                if (selectedCharacterSkillSlot != currentCharacter || selectedEnemy != currentEnemy)
                {
                    currentCharacter = selectedCharacterSkillSlot;
                    currentEnemy = selectedEnemy;

                    CreateOrUpdateLineRenderer(selectedCharacterSkillSlot, selectedEnemy);
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

    private void CreateOrUpdateLineRenderer(GameObject characterSkillSlotPosition, GameObject enemyPosition)
    {
        if (this.lineRenderer == null)
        {
            lineObject = new GameObject("SkillSlotLineRenderer");
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

        CurvePointCalculator(characterSkillSlotPosition.transform.position, enemyPosition.transform.position, this.lineRenderer);

        currentCharacter = characterSkillSlotPosition;
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

        CurvePointCalculator(enemyPosition.transform.position, characterPosition.transform.position, lineRenderer);
    }

    public void FollowMouseLineRenderer(GameObject characterPositon ,Vector3 mousePosition)
    {
        if (this.lineRenderer == null)
        {
            GameObject lineObject = new GameObject("FollowLineRenderer");
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

        CurvePointCalculator(characterPositon.transform.position, mousePosition, this.lineRenderer);
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

    private void OnEnemySelected()
    {
        if (lineObject != null)
        {
            Destroy(lineObject);
            lineObject = null;
            lineRenderer = null;
        }

        CreateOrUpdateLineRenderer(SkillSlotSelection.Instance.CurrentSkillSlotSelected, PlayerTargetingSystem.Instance.GetTargetForCharacter(SkillSlotSelection.Instance.CurrentSkillSlotSelected.transform.parent.gameObject));
    }

    private void DestroyAllLineRenderers()
    {
        foreach (LineRenderer renderer in FindObjectsOfType<LineRenderer>())
        {
            Destroy(renderer.gameObject);
        }
    }

    private void CurvePointCalculator(Vector3 start, Vector3 end, LineRenderer lineRenderer)
    {
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
