using UnityEngine;

public class ArrowDragIndicator : MonoBehaviour
{
    [SerializeField] AnimationCurve animationCurve;

    private void Update()
    {
        if (PlayerInput.Instance.LeftClickClicked && CharacterSelection.Instance.AnyCharacterSelected && TargetingSystem.Instance.CurrentTarget != null)
        {
            GameObject lineObject = new GameObject("LineRenderer");
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = true;
            lineRenderer.widthCurve = animationCurve;
            lineRenderer.numCapVertices = 10;

            Vector3 characterPosition = Character.Instance.GetCharacterPosition();
            Vector3 enemyPosition = TargetingSystem.Instance.CurrentTarget.transform.position;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, characterPosition);
            lineRenderer.SetPosition(1, enemyPosition);
        }
    }
}
