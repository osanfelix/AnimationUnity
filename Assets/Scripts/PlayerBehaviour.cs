using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
	static int speedHash = Animator.StringToHash("Speed");
	static int attackHash = Animator.StringToHash("Attack");
	static int takeDamageHash = Animator.StringToHash("Damage");
	static int attackStateHash = Animator.StringToHash("Attack Layer.Attack");
	static int takeDamageStateHash = Animator.StringToHash("Attack Layer.TakeDamage");

	public float walkSpeed = 1;
	public float runSpeed = 1;
	public float rotateSpeed = 160;


	float angularSpeed = 0;
	float speed = 0;
	float colliderZ = 0;

	Animator anim = null;
	AnimatorStateInfo stateInfo = default(AnimatorStateInfo);
	Rigidbody rigid = null;

	bool recievingDamage = false;
	bool attacking = false;

	int lives = 3;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		colliderZ = GetComponent<BoxCollider>().center.z;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			if (Input.GetKey(KeyCode.RightShift))
				speed = runSpeed;
			else
				speed = walkSpeed;
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			speed = -walkSpeed;
		}
		else
			speed = 0;

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			angularSpeed = -rotateSpeed;
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			angularSpeed = rotateSpeed;
		}
		else
			angularSpeed = 0;

			anim.SetFloat(speedHash, speed);

		if (Input.GetKeyDown(KeyCode.Space) && ! attacking)
		{
			anim.SetTrigger(attackHash);
		}

		rigid.MovePosition(transform.position + transform.forward * Time.deltaTime * anim.GetFloat(speedHash));
		rigid.MoveRotation(transform.rotation * Quaternion.Euler(transform.up * Time.deltaTime * angularSpeed));
		GetComponent<BoxCollider>().center = new Vector3(GetComponent<BoxCollider>().center.x, GetComponent<BoxCollider>().center.y, colliderZ + anim.GetFloat("Distance") * 10);
	}

	public void recieveDamage()
	{
		stateInfo = anim.GetCurrentAnimatorStateInfo(1);
		if (!recievingDamage)
		{
			anim.SetTrigger(takeDamageHash);
			recievingDamage = true;

			StartCoroutine(timeDamage());

			lives--;
			if(lives <= 0)
			{
				// Finish game LOOSE
				//TODO
			}
		}
	}

	IEnumerator timeDamage()
	{
		yield return new WaitForSeconds(1);
		recievingDamage = false;
		yield break;
	}
	IEnumerator timeAttack()
	{
		yield return new WaitForSeconds(2f);
		attacking = false;
		yield break;
	}

	private void OnCollisionStay(Collision collision)
	{
		stateInfo = anim.GetCurrentAnimatorStateInfo(1);
		if(stateInfo.fullPathHash == attackStateHash)
		{
			if(collision.gameObject.tag == "Enemy")
			{
				GameManager.instance.killEnemy(collision.gameObject.GetComponent<SkeletonBehaviour>());
			}
		}
	}
}
