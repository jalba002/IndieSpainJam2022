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

    private void Awake()
    {
		input = GetComponent<PlayerInput>();
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