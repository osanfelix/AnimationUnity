using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
	// Definir Hashes de:
	// Parametros (Speed, Attack, Damage, Dead)
	// Estados (Base Layer.Idle, Attack Layer.Idle, Attack Layer.Attack)
	static int speedHash = Animator.StringToHash("Speed");
	static int attackHash = Animator.StringToHash("Attack");
	static int takeDamageHash = Animator.StringToHash("Damage");
	static int attackStateHash = Animator.StringToHash("Attack Layer.Attack");

	public float walkSpeed		= 1;		// Parametro que define la velocidad de "caminar"
	public float runSpeed		= 1;		// Parametro que define la velocidad de "correr"
	public float rotateSpeed	= 160;		// Parametro que define la velocidad de "girar"

	// Variables auxiliares
	float _angularSpeed			= 0;		// Velocidad de giro actual
	float _speed				= 0;		// Velocidad de traslacion actual
	float _originalColliderZ	= 0;		// Valora original de la posición 'z' del collider

	Animator anim = null;
	AnimatorStateInfo stateInfo = default(AnimatorStateInfo);
	Rigidbody rigid = null;

	// Variables internas:	KEEP
	int _lives = 3;
	bool _paused = false;
	public bool pause
	{
		get { return _paused; }
		set { _paused = value; }
	}

	void Start()
	{
		// Obtener los componentes Animator, Rigidbody y el valor original center.z del BoxCollider
		anim = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		_originalColliderZ = GetComponent<BoxCollider>().center.z;
	}

	// Aqui moveremos y giraremos la araña en funcion del Input
	void FixedUpdate()
	{
		// Si estoy en pausa no hacer nada (no moverme ni atacar)
		if (_paused) return;	// KEEP

		// Calculo de velocidad lineal (_speed) y angular (_angularSpeed) en función del Input
		// Si camino/corro hacia delante delante: _speed = walkSpeed   /  _speed = runSpeed
		if (Input.GetKey(KeyCode.UpArrow) || CrossButton.GetInput(InputType.UP))
		{
			if (Input.GetKey(KeyCode.RightShift))
				_speed = runSpeed;
			else
				_speed = walkSpeed;
		}
		// Si camino/corro hacia delante detras: _speed = -walkSpeed   /  _speed = -runSpeed
		else if (Input.GetKey(KeyCode.DownArrow) || CrossButton.GetInput(InputType.DOWN))
		{
			_speed = -walkSpeed;
		}
		// Si no me muevo: _speed = 0
		else
			_speed = 0;

		// Si giro izquierda: _angularSpeed = -rotateSpeed;
		if (Input.GetKey(KeyCode.LeftArrow) || CrossButton.GetInput(InputType.LEFT))
		{
			_angularSpeed = -rotateSpeed;
		}
		// Si giro derecha: _angularSpeed = rotateSpeed;
		else if (Input.GetKey(KeyCode.RightArrow) || CrossButton.GetInput(InputType.RIGHT))
		{
			_angularSpeed = rotateSpeed;
		}
		// Si no giro : _angularSpeed = 0;
		else
			_angularSpeed = 0;

		// Actualizamos el parámetro "Speed" en función de _speed. Para activar la anicación de caminar/correr
		anim.SetFloat(speedHash, _speed);

		// Movemov y rotamos el rigidbody (MovePosition y MoveRotation) en función de "_speed" y "_angularSpeed"
		rigid.MovePosition(transform.position + transform.forward * Time.deltaTime * _speed);
		rigid.MoveRotation(transform.rotation * Quaternion.Euler(transform.up * Time.deltaTime * _angularSpeed));

		// Mover el collider en función del parámetro "Distance" (necesario cuando atacamos)
		GetComponent<BoxCollider>().center = new Vector3(GetComponent<BoxCollider>().center.x, GetComponent<BoxCollider>().center.y, _originalColliderZ + anim.GetFloat("Distance") * 10);
	}

	// En este bucle solamente comprobaremos si el Input nos indica "atacar" y activaremos el trigger "Attack"
	private void Update()
	{
		if (_paused) return;	// KEEP
		if (Input.GetKeyDown(KeyCode.Space))
		{
			anim.SetTrigger(attackHash);
		}
	}

	// Función para resetear el Player
	public void reset()
	{
		//Reiniciar el numero de vidas
		_lives = 3;

		// Pausamos a Player
		_paused = true;

		// Forzar estado Idle en las dos capas (Base Layer y Attack Layer): función Play() de Animator
		anim.Play("Base Layer.Idle");
		anim.Play("Attack Layer.Idle");

		// Reseteo todos los triggers (Attack y Dead)
		anim.ResetTrigger("Dead");
		anim.ResetTrigger(attackHash);

		// Posicionar el jugador en el (0,0,0) y rotación nula (Quaternion.identity)
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
	}

	// Funcion recibir daño
	public void recieveDamage()
	{
		// Restar una vida
		// Si no me quedan vidas notificar al GameManager (notifyPlayerDead) y disparar trigger "Dead"
		_lives--;
		if (_lives <= 0)
		{
			// Notify player loses the game
			GameManager.instance.notifyPlayerDead();
			anim.SetTrigger("Dead");
		}
		// Si aun me quedan vidas dispara el trigger TakeDamage
		else
		{
			anim.SetTrigger(takeDamageHash);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		// Obtener estado actual de la capa Attack Layer
		stateInfo = anim.GetCurrentAnimatorStateInfo(1);

		// Si el estado es 'Attack' matamos al enemigo (mirar etiqueta)
		if(stateInfo.fullPathHash == attackStateHash)
		{
			if(collision.gameObject.tag == "Enemy")
			{
				collision.gameObject.GetComponent<SkeletonBehaviour>().kill();		// Mata al enemigo
			}
		}
	}
}
