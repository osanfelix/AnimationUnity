using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehaviour : MonoBehaviour
{
	static int deadHash = Animator.StringToHash("Dead");
	static int attackHash = Animator.StringToHash("Attack");

	static int attackStateHash = Animator.StringToHash("Base Layer.Attack");

	Animator anim = null;
	AnimatorStateInfo stateInfo = default(AnimatorStateInfo);

	PlayerBehaviour _player = null;
	bool dead = false;

	float colliderZ = 0;

	float timeToAttack = 0;
	public void setPlayer(PlayerBehaviour player)
	{
		_player = player;
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		colliderZ = GetComponent<BoxCollider>().center.z;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		// Look for the player
		if ((_player.transform.position - transform.position).sqrMagnitude < 2 && !dead)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_player.transform.position - transform.position), Time.deltaTime * 5f);

			if(timeToAttack > 1)
			{
				attack();
				timeToAttack = Random.Range(-1, 1);
			}
			timeToAttack += Time.deltaTime;
		}
		GetComponent<BoxCollider>().center = new Vector3(GetComponent<BoxCollider>().center.x, GetComponent<BoxCollider>().center.y, colliderZ + anim.GetFloat("Distance")*0.1f);
	}

	public void attack()
	{
		anim.SetTrigger(attackHash);
	}

	public void kill()
	{
		dead = true;
		anim.SetTrigger(deadHash);
		GetComponent<Collider>().enabled = false;
	}

	private void OnCollisionStay(Collision collision)
	{
		stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if (stateInfo.fullPathHash == attackStateHash && anim.GetFloat("Distance") > 0)
		{
			if (collision.gameObject.tag == "Player")
			{
				GameManager.instance.attackPlayer();
			}
		}
	}
}
