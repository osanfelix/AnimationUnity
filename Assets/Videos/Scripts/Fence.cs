using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour {

	public Animator anim;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		anim.SetBool("Open", true);
	}

	private void OnTriggerExit(Collider other)
	{
		anim.SetBool("Open", false);
	}
}
