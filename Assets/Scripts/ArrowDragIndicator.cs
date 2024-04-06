using UnityEngine;

public class ArrowDragIndicator : MonoBehaviour
{
    [SerializeField] AnimationCurve animationCurve;

    private GameObject lineObject;
    private LineRenderer lineRenderer;
    private GameObject currentCharacter;
    private GameObject currentEnemy;

    private void Start()
    {
        TargetingSystem.Instance.AllTargetsReached += OnAllTargetsReached;
    }

    private void OnDestroy()
    {
        TargetingSystem.Instance.AllTargetsReached -= OnAllTargetsReached;
    }

    private void Update()
    {
        if (CharacterSelection.Instance.CurrentCharacterSelected != null && TargetingSystem.Instance.CharacterTargets != null)
        {
            GameObject selectedCharacter = CharacterSelection.Instance.CurrentCharacterSelected;
            GameObject selectedEnemy = TargetingSystem.Instance.GetTargetForCharacter(selectedCharacter);

            if (selectedCharacter != null && selectedEnemy != null)
            {
                if (selectedCharacter != currentCharacter || selectedEnemy != currentEnemy)
                {
                    currentCharacter = selectedCharacter;
                    currentEnemy = selectedEnemy;

                    CreateOrUpdateLineRenderer(selectedCharacter.transform.position, selectedEnemy.transform.position);
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

    private void CreateOrUpdateLineRenderer(Vector3 characterPosition, Vector3 enemyPosition)
    {
        if (lineObject == null)
        {
            lineObject = new GameObject("LineRenderer");
            lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = true;
            lineRenderer.widthCurve = animationCurve;
            lineRenderer.numCapVertices = 10;
        }

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, characterPosition);
        lineRenderer.SetPosition(1, enemyPosition);
    }

    private void RemoveLineRenderer()
    {
        if (lineObject != null && !TargetingSystem.Instance.IsMoving)
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
}
