using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	static int speedHash = Animator.StringToHash("Speed");
	static int turnHash = Animator.StringToHash("Turn");
	static int crouchHash = Animator.StringToHash("Crouch");
	static int jumpHash = Animator.StringToHash("Jump");

	Animator anim;
	float h = 0;
	float v = 0;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Agacharse
		if(Input.GetKey(KeyCode.C))
			anim.SetBool(crouchHash, true);
		else
			anim.SetBool(crouchHash, false);


		// Saltar
		if (Input.GetKeyDown(KeyCode.Space))
			anim.SetTrigger(jumpHash);


		// Girar, caminar y correr
		float axisHorizontal = Input.GetAxis("Horizontal");
		h = Mathf.Lerp(h, axisHorizontal, Time.deltaTime * 10);
		v = Input.GetKey(KeyCode.LeftShift) ? Input.GetAxis("Vertical") * 2 : Input.GetAxis("Vertical");

		anim.SetFloat(speedHash, v);
		anim.SetFloat(turnHash, h);
	}
}
