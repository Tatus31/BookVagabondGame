using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Slider slider;

    


    public HealthManager(Slider slider, int health)
    {
        this.slider = slider;
       
    }


    private void Start()
    {
        Debug.Log("Health" + " " + GetCurrentHealth());
        slider.value = 100;
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.W))
        {
           TakeDamage(10);  
        }
        Heal(10);
    }

    public void TakeDamage(int damage)
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (slider != null)
            {
                slider.value -= damage;
                Debug.Log("Health" + " " + GetCurrentHealth());
            }
            else
            {
                Debug.LogError("Slider not assigned in the Inspector");
            }
        }
        
    }

    public int GetCurrentHealth()
    {
        return (int)(slider.value);
    }


    public void Heal(int helathamount)
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            slider.value += helathamount;
        }
        
    }

}
