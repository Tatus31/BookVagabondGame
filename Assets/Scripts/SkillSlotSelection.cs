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
        if (PlayerInput.Instance.LeftClickClicked)
        {
            SelectSkillSlot();
        }

        if (PlayerInput.Instance.RightClickClicked)
        {
            DeselectSkillSlot();
        }
    }
    private void SelectSkillSlot()
    {
        GameObject skillSlot = CharacterSelection.Instance.SelectionCheck();

        if (skillSlot != null && skillSlot.tag == "SkillSlot")
        {
            Debug.Log($"Selected SkillSlot for {skillSlot.transform.parent.name}");
            _currentSkillSlotSelected = skillSlot;
        }
    }

    private void DeselectSkillSlot()
    {
        GameObject skillSlot = CharacterSelection.Instance.SelectionCheck();

        if (skillSlot != null && _currentSkillSlotSelected != null && skillSlot.tag == "SkillSlot")
        {
            _currentSkillSlotSelected = null;
        }
    }
}
