using System.Collections;
using Cinemachine;
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
	private float _cinemachineTargetYaw;
	private float _cinemachineTargetPitch;

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

	private Animator animator;
	private CharacterController controller;
	private PlayerInputs input;
	private GameObject mainCamera;

	private const float threshold = 0.01f;

	[SerializeField] private CinemachineVirtualCamera virtualCamera;

	// dodge
	[Header("Dodge")]
	[SerializeField]
	private bool isDodging = false;
	public float DodgeLaunchSpeed = 8f;
	public float DodgeUnactableTime = 0.5f;
	public float DodgeAnimationSpeed = 1f;
	private float preDodgeTargetRotation;

	[Range(0, 1)]
	public float DeadZoneRange = 0.5f;


	private void OnEnable()
	{
		input.OnDashing += Dodge;
	}

	private void OnDisable()
	{
		input.OnDashing -= Dodge;
	}

	private void Awake()
	{
		Application.targetFrameRate = 20;
		// get a reference to our main camera
		if (mainCamera == null)
		{
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}

		animator = GetComponent<Animator>();
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
		//GroundedCheck();
		Dodge();

		if (isDodging)
		{
			MoveDodge();
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
	}

	public GameObject m_KickHitBox;
	public float m_PunchComboValidTime = 0.5f;
	public bool m_PunchComboTimerDone;
	Coroutine PunchComboCo;
	public int m_PunchCombo = 0;
	public bool m_IsAttacking = false;

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
			_cinemachineTargetYaw += input.Look.x * Time.deltaTime;
			_cinemachineTargetPitch += input.Look.y * Time.deltaTime;
		}

		_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
		_cinemachineTargetPitch = Mathf.Clamp(_cinemachineTargetPitch, BottomClamp, TopClamp);

		CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
	}

	private void Move()
	{
		float targetSpeed = MoveSpeed;

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

		animationBlendForward = Mathf.Lerp(animationBlendForward, targetSpeed * inputDirection.z, Time.deltaTime * SpeedChangeRate);
		animationBlendRight = Mathf.Lerp(animationBlendRight, targetSpeed * inputDirection.x, Time.deltaTime * SpeedChangeRate);
		animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

		inputDirection = new Vector3(input.Move.x, 0.0f, input.Move.y).normalized;

		targetRotation = mainCamera.transform.eulerAngles.y;
		float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);
		transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

		animator.SetFloat(animIDSpeed, animationBlend);
		
		movement = inputDirection.z * mainCamera.transform.forward + inputDirection.x * mainCamera.transform.right;
		movement.Normalize();
		movement *= speed * Time.deltaTime;
		movement.y = verticalVelocity * Time.deltaTime;

		controller.Move(movement);
	}

	private void MoveDodge()
	{
		float targetSpeed = DodgeLaunchSpeed;
		speed = targetSpeed;

		float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, preDodgeTargetRotation, ref rotationVelocity, RotationSmoothTime);
		transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

		playerTargetRotation = preDodgeTargetRotation;

		movement = Quaternion.Euler(0.0f, playerTargetRotation, 0.0f) * Vector3.forward;
		movement.Normalize();
		movement *= speed * Time.deltaTime;
		movement.y = verticalVelocity * Time.deltaTime;

		controller.Move(movement);
	}

	IEnumerator OnDodgeTimeCo(float duration)
	{
		yield return new WaitForSeconds(duration);

		StopDodge();
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
			verticalVelocity += Gravity * Time.deltaTime;
		}
	}

	public void Dodge()
	{
		if (input.Dash && input.Move != Vector2.zero && !isDodging)
		{
			StartDodge();
		}
	}

	public void StartDodge()
	{
		animator.SetFloat("DirectionX", 0f);
		animator.SetFloat("DirectionY", 1f);

		preDodgeTargetRotation = targetRotation;

		animator.SetTrigger("Dodge");
		animator.SetBool(animIDDodging, true);
		input.Dash = false;
		StartCoroutine(OnDodgeTimeCo(DodgeUnactableTime));
		isDodging = true;
	}

	public void StopDodge()
	{
		isDodging = false;
		animator.SetBool(animIDDodging, false);
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