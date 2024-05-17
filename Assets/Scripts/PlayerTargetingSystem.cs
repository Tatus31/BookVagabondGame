using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTargetingSystem : MonoBehaviour
{
    public static PlayerTargetingSystem Instance;

    public event Action EnemySelected;

    private Dictionary<GameObject, GameObject> _lockedEnemies = new Dictionary<GameObject, GameObject>();

    private Dictionary<GameObject, GameObject> _characterTargets = new Dictionary<GameObject, GameObject>();
    public Dictionary<GameObject, GameObject> CharacterTargets { get { return _characterTargets; } }

    private SkillSlotSelection skillSlotSelection;
    private ArrowDragIndicator arrowDragIndicator;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        skillSlotSelection = SkillSlotSelection.Instance;
        arrowDragIndicator = ArrowDragIndicator.Instance;
    }

    private void Update()
    {
        if (PlayerInput.Instance == null || CharacterSelection.Instance == null)
        {
            Debug.LogWarning("PlayerInput or CharacterSelection instance not set.");
            return;
        }

        if (skillSlotSelection.CurrentSkillSlotSelected != null)
        {
            Vector3 mousePosition = MouseWorldPosition.Instance.GetMouseWorldPosition();
            arrowDragIndicator.FollowMouseLineRenderer(skillSlotSelection.CurrentSkillSlotSelected, mousePosition);
        }

        GameObject searchEnemy = CharacterSelection.Instance.SelectionCheck();

        if (searchEnemy is null)
        {
            return;
        }

        if (PlayerInput.Instance.LeftClickClicked && skillSlotSelection.CurrentSkillSlotSelected == null && searchEnemy.CompareTag("SkillSlot"))
        {
            skillSlotSelection.SelectSkillSlot();
        }
        else if (PlayerInput.Instance.LeftClickClicked && skillSlotSelection.CurrentSkillSlotSelected != null && searchEnemy.CompareTag("Enemy"))
        {
            SelectEnemy();
        }

        if (PlayerInput.Instance.RightClickClicked)
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
            EnemySelected?.Invoke();
            skillSlotSelection.DeselectSkillSlot();
        }
    }


    public void DeselectEnemy()
    {
        GameObject enemySkillSlot = CharacterSelection.Instance.SelectionCheck();

        if (enemySkillSlot != null)
        {
            GameObject selectedSkillSlot = CharacterSelection.Instance.CurrentCharacterSelected;

            if (selectedSkillSlot != null && _characterTargets.ContainsKey(selectedSkillSlot) && _characterTargets[selectedSkillSlot] == enemySkillSlot)
            {
                _characterTargets.Remove(selectedSkillSlot);
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
