using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance;

    private bool _leftClickClicked;
    private bool _rightClickClicked;

    public bool LeftClickClicked {get { return _leftClickClicked;} private set { _leftClickClicked = value; } }
    public bool RightClickClicked { get { return _rightClickClicked; } private set { _rightClickClicked = value; } }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("left mouse clicked!");
            LeftClickClicked = true;
        }
        else
        {
            LeftClickClicked = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("right mouse clicked!");
            RightClickClicked = true;
        }
        else
        {
            RightClickClicked = false;
        }
    }
}
