using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 *	v1.0
 *	Este script foi obtido em https://github.com/Brackeys/2D-Animation
 *  O m�todo Move foi adaptado para receber dois par�metros de acordo com a mudan�a v1.0 em CharacterController2D
 */

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D playerController2D; // deve ser referenciado em Inspector
	private Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;

	private void Awake()
	{
		// Animator � componente de Player
		animator = GetComponentInParent<Animator>();

	}

		void Update () {

		/* movimento horizontal */
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed; 

		/* anima��o de movimento */
		animator.SetFloat("move", Mathf.Abs(horizontalMove)); 
		
		/* condi��es de pulo */
		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("jump", true);
		}

	}

	// m�todo invocado pelo evento em CharacterController2D
	public void OnLanding ()
	{
		animator.SetBool("jump", false);
	}

	// m�todo invocado pelo evento em PlayerCollision
	public void HitEnemy ()
	{
		animator.SetTrigger("hit");
	}

	void FixedUpdate ()
	{
		// move o player com o FixedDeltaTime
		playerController2D.Move(horizontalMove * Time.fixedDeltaTime, jump);
		jump = false;
	}

}
