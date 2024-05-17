using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlotSelection : MonoBehaviour
{
    public static SkillSlotSelection Instance;

    public GameObject _currentSkillSlotSelected;
    public GameObject CurrentSkillSlotSelected { get { return _currentSkillSlotSelected; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(PlayerInput.Instance.RightClickClicked && CharacterSelection.Instance.SelectionCheck().CompareTag("SkillSlot"))
        {
            DeselectSkillSlot();
        }
    }

    public void SelectSkillSlot()
    {
        GameObject skillSlot = CharacterSelection.Instance.SelectionCheck();

        if (skillSlot != null && skillSlot.tag == "SkillSlot")
        {
            Debug.Log($"Selected SkillSlot for {skillSlot.transform.parent.name}");
            _currentSkillSlotSelected = skillSlot;
        }
    }

    public void DeselectSkillSlot()
    {
        _currentSkillSlotSelected = null;
        PlayerTargetingSystem.Instance.DeselectEnemy();
    }

    public GameObject GetSkillSlotForCharacter(GameObject character)
    {
        if (character == null)
        {
            Debug.LogWarning("Character GameObject is null. Cannot find associated skill slot.");
            return null;
        }

        foreach (Transform child in character.transform)
        {
            if (child.CompareTag("SkillSlot"))
            {
                GameObject skillSlot = child.gameObject;
                return skillSlot;
            }
        }

        Debug.LogWarning($"No skill slot found for {character.name}.");
        return null;
    }
}
