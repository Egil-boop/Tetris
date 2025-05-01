using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
	private InputAction rotate;
	private InputAction moveRight;
	private InputAction moveLeft;
	private InputAction speedUp;

	public Action onRotate;
	public Action onMoveRight;
	public Action onMoveLeft;
	public Action onSpeedUpStart;
	public Action onSpeedUpStop;

	private void Start()
	{
		rotate = InputSystem.actions.FindAction("Rotate");
		moveLeft = InputSystem.actions.FindAction("MoveLeft");
		moveRight = InputSystem.actions.FindAction("MoveRight");
		speedUp = InputSystem.actions.FindAction("SpeedUp");

		rotate.performed += ctx => onRotate?.Invoke();
		moveLeft.performed += ctx => onMoveLeft?.Invoke();
		moveRight.performed += ctx => onMoveRight?.Invoke();
		speedUp.started += ctx => onSpeedUpStart?.Invoke();
		speedUp.canceled += ctx => onSpeedUpStop?.Invoke();
	}
}