using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Slider slider;

    void Update()
    {
        // Check for key press in the Update method
        if (Input.GetKeyDown(KeyCode.W))
        {
            TakeDamage(10);  // Pass the damage value as a parameter
        }
    }

    public void TakeDamage(int damage)
    {
        // Check if slider is assigned to avoid null reference exception
        if (slider != null)
        {
            slider.value -= damage;  // Subtract damage from slider value
        }
        else
        {
            Debug.LogError("Slider not assigned in the Inspector");
        }
    }
}
