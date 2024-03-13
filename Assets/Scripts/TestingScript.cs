using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TestingScript : MonoBehaviour
{
    void Update()
    {
        MouseWorldPosition.Instance.GetMouseWorldPosition();
    }

}
