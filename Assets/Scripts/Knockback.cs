using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public static KnockBack Instance;

    [SerializeField] private float knockbackDistance;
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private AnimationCurve knockbackCurve;

    private void Awake()
    {
        Instance = this;
    }

    public void KnockBackEntity(GameObject entity, Vector3 direction)
    {
        StartCoroutine(ApplyKnockBack(entity, direction));
    }

    private IEnumerator ApplyKnockBack(GameObject entity, Vector3 direction)
    {
        Vector3 startPosition = entity.transform.position;
        Vector3 targetPosition = startPosition + direction * knockbackDistance;

        float movedDistance = 0f;
        float totalDistance = knockbackDistance;

        while (movedDistance < totalDistance)
        {
            float progress = movedDistance / totalDistance;
            float slowdownFactor = knockbackCurve.Evaluate(progress);
            float movementStep = Time.deltaTime / knockbackSpeed * slowdownFactor;

            entity.transform.position += direction * movementStep;
            movedDistance += movementStep;

            yield return null;
        }

        entity.transform.position = targetPosition;
    }

}
