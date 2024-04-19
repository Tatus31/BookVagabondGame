using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTargetingSystem : MonoBehaviour
{
    public static PlayerTargetingSystem Instance;

    private Dictionary<GameObject, GameObject> _characterTargets = new Dictionary<GameObject, GameObject>();
    public Dictionary<GameObject, GameObject> CharacterTargets { get { return _characterTargets; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (PlayerInput.Instance == null || CharacterSelection.Instance == null)
        {
            Debug.LogWarning("PlayerInput or CharacterSelection instance not set.");
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
    }

    private void SelectEnemy()
    {
        if (CharacterSelection.Instance == null || CharacterSelection.Instance.CurrentCharacterSelected == null)
        {
            return;
        }

        GameObject enemy = CharacterSelection.Instance.SelectionCheck();

        if (enemy != null && enemy.CompareTag("Enemy") && CharacterSelection.Instance.CurrentCharacterSelected.CompareTag("Character"))
        {
            GameObject selectedCharacter = CharacterSelection.Instance.CurrentCharacterSelected;
            if (selectedCharacter != null)
            {
                _characterTargets[selectedCharacter] = enemy;
                Clashing.Instance.ForceClashingTarget();
                //Debug.Log($"Selected {selectedCharacter.name} targets enemy {enemy.name}");
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
                Clashing.Instance.DeselectEnemy(enemy);
                //Debug.Log($"Deselected {selectedCharacter.name} from targeting enemy {enemy.name}");
            }
        }
    }

    public GameObject GetTargetForCharacter(GameObject character)
    {
        if (_characterTargets.ContainsKey(character))
        {
            return _characterTargets[character];
        }
        return null;
    }
}
