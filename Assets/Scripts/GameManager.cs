using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	#region SINGLETON
	protected static GameManager _instance = null;
	public static GameManager instance { get { return _instance; } }
	void Awake () { _instance = this; }
	#endregion

	// Punteros a player y a todos los enemigos (lista 'enemiesList')
	public PlayerBehaviour player = null;
	public List<SkeletonBehaviour> enemiesList = null;	// No requiere inicializacion, se rellena desde el Inspector

	// Lista con los enemigos que quedan vivos
	List<SkeletonBehaviour> currentEnemiesList = null;

	// Variables internas
	int _score = 0;
	bool _soundEnabled = true;

	public bool soundEnabled	// Indica si el sonido esta activado
	{
		get;
		set;
	}

	// KEEP
	void Start ()
	{
		currentEnemiesList = new List<SkeletonBehaviour>();
		reset();	// Reiniciamos el juego

		KeyFunction.createInstance(KeyCode.R, reset);	// DELETE
	}

	private void reset()
	{
		// Reiniciamos a Player
		player.reset();

		// Incializamos la puntuacion a cero
		_score = 0;

		// Rellenamos la lista de enemigos actual. KEEP
		currentEnemiesList.Clear();
		foreach (SkeletonBehaviour skeleton in enemiesList)
		{
			skeleton.setPlayer(player);
			skeleton.reset();

			currentEnemiesList.Add(skeleton);
		}
	}

	#region UI EVENTS
	// Evento al pulsar boton 'Start'
	public void onStartGameButton()
	{
		// Ocultamos el menu principal
		UIManager.instance.hideMainMenu();

		// Actualizamos la puntuacion en el panel Score
		UIManager.instance.updateScore(_score);

		// Quitamos la pausa a Player
		player.pause = false;
	}

	// Evento al pulsar boton 'Exit'
	public void onExitGameButton()
	{
		// Mostramos el panel principal
		UIManager.instance.showMainMenu();
		
		// Reseteamos el juego
		reset();
	}
	#endregion

	#region GAME EVENTS
	// Evento al ser notificado por un enemigo (cuando muere)
	public void notifyEnemyKilled(SkeletonBehaviour enemy)
	{
		// Eliminamos enemigo de la lista actual
		currentEnemiesList.Remove(enemy);	// KEEP

		// Subimos 10 puntos y actualizamos la puntuacion en la UI
		_score += 10;
		UIManager.instance.updateScore(_score);

		// Si no quedan enemmigos
		if (currentEnemiesList.Count == 0)	// KEEP
		{
			// Mostrar panel de 'Mision cumplida' y pausar a Player
			UIManager.instance.showEndPanel(true);
			player.pause = true;
		}
	}

	// Evento al ser notificado por player (cuando muere)
	public void notifyPlayerDead()
	{
		// Mostrar panel de 'Mision fallida' y pausar a Player
		UIManager.instance.showEndPanel(false);
		player.pause = true;
	}
	#endregion
}
