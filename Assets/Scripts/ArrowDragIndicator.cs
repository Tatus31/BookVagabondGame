using UnityEngine;

public class ArrowDragIndicator : MonoBehaviour
{
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] Material lineMaterial;

    private GameObject lineObject;
    private LineRenderer lineRenderer;
    private GameObject currentCharacter;
    private GameObject currentEnemy;

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

            lineRenderer.material = lineRenderer.material;
        }

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, characterPosition);
        lineRenderer.SetPosition(1, enemyPosition);
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
}
