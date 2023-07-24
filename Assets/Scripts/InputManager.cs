using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager: MonoBehaviour
{
    // Set up input actions here
    // Set up references to other scripts here

    public static Controls inputActions = null;
    public static event Action<InputActionMap> actionMapChange;

    private void Awake()
    {
        inputActions = new Controls();
        ToggleActionMap(inputActions.Gameplay);
    }

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled)
            return;

        inputActions.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    }
}
