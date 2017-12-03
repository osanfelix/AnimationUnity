using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehaviour : MonoBehaviour
{
	// Definir Hashes de:
	// Parametros (Attack, Dead)
	// Estados (Attack, Idle)
	static int deadHash = Animator.StringToHash("Dead");
	static int attackHash = Animator.StringToHash("Attack");
	static int attackStateHash = Animator.StringToHash("Base Layer.Attack");

	Animator anim = null;
	AnimatorStateInfo stateInfo = default(AnimatorStateInfo);

	// Variables auxiliares 
	PlayerBehaviour _player		= null;     //Puntero a Player (establecido por método 'setPlayer')
	bool _dead					= false;	// Indica si ya he sido eliminado
	float _originalColliderZ	= 0;
	float _timeToAttack			= 0;

	// KEEP
	public void setPlayer(PlayerBehaviour player)
	{
		_player = player;
	}

	void Start ()
	{
		// Obtener los componentes Animator y el valor original center.z del BoxCollider
		anim = GetComponent<Animator>();
		_originalColliderZ = GetComponent<BoxCollider>().center.z;
	}
	
	
	void FixedUpdate ()
	{
		// Si estoy muerto ==> No hago nada
		if (_dead) return;
		// Si Player esta a menos de 1m de mi y no estoy muerto:
		// - Le miro
		// - Si ha pasado 1s o más desde el ultimo ataque ==> attack()
		if ((_player.transform.position - transform.position).sqrMagnitude < 1)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_player.transform.position - transform.position), Time.deltaTime * 5f);

			if(_timeToAttack > 1)
			{
				attack();
				_timeToAttack = Random.Range(-1, 1);
			}
			_timeToAttack += Time.deltaTime;
		}
		GetComponent<BoxCollider>().center = new Vector3(GetComponent<BoxCollider>().center.x, GetComponent<BoxCollider>().center.y, _originalColliderZ + anim.GetFloat("Distance")*0.2f);
	}

	public void attack()
	{
		// Activo eñ trigger "Attack"
		anim.SetTrigger(attackHash);
	}

	public void kill()
	{
		// Guardo que estoy muerto, disparo trigger "Dead" y desactivo el collider
		_dead = true;
		anim.SetTrigger(deadHash);
		GetComponent<Collider>().enabled = false;

		// Notifico al GameManager que he sido eliminado
		GameManager.instance.notifyEnemyKilled(this);
	}

	// Funcion para resetear el collider (activado por defecto), la variable donde almaceno si he muerto y forzar el estado "Idle" en Animator
	public void reset()
	{
		if(anim != null)
			anim.Play("Idle");
		_dead = false;
		GetComponent<Collider>().enabled = true;
	}

	private void OnCollisionStay(Collision collision)
	{
		// Obtener el estado actual
		stateInfo = anim.GetCurrentAnimatorStateInfo(0);

		// Si el estado es 'Attack' atacamos a Player (mirar etiqueta)
		if (stateInfo.fullPathHash == attackStateHash && anim.GetFloat("Distance") > 0)
		{
			if (collision.gameObject.tag == "Player")
			{
				collision.gameObject.GetComponent<PlayerBehaviour>().recieveDamage();
			}
		}
	}
}
