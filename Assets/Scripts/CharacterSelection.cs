using UnityEngine;
using System.Collections.Generic;

public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection Instance;

    [SerializeField] private List<GameObject> selectedCharacters = new List<GameObject>();
    [Space(10)]
    [SerializeField] private int characterSelectionLimit;

    private GameObject _currentCharacterSelected;
    public GameObject CurrentCharacterSelected { get { return _currentCharacterSelected; } private set { _currentCharacterSelected = value; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (PlayerInput.Instance.LeftClickClicked)
        {
            SelectCharacter();
        }
        else if (PlayerInput.Instance.RightClickClicked)
        {
            DeselectCharacter();
        }
    }

    private void SelectCharacter()
    {
        GameObject character = SelectionCheck();
        if (character != null && IsCharacterSelected())
        {
            selectedCharacters.Add(character);
            UpdateCharacterSelectedStatus();
        }
    }

    private void DeselectCharacter()
    {
        GameObject character = SelectionCheck();
        if (character != null && !IsCharacterSelected())
        {
            selectedCharacters.Remove(character);
            UpdateCharacterSelectedStatus();
        }
    }


    private bool IsCharacterSelected()
    {
        return selectedCharacters.Count < characterSelectionLimit;
    }

    private void UpdateCharacterSelectedStatus()
    {
        if (selectedCharacters.Count > 0)
        {
            _currentCharacterSelected = selectedCharacters[0];
            //Debug.Log(_currentCharacterSelected.name);
        }
        else
        {
            _currentCharacterSelected = null;
        }
    }

    public GameObject SelectionCheck()
    {
        RaycastHit hit;
        Ray ray = MouseWorldPosition.Instance.GetMouseRayWorld();

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            while (objectHit != null)
            {
                if (objectHit.gameObject.CompareTag("Character") || objectHit.gameObject.CompareTag("Enemy"))
                {
                    return objectHit.gameObject;
                }
                objectHit = objectHit.parent;
            }
        }

        return null;
    }


    public Vector3 GetSelectedCharacterPosition()
    {
        return _currentCharacterSelected.transform.position;
    }
}
