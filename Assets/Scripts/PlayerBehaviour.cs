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

	bool _paused = false;

	public bool pause
	{
		get { return _paused; }
		set { _paused = value; }
	}

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
		if (_paused) return;

		if (Input.GetKey(KeyCode.UpArrow) || CrossButton.GetInput(InputType.UP))
		{
			if (Input.GetKey(KeyCode.RightShift))
				speed = runSpeed;
			else
				speed = walkSpeed;
		}
		else if (Input.GetKey(KeyCode.DownArrow) || CrossButton.GetInput(InputType.DOWN))
		{
			speed = -walkSpeed;
		}
		else
			speed = 0;

		if (Input.GetKey(KeyCode.LeftArrow) || CrossButton.GetInput(InputType.LEFT))
		{
			angularSpeed = -rotateSpeed;
		}
		else if (Input.GetKey(KeyCode.RightArrow) || CrossButton.GetInput(InputType.RIGHT))
		{
			angularSpeed = rotateSpeed;
		}
		else
			angularSpeed = 0;

			anim.SetFloat(speedHash, speed);

		rigid.MovePosition(transform.position + transform.forward * Time.deltaTime * anim.GetFloat(speedHash));
		rigid.MoveRotation(transform.rotation * Quaternion.Euler(transform.up * Time.deltaTime * angularSpeed));
		GetComponent<BoxCollider>().center = new Vector3(GetComponent<BoxCollider>().center.x, GetComponent<BoxCollider>().center.y, colliderZ + anim.GetFloat("Distance") * 10);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !attacking)
		{
			anim.SetTrigger(attackHash);
			Debug.Log("ATTACK");
		}
	}

	public void reset()
	{
		lives = 3;
		recievingDamage = false;
		attacking = false;

		anim.Play("Base Layer.Idle");
		anim.Play("Attack Layer.Idle");

		anim.ResetTrigger("Dead");
		anim.ResetTrigger(attackHash);

		// Set Position
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
	}

	public void recieveDamage()
	{
		stateInfo = anim.GetCurrentAnimatorStateInfo(1);
		if (!recievingDamage)
		{
			lives--;
			if (lives <= 0)
			{
				// Finish game LOSE
				GameManager.instance.playerDead();
				anim.SetTrigger("Dead");
			}
			else
			{
				anim.SetTrigger(takeDamageHash);
				recievingDamage = true;

				StartCoroutine(timeDamage());
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
