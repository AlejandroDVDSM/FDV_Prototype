using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	// private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private SpriteRenderer m_SpriteRenderer;

	[Header("Events")]
	[Space]

	public UnityEvent OnGroundMoveEvent;
	public UnityEvent OnJumpEvent;
	public UnityEvent OnFallEvent;
	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
		
		if (OnGroundMoveEvent == null)
			OnGroundMoveEvent = new UnityEvent();
		
		if (OnJumpEvent == null)
			OnJumpEvent = new UnityEvent();
		
		if (OnFallEvent == null)
			OnFallEvent = new UnityEvent();
		
		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool jump)
	{
		// Only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity,
				m_MovementSmoothing);

			// Flip the player
			if (move > 0)
				Flip(false);
			else if (move < 0)
				Flip(true);

			if (move != 0 && m_Grounded)
				OnGroundMoveEvent.Invoke();
		}

		// If the character is not grounded, invoke events depending on if he is jumping or falling
		if (!m_Grounded)
		{
			if (m_Rigidbody2D.velocity.y < -0.01)
				OnFallEvent.Invoke();
			else if (m_Rigidbody2D.velocity.y > 0.00)
				OnJumpEvent.Invoke();
		}
		
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	private void Flip(bool isFacingLeft)
	{
		m_SpriteRenderer.flipX = isFacingLeft;
	}
}
