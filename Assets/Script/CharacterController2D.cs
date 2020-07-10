using System;
using UnityEngine;
using UnityEngine.Events;


/**
 *	v1.0
 *	Este script foi obtido em https://github.com/Brackeys/2D-Animation
 *  As partes comentadas referem-se a um recurso do script original, no qual o GameObject possuia 3 estados: "Jump", "Crounch" e "Move"
 *  O script foi editado para não considerar as lógicas do estado "Crounch".
 *  
 *  v1.1
 *  O GameObject precisa ter um Objeto Transform chamado "Foot" definido como filho na hierarquia
 *  Este Objeto Transform é usado para fazer a checagem de colisão com o chão
 *  m_WhatIsGround foi definida como 8
 *  
 *  v1.2
 *  O script foi editado para incluir um evento Fire que serve para disparar objetos em uma determinada direção
 *  o evento envia para o script PlayerObjectLauncher a informação de qual direção o Player está olhando
 *  Desta forma o objeto é lançado na direção correta.
 *  
 */



public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	//[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround=8;                          // A mask determining what is ground to the character
	private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	//[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	//[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	//const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	private bool m_wasFiring = false;

	[Header("Player Movement: On Landing")]
	[Space]

	public UnityEvent OnLandEvent;

	/*[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public static BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;*/


	public static event Action<bool> Fire = delegate { };

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		// Instancia o evento
		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		//if (OnCrouchEvent == null)
		//OnCrouchEvent = new BoolEvent();

		// verifica se existe um GameObject chamado Foot como filho do Player na hierarquia
		foreach (Transform child in transform)
		{
			if(child.name == "Foot")
			{
				m_GroundCheck = child;
			}
		}
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


		// se não estava atirando...
		if (!m_wasFiring)
		{
			// atire
			m_wasFiring = true;
			Fire(m_FacingRight); // envia a informação m_FacingRight para o método assinado em PlayerObjectLauncher.cs
		}
		else
		{
			m_wasFiring = false;
		}
	}

	public void Move(float move, bool jump)
	{
		/* If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}
		*/
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			/* If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}*/

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	protected void Flip()
	{
		m_FacingRight = !m_FacingRight;
		//transform.Rotate(0f, 180f, 0f);

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}