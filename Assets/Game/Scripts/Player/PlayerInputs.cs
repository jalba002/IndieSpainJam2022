using CosmosDefender;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 Move;
	public Vector2 Look;
	public bool Dash;
	public bool InteractButton;

	[Header("Movement Settings")]
	public bool AnalogMovement;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;

	private PlayerInput input;
	InputActionMap ingameMap;
	InputActionMap uiMap;
	public bool GameOver = false;
	private ResourceManager resourceManager;

	private void Awake()
    {
		input = GetComponent<PlayerInput>();
		ingameMap = input.actions.FindActionMap("Player");
		uiMap = input.actions.FindActionMap("UI");
		resourceManager = GetComponent<ResourceManager>();
	}

	public void SetInputMap(PlayerInputMaps inputMap)
    {
		switch (inputMap)
        {
			case PlayerInputMaps.Ingame:
				ingameMap.Enable();
				uiMap.Disable();
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				break;

			case PlayerInputMaps.UI:
				ingameMap.Disable();
				uiMap.Enable();
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				break;
		}
    }

	void OnGoddessMode()
    {
		if (resourceManager.SpendResource(ResourceType.Goddess, resourceManager.GetResourceData(ResourceType.Goddess).MaxResource))
		{
			GameManager.Instance.ActivateGoddessMode();
		}
	}

	void OnPause()
    {
		if (!GameOver)
		{
			PauseManager.Instance.PauseGame();

			SetInputMap(PauseManager.Instance.isGamePaused ? PlayerInputMaps.UI : PlayerInputMaps.Ingame);
		}
	}

	public void DisableInputs()
	{
		ingameMap.Disable();
		uiMap.Disable();
	}

	public void OnMove(InputValue value)
	{
		MoveInput(value.Get<Vector2>());
	}

	public void OnLook(InputValue value)
	{
		LookInput(value.Get<Vector2>());
	}

	public void OnDodge(InputValue value)
	{
		SetInputState(value.isPressed, ref Dash);
	}

	public void OnInteractButton(InputValue value)
    {
		SetInputState(value.isPressed, ref InteractButton);
	}

	public void MoveInput(Vector2 newMoveDirection)
	{
		Move = newMoveDirection;
	}

	public void LookInput(Vector2 newLookDirection)
	{
		Look = newLookDirection;
	}

	public void SetInputState(bool newState, ref bool keyRef)
	{
		keyRef = newState;
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}

	private void OnDisable()
	{
		input.actions = null;
	}
}

public enum PlayerInputMaps
{
	Ingame,
	UI
}