using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public static TargetingSystem Instance;

    public event Action AllTargetsReached;

    private Dictionary<GameObject, GameObject> _characterTargets = new Dictionary<GameObject, GameObject>();
    public Dictionary<GameObject, GameObject> CharacterTargets { get { return _characterTargets; } }

    private bool _isMoving = false;
    public bool IsMoving { get { return _isMoving; } }

    [SerializeField] private float nextCharacterMove;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stoppingDistance;

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

        if (Input.GetKeyDown(KeyCode.Space) && _characterTargets.Count > 0 && !_isMoving)
        {
            OnAllTargetsReached();
            StartCoroutine(MoveTowardsTargets());
        }
    }

    private void SelectEnemy()
    {
        GameObject enemy = CharacterSelection.Instance.SelectionCheck();

        if (enemy != null && enemy.CompareTag("Enemy"))
        {
            GameObject selectedCharacter = CharacterSelection.Instance.CurrentCharacterSelected;
            if (selectedCharacter != null)
            {
                if (_characterTargets.ContainsKey(selectedCharacter))
                {
                    _characterTargets[selectedCharacter] = enemy;
                }
                else
                {
                    _characterTargets.Add(selectedCharacter, enemy);
                }
            }
        }
    }

    private void DeselectEnemy()
    {
        GameObject enemy = CharacterSelection.Instance.SelectionCheck();

        if (enemy != null)
        {
            GameObject selectedCharacter = CharacterSelection.Instance.CurrentCharacterSelected;
            if (selectedCharacter != null && _characterTargets.ContainsKey(selectedCharacter) && _characterTargets[selectedCharacter] == enemy)
            {
                _characterTargets.Remove(selectedCharacter);
            }
        }
    }

    private IEnumerator MoveTowardsTargets()
    {
        _isMoving = true;

        List<CharacterWithSpeed> charactersWithSpeed = new List<CharacterWithSpeed>();

        foreach (var pair in _characterTargets)
        {
            GameObject character = pair.Key;
            GameObject enemy = pair.Value;

            if (character != null && enemy != null)
            {
                int speed = GetCharacterSpeed(character);
                charactersWithSpeed.Add(new CharacterWithSpeed(character, speed));
            }
        }

        charactersWithSpeed.Sort((a, b) => a.Speed.CompareTo(b.Speed));

        foreach (var characterWithSpeed in charactersWithSpeed)
        {
            GameObject character = characterWithSpeed.Character;
            GameObject enemy = _characterTargets[character];

            if (enemy != null)
            {
                while (Vector3.Distance(character.transform.position, enemy.transform.position) > stoppingDistance)
                {
                    Vector3 targetPosition = enemy.transform.position;
                    Vector3 direction = (targetPosition - character.transform.position).normalized;
                    character.transform.position += direction * moveSpeed * Time.deltaTime;
                    yield return null;
                }
            }

            yield return new WaitForSeconds(nextCharacterMove);
        }

        _isMoving = false;
        Debug.Log("All targets reached");    
    }


    private void OnAllTargetsReached()
    {
        AllTargetsReached?.Invoke();
    }

    private int GetCharacterSpeed(GameObject character)
    {
        CharacterSpeed characterSpeedScript = character.GetComponent<CharacterSpeed>();

        if (characterSpeedScript != null)
        {
            return characterSpeedScript.GetCharacterSpeed();
        }
        else
        {
            return 0;
        }
    }

    public GameObject GetTargetForCharacter(GameObject character)
    {
        if (_characterTargets.ContainsKey(character))
        {
            return _characterTargets[character];
        }
        else
        {
            return null;
        }
    }

    private class CharacterWithSpeed
    {
        public GameObject Character { get; private set; }
        public int Speed { get; private set; }

        public CharacterWithSpeed(GameObject character, int speed)
        {
            Character = character;
            Speed = speed;
        }
    }
}
