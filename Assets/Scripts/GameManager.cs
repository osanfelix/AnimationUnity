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

	int _score = 0;
	public Text scoreText;


	// Use this for initialization
	void Start ()
	{
		foreach(SkeletonBehaviour skeleton in enemiesList)
		{
			skeleton.setPlayer(player);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	#region UI
	public void onExitButton()
	{

	}

	public void updateScoreUI()
	{
		scoreText.text = _score.ToString();
	}
	#endregion

	#region GAME
	public void killEnemy(SkeletonBehaviour enemy)
	{
		enemy.kill();
		enemiesList.Remove(enemy);
		Debug.Log(enemy.name + " eliminado.");

		// Increase score
		_score += 10;
		updateScoreUI();

		// Check kill all enemies?
		if (enemiesList.Count == 0)
		{
			// Finish!!
			Debug.Log("Finish!!!!!");
		}
	}

	public void attackPlayer()
	{
		player.recieveDamage();
	}
	#endregion
}
