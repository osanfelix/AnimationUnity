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

	public PlayerBehaviour player = null;
	public List<SkeletonBehaviour> enemiesList = null;

	List<SkeletonBehaviour> currentEnemiesList = null;

	int _score = 0;
	public bool sound = true;

	void Start ()
	{
		currentEnemiesList = new List<SkeletonBehaviour>();
		reset();

		KeyFunction.createInstance(KeyCode.R, reset);

	}

	private void reset()
	{
		player.reset();
		player.pause = false;
		_score = 0;

		currentEnemiesList.Clear();
		foreach (SkeletonBehaviour skeleton in enemiesList)
		{
			skeleton.setPlayer(player);
			skeleton.reset();

			currentEnemiesList.Add(skeleton);
		}
	}

	#region UI EVENTS
	public void onStartGameButton()
	{
		reset();
		// Hide Main Menu
		UIManager.instance.hideMainMenu();
		UIManager.instance.updateScore(0);
	}

	public void onExitGameButton()
	{
		// Show Main Menu
		UIManager.instance.showMainMenu();
		player.pause = true;
	}

	public void updateScoreUI()
	{
		UIManager.instance.updateScore(_score);
	}
	#endregion

	#region GAME
	public void killEnemy(SkeletonBehaviour enemy)
	{
		enemy.kill();
		currentEnemiesList.Remove(enemy);
		Debug.Log(enemy.name + " eliminado.");

		// Increase score
		_score += 10;
		updateScoreUI();

		// Check kill all enemies?
		if (currentEnemiesList.Count == 0)
		{
			// Finish!!
			UIManager.instance.showEndPanel(true);
			player.pause = true;
		}
	}

	public void playerDead()
	{
		UIManager.instance.showEndPanel(false);
		player.pause = true;
	}

	public void attackPlayer()
	{
		player.recieveDamage();
	}
	#endregion
}
