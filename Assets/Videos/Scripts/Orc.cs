using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : MonoBehaviour {
	BoxCollider box;
	Animator anim = null;
	float original_z = 0;

	// Use this for initialization
	void Start () {
		box = GetComponent<BoxCollider>();
		original_z = box.center.z;
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		box.center = new Vector3(box.center.x, box.center.y, anim.GetFloat("AttackDistance") * original_z * 6);
	}
}
