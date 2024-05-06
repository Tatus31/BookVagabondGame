using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance;

    private bool _leftClickClicked;
    private bool _leftClickHeld;
    private bool _leftClickUp;
    private bool _rightClickClicked;

    public bool LeftClickClicked {get { return _leftClickClicked;} set { _leftClickClicked = value; } }
    public bool RightClickClicked { get { return _rightClickClicked; } set { _rightClickClicked = value; } }
    public bool LeftClickHeld { get { return _leftClickHeld; }  set { _leftClickHeld = value; } }
    public bool LeftClickUp { get { return _leftClickUp; } set { _leftClickUp = value; } }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        switch (Input.GetMouseButtonDown(0))
        {
            case true:
                LeftClickClicked = true;
                break;
            default:
                LeftClickClicked = false;
                break;
        }

        switch (Input.GetMouseButton(0))
        {
            case true:
                LeftClickHeld = true;
                break;
            default:
                LeftClickHeld = false;
                break;
        }

        switch (Input.GetMouseButtonUp(0))
        {
            case true:
                LeftClickUp = true;
                break;
            default:
                LeftClickUp = false;
                break;
        }

        switch (Input.GetMouseButtonDown(1))
        {
            case true:
                RightClickClicked = true;
                break;
            default:
                RightClickClicked = false;
                break;
        }
    }
}
