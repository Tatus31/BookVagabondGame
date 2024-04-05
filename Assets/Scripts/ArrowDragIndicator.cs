using UnityEngine;

public class ArrowDragIndicator : MonoBehaviour
{
    [SerializeField] AnimationCurve animationCurve;

    private void Update()
    {
        if (PlayerInput.Instance.LeftClickClicked && CharacterSelection.Instance.CurrentCharacterSelected && TargetingSystem.Instance.TargetList.Count >= 1)
        {
            GameObject lineObject = new GameObject("LineRenderer");
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = true;
            lineRenderer.widthCurve = animationCurve;
            lineRenderer.numCapVertices = 10;

            Vector3 characterPosition = CharacterSelection.Instance.GetSelectedCharacterPosition();
            Vector3 enemyPosition = Enemy.Instance.GetSelectedEnemyPosition();

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, characterPosition);
            lineRenderer.SetPosition(1, enemyPosition);
        }
    }
}
