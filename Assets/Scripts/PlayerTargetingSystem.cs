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
        if (CharacterSelection.Instance == null || SkillSlotSelection.Instance.CurrentSkillSlotSelected == null )
        {
            return;
        }

        GameObject enemySkillSlot = CharacterSelection.Instance.SelectionCheck();

        GameObject selectedSkillSlot = SkillSlotSelection.Instance.CurrentSkillSlotSelected;

        if (_characterTargets.ContainsKey(selectedSkillSlot.transform.parent.gameObject))
        {
            Debug.Log($"{selectedSkillSlot.transform.parent.gameObject.name} is already targeting an enemy.");
            return;
        }

        if (enemySkillSlot != null && enemySkillSlot.CompareTag("Enemy") && selectedSkillSlot.CompareTag("SkillSlot"))
        {
            _characterTargets[selectedSkillSlot.transform.parent.gameObject] = enemySkillSlot;
            Clashing.Instance.ForceClashingTarget();
            Debug.Log($"Selected {selectedSkillSlot.transform.parent.gameObject.name} targets enemy {enemySkillSlot.name}");
        }
    }


    private void DeselectEnemy()
    {
        GameObject enemySkillSlot = CharacterSelection.Instance.SelectionCheck();

        if (enemySkillSlot != null)
        {
            GameObject selectedSkillSlot = SkillSlotSelection.Instance.CurrentSkillSlotSelected;

            if (selectedSkillSlot != null && _characterTargets.ContainsKey(selectedSkillSlot.transform.parent.gameObject) && _characterTargets[selectedSkillSlot.transform.parent.gameObject] == enemySkillSlot)
            {
                _characterTargets.Remove(selectedSkillSlot.transform.parent.gameObject);
                Clashing.Instance.DeselectEnemy(enemySkillSlot);
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
