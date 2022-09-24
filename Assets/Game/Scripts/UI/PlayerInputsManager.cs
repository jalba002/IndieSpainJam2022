using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public static Action OnDismissAction;
    public bool GameOver = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions.FindActionMap("Player").Enable();
        playerInput.actions.FindActionMap("UI").Disable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnPause()
    {
        if (!GameOver)
        {
            PauseManager.Instance.PauseGame();

            ChangeInputs();
        }
    }

    public void ChangeInputs()
    {
        if (PauseManager.Instance.isGamePaused)
        {
            playerInput.actions.FindActionMap("Player").Disable();
            playerInput.actions.FindActionMap("UI").Enable();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            playerInput.actions.FindActionMap("Player").Enable();
            playerInput.actions.FindActionMap("UI").Disable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void DisableInputs()
    {
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("UI").Disable();
    }

    public void DisableInputsWin()
    {
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("UI").Enable();
    }

    public void OnLockCursor()
    {
        //if (Cursor.lockState == CursorLockMode.Locked)
        //{
        //    Cursor.lockState = CursorLockMode.None;
        //    Cursor.visible = true;
        //}
        //else
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = false;
        //}
        
    }
}
