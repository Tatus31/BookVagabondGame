using UnityEngine;
using System.Collections.Generic;

public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection Instance;

    [SerializeField] private List<GameObject> selectedCharacters = new List<GameObject>();
    [Space(10)]
    [SerializeField] private int characterSelectionLimit;

    private bool _anyCharacterSelected;
    public bool AnyCharacterSelected { get { return _anyCharacterSelected; } private set { _anyCharacterSelected = value; } }

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
        if (character != null && character.CompareTag("Character") && !selectedCharacters.Contains(character) && IsCharacterSelected())
        {
            selectedCharacters.Add(character);
            UpdateCharacterSelectedStatus();
        }
    }

    private void DeselectCharacter()
    {
        GameObject character = SelectionCheck();
        if (character != null && character.CompareTag("Character") && selectedCharacters.Contains(character) && !IsCharacterSelected())
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
        AnyCharacterSelected = selectedCharacters.Count > 0;
    }

    public GameObject SelectionCheck()
    {
        RaycastHit hit;
        Ray ray = MouseWorldPosition.Instance.GetMouseRayWorld();

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject;
        }

        return null;
    }
}
