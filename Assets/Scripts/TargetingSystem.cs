using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TargetingSystem : MonoBehaviour
{
    //should work fine with more diffrent characters 
    public static TargetingSystem Instance;

    private GameObject _currentTarget;

    public GameObject CurrentTarget { get { return _currentTarget; } private set { _currentTarget = value; } }

    //testing
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stoppingDistance;

    private bool isMoving = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (PlayerInput.Instance.LeftClickClicked)
        {
            SelectEnemy();
        }
        else if (PlayerInput.Instance.RightClickClicked)
        {
            DeselectEnemy();
        }

        //testing will change to a specific point probably
        if (Input.GetKeyDown(KeyCode.Space) && _currentTarget != null)
        {
            Debug.Log("Going");
            isMoving = true;
        }

        if (isMoving && _currentTarget != null)
        {
            MoveTowardsTarget(_currentTarget.transform.position);
        }
    }
    private void SelectEnemy()
    {
        GameObject Enemy = CharacterSelection.Instance.SelectionCheck();

        if(Enemy != null && Enemy.CompareTag("Enemy") && !_currentTarget == Enemy && CharacterSelection.Instance.AnyCharacterSelected)
        {
            _currentTarget = Enemy;
        }
    }

    private void DeselectEnemy()
    {
        GameObject Enemy = CharacterSelection.Instance.SelectionCheck();

        if (Enemy != null && Enemy.CompareTag("Enemy") && _currentTarget == Enemy && CharacterSelection.Instance.AnyCharacterSelected)
        {
            _currentTarget = null;
        }
    }

    //testing
    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget > stoppingDistance)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            isMoving = false; 
        }
    }
}
