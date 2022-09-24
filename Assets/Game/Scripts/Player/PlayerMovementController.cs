using System.Collections;
using Cinemachine;
using CosmosDefender;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
	[Header("Player")]
	[Tooltip("Move speed of the character in m/s")]
	public float MoveSpeed = 2.0f;
	[Tooltip("How fast the character turns to face movement direction")]
	[Range(0.0f, 0.3f)]
	public float RotationSmoothTime = 0.12f;
	[Tooltip("Acceleration and deceleration")]
	public float SpeedChangeRate = 10.0f;

	[Space(10)]
	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	public float Gravity = -15.0f;

	[Space(10)]
	[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
	public float JumpTimeout = 0.50f;
	[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
	public float FallTimeout = 0.15f;

	[Header("Player Grounded")]
	[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
	public bool Grounded = true;
	[Tooltip("Useful for rough ground")]
	public float GroundedOffset = -0.14f;
	[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
	public float GroundedRadius = 0.28f;
	[Tooltip("What layers the character uses as ground")]
	public LayerMask GroundLayers;

	[Header("Cinemachine")]
	[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
	public GameObject CinemachineCameraTarget;
	[Tooltip("How far in degrees can you move the camera up")]
	public float TopClamp = 70.0f;
	[Tooltip("How far in degrees can you move the camera down")]
	public float BottomClamp = -30.0f;
	[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
	public float CameraAngleOverride = 0.0f;
	[Tooltip("For locking the camera position on all axis")]
	public bool LockCameraPosition = false;

	// cinemachine
	private float cinemachineTargetYaw;
	private float cinemachineTargetPitch;

	// player
	private float speed;
	private float animationBlend;
	private float animationBlendForward;
	private float animationBlendRight;
	private float targetRotation = 0.0f;
	private float playerTargetRotation = 0.0f;
	private float rotationVelocity;
	private float verticalVelocity;
	private float terminalVelocity = 53.0f;
	private Vector3 movement = new Vector3();
	private Vector3 inputDirection = new Vector3();
	private bool canMove = true;

	// timeout deltatime
	private float jumpTimeoutDelta;
	private float fallTimeoutDelta;

	// animation IDs
	private int animIDSpeed;
	//private int _animIDGrounded;
	//private int _animIDFreeFall;
	private int animIDRoll;
	private int animIDSpeedForward;
	private int animIDSpeedRight;
	private int animIDLocked;
	private int animIDDodging;
	private int animIDCameraVerticalDirection;

	private Animator animator;
	private CharacterController controller;
	private PlayerInputs input;
	private GameObject mainCamera;

	private const float threshold = 0.01f;

	[SerializeField] private CinemachineVirtualCamera virtualCamera;

	[Header("Dash")]
	[SerializeField]
	private bool isDashing = false;
	public float DashLaunchSpeed = 8f;
	public float DashDuration = 0.5f;
	public float DashAnimationSpeed = 1f;
	private float preDashTargetRotation;
	public Transform PlayerCenter;

	[Space(10)]
	[Header("Attributes")]
	[SerializeField] private PlayerAttributes playerAttributes;

	private void Awake()
	{
		// get a reference to our main camera
		if (mainCamera == null)
		{
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}

		animator = GetComponentInChildren<Animator>();
		controller = GetComponent<CharacterController>();
		input = GetComponent<PlayerInputs>();
	}

	private void Start()
	{
		AssignAnimationIDs();

		jumpTimeoutDelta = JumpTimeout;
		fallTimeoutDelta = FallTimeout;
	}

	private void Update()
	{
		JumpAndGravity();
		GroundedCheck();

		if (isDashing)
		{
			MoveDash();
		}
		else
		{
			Move();
		}
	}

	private void LateUpdate()
	{
		CameraRotation();
	}

	private void AssignAnimationIDs()
	{
		animIDSpeed = Animator.StringToHash("Speed");
		//_animIDGrounded = Animator.StringToHash("Grounded");
		//_animIDFreeFall = Animator.StringToHash("FreeFall");
		animIDRoll = Animator.StringToHash("Roll");
		animIDSpeedForward = Animator.StringToHash("SpeedForward");
		animIDSpeedRight = Animator.StringToHash("SpeedRight");
		animIDLocked = Animator.StringToHash("Locked");
		animIDDodging = Animator.StringToHash("Dodging");
		animIDCameraVerticalDirection = Animator.StringToHash("CameraVerticalDirection");
	}

	private void GroundedCheck()
	{
		Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
		Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
	
		//_animator.SetBool(_animIDGrounded, Grounded);
	}

	private void CameraRotation()
	{
		if (input.Look.sqrMagnitude >= threshold && !LockCameraPosition)
		{
			cinemachineTargetYaw += input.Look.x * Time.deltaTime;
			cinemachineTargetPitch += input.Look.y * Time.deltaTime;
		}

		cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
		cinemachineTargetPitch = Mathf.Clamp(cinemachineTargetPitch, BottomClamp, TopClamp);
		animator.SetFloat(animIDCameraVerticalDirection, cinemachineTargetPitch/TopClamp * -1);

		CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride, cinemachineTargetYaw, 0.0f);
	}

	private void Move()
	{
		MoveSpeed = playerAttributes.SpeedData.Speed;

		float targetSpeed = MoveSpeed * (canMove ? 1 : 0.5f);

		if (input.Move == Vector2.zero)
			targetSpeed = 0.0f;

		float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

		float speedOffset = 0.1f;
		float inputMagnitude = input.AnalogMovement ? input.Move.magnitude : 1f;

		if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
		{
			speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

			speed = Mathf.Round(speed * 1000f) / 1000f;
		}
		else
		{
			speed = targetSpeed;
		}

		animationBlendForward = Mathf.Lerp(animationBlendForward, targetSpeed / MoveSpeed * inputDirection.z * inputMagnitude, Time.deltaTime * SpeedChangeRate);
		animationBlendRight = Mathf.Lerp(animationBlendRight, targetSpeed / MoveSpeed * inputDirection.x * inputMagnitude, Time.deltaTime * SpeedChangeRate);
		animationBlend = Mathf.Lerp(animationBlend, targetSpeed / MoveSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

		inputDirection = new Vector3(input.Move.x, 0.0f, input.Move.y).normalized;

		targetRotation = mainCamera.transform.eulerAngles.y;
		float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);
		transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

		//animator.SetFloat(animIDSpeed, animationBlend);
		animator.SetFloat(animIDSpeedForward, animationBlendForward);
		animator.SetFloat(animIDSpeedRight, animationBlendRight);

		var cameraFoward = mainCamera.transform.forward;
		var cameraRight = mainCamera.transform.right;

		cameraFoward.y = 0;
		cameraRight.y = 0;

		cameraFoward.Normalize();
		cameraRight.Normalize();

		movement = inputDirection.z * cameraFoward + inputDirection.x * cameraRight;
		movement.Normalize();
		movement *= speed * Time.deltaTime;
		movement.y = verticalVelocity * Time.deltaTime;

		controller.Move(movement);
	}

	public void SetMovementState(bool newState)
    {
		canMove = newState;
	}

	private void MoveDash()
	{
		float targetSpeed = DashLaunchSpeed;
		speed = targetSpeed;

		float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, preDashTargetRotation, ref rotationVelocity, RotationSmoothTime);
		transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

		playerTargetRotation = preDashTargetRotation;

		movement = Quaternion.Euler(0.0f, playerTargetRotation, 0.0f) * Vector3.forward;
		movement.Normalize();
		movement *= speed * Time.deltaTime;
		movement.y = verticalVelocity * Time.deltaTime * 0.3f;

		controller.Move(movement);
	}

	IEnumerator OnDashTimeCo(float duration)
	{
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"));
		yield return new WaitForSeconds(duration);

		StopDash();

		yield return new WaitForSeconds(1f);
		SetMovementState(true);
	}

	private void JumpAndGravity()
	{
		if (Grounded)
		{
			fallTimeoutDelta = FallTimeout;

			//_animator.SetBool(_animIDFreeFall, false);

			if (verticalVelocity < 0.0f)
			{
				verticalVelocity = -2f;
			}

			if (jumpTimeoutDelta >= 0.0f)
			{
				jumpTimeoutDelta -= Time.deltaTime;
			}
		}
		else
		{
			jumpTimeoutDelta = JumpTimeout;

			if (fallTimeoutDelta >= 0.0f)
			{
				fallTimeoutDelta -= Time.deltaTime;
			}
			else
			{
				//_animator.SetBool(_animIDFreeFall, true);
			}
		}

		if (verticalVelocity < terminalVelocity)
		{
			verticalVelocity += Gravity * Time.deltaTime * (canMove ? 1 : 0.6f);
		}
	}

	public void Dash(SpellData data)
	{
		if (!isDashing)
		{
			DashLaunchSpeed = data.Speed;
			DashDuration = data.ActiveDuration;
			StartDash();
		}
	}

	public void StartDash()
	{
		preDashTargetRotation = targetRotation;
		input.Dash = false;
		StartCoroutine(OnDashTimeCo(DashDuration));
		isDashing = true;
		controller.Move(Vector3.zero);
	}

	public void StopDash()
	{
		isDashing = false;
		verticalVelocity = 0;
	}

	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < 0f)
			lfAngle += 360f;
		if (lfAngle > 360f)
			lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}

	private void OnDrawGizmosSelected()
	{
		Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
		Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

		if (Grounded)
			Gizmos.color = transparentGreen;
		else
			Gizmos.color = transparentRed;

		Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
	}
}