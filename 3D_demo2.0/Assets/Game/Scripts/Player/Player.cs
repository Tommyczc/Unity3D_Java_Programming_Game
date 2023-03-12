using UnityEngine;
using System.Collections;
using Michsky.UI.ModernUIPack;

public class Player : MonoBehaviour {

	private Animator anim;
	private CharacterController controller;

	public float speed = 600.0f;
	public float turnSpeed = 400.0f;
	private Vector3 moveDirection = Vector3.zero;
	public float gravity = 1.0f;
	public static float mouseXSpeed = 1.0f;
	private bool isJump=false;
	public float jumpSpeed = 50f;

	public GameObject gameInteraction;

	void Start () {
		Physics.autoSyncTransforms = true;
		controller = GetComponent <CharacterController>();
		anim = gameObject.GetComponentInChildren<Animator>();

	}

	void Update (){


		//movement input
		if (Input.GetKey("w") && isJump == false && controller.isGrounded) {
			
			 anim.SetInteger("AnimationPar", 1);
			
		}
		
		
		else {
			anim.SetInteger("AnimationPar", 0);
		}

		if (Input.GetKey(KeyCode.Space) && isJump == false && controller.isGrounded)
		{
			//anim.SetInteger("AnimationPar", 0);
			//anim.SetInteger("AnimationPar", 2);
			//isJump = true;
			moveDirection.y = jumpSpeed;

		}

		if (isJump && controller.isGrounded)
		{
			isJump = false;
			//anim.SetInteger("AnimationPar", 0);
		}

		if (controller.isGrounded){
				moveDirection.x = transform.forward.x * Input.GetAxis("Vertical") * speed;
				moveDirection.z = transform.forward.z * Input.GetAxis("Vertical") * speed;
		}

		

		float turn = Input.GetAxis("Horizontal")+Input.GetAxis("Mouse X")*mouseXSpeed;
			transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
			controller.Move(moveDirection * Time.deltaTime);
			moveDirection.y -= gravity * Time.deltaTime;
	}


	private void gameLost() {
		Debug.Log("game over!!");
		gameInteraction.gameObject.GetComponent<level1_allInteraction>().game_lost();
	}
}
