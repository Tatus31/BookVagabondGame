using UnityEngine;
using System.Collections.Generic;

public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection Instance;

    [SerializeField] private List<GameObject> selectedCharacters = new List<GameObject>();

    public bool isCharacterSelected;

    private void Awake()
    {
        Instance = this;
    }

    private void OnMouseOver()
    {
        if (PlayerInput.Instance.LeftClickClicked)
        {
            SelectCharacter(gameObject);
        }
        else if (PlayerInput.Instance.RightClickClicked)
        {
            DeselectCharacter(gameObject);
        }
    }

    private void SelectCharacter(GameObject character)
    {
        if (!selectedCharacters.Contains(character))
        {
            selectedCharacters.Add(character);
            isCharacterSelected = true;
        }
    }

    private void DeselectCharacter(GameObject character)
    {
        if (selectedCharacters.Contains(character))
        {
            selectedCharacters.Remove(character);
            isCharacterSelected = false;
        }
    }
}
