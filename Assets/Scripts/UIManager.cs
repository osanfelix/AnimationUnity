using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

	#region SINGLETON
	protected static UIManager _instance = null;
	public static UIManager instance { get { return _instance; } }
	void Awake() { _instance = this; }
	#endregion

	public GameObject mainMenu = null;

	public PanelBehaviour endPanel = null;
	public Text scoreText;


	public void showMainMenu()
	{
		mainMenu.SetActive(true);
		hideEndPanel();
	}

	public void hideMainMenu()
	{
		mainMenu.SetActive(false);
	}

	public void showEndPanel(bool win)
	{
		if (win)	endPanel.showWin();
		else		endPanel.showLose();	
	}

	public void hideEndPanel()
	{
		endPanel.hide();
	}

	public void updateScore(int score)
	{
		scoreText.text = score.ToString();
	}

}
