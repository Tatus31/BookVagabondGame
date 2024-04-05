using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TargetingSystem : MonoBehaviour
{
    //should work fine with more diffrent characters (it fucking didnt)
    public static TargetingSystem Instance;

    public int targetTest;

    private Dictionary<GameObject, GameObject> _targetList = new Dictionary<GameObject, GameObject>();
    public Dictionary<GameObject, GameObject> TargetList { get { return _targetList; } private set { _targetList = value; } }

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
        targetTest = _targetList.Values.Count;

        if(CharacterSelection.Instance.CurrentCharacterSelected == null)
        {
            return;
        }

        if (PlayerInput.Instance.LeftClickClicked)
        {
            SelectEnemy();
        }
        else if (PlayerInput.Instance.RightClickClicked)
        {
            DeselectEnemy();
        }

        //testing will change to a specific point probably
        if (Input.GetKeyDown(KeyCode.Space) && _targetList.Count > 0)
        {
            Debug.Log("Going");
            isMoving = true;
        }

        if (isMoving && _targetList.Count > 0)
        {
            MoveTowardsTargets();
        }
    }
    private void SelectEnemy()
    {
        GameObject enemy = CharacterSelection.Instance.SelectionCheck();

        if (enemy != null && enemy.CompareTag("Enemy") && !_targetList.ContainsKey(CharacterSelection.Instance.CurrentCharacterSelected))
        {
            _targetList.Add(CharacterSelection.Instance.CurrentCharacterSelected,enemy);            
            foreach (KeyValuePair<GameObject, GameObject> go in _targetList)
            {
                Debug.Log($"character: {go.Key.name} enemy: {go.Value.name}");
            }
        }
    }

    private void DeselectEnemy()
    {
        GameObject enemy = CharacterSelection.Instance.SelectionCheck();

        if (enemy != null && _targetList.ContainsValue(enemy))
        {
            _targetList.Remove(CharacterSelection.Instance.CurrentCharacterSelected,out enemy);
        }
    }

    //testing
    private void MoveTowardsTargets()
    {
        List<GameObject> charactersToRemove = new List<GameObject>();

        foreach (var pair in _targetList)
        {
            GameObject character = pair.Key;
            GameObject enemy = pair.Value;

            if (enemy != null)
            {
                Vector3 targetPosition = enemy.transform.position;
                Vector3 direction = (targetPosition - character.transform.position).normalized;
                float distanceToTarget = Vector3.Distance(character.transform.position, targetPosition);

                if (distanceToTarget > stoppingDistance)
                {
                    character.transform.position += direction * moveSpeed * Time.deltaTime;
                }
                else
                {
                    charactersToRemove.Add(character);
                }
            }
        }

        foreach (var character in charactersToRemove)
        {
            _targetList.Remove(character);
        }

        if (_targetList.Count == 0)
        {
            isMoving = false;
            Debug.Log("All targets reached");
        }
    }
}
