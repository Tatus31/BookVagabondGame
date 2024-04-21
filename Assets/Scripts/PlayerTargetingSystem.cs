using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTargetingSystem : MonoBehaviour
{
    public static PlayerTargetingSystem Instance;

    private Dictionary<GameObject, GameObject> _lockedEnemies = new Dictionary<GameObject, GameObject>();

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

        GameObject selectedCharacter = CharacterSelection.Instance.CurrentCharacterSelected;

        if (_characterTargets.ContainsKey(selectedCharacter))
        {
            Debug.Log($"{selectedCharacter.name} is already targeting an enemy.");
            return;
        }

        if (enemy != null && enemy.CompareTag("Enemy") && selectedCharacter.CompareTag("Character"))
        {
            _characterTargets[selectedCharacter] = enemy;
            Clashing.Instance.ForceClashingTarget();
            //Debug.Log($"Selected {selectedCharacter.name} targets enemy {enemy.name}");
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
    public bool IsTargetLocked(GameObject enemy)
    {
        return _lockedEnemies.ContainsKey(enemy);
    }

    public void LockTarget(GameObject enemy, GameObject character)
    {
        if (!_lockedEnemies.ContainsKey(enemy))
        {
            _lockedEnemies.Add(enemy, character);
        }
    }

    public void UnlockTarget(GameObject enemy)
    {
        if (_lockedEnemies.ContainsKey(enemy))
        {
            _lockedEnemies.Remove(enemy);
        }
    }
}
