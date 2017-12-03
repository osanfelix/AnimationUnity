using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	#region SINGLETON
	protected static UIManager _instance = null;
	public static UIManager instance { get { return _instance; } }
	void Awake() { _instance = this; }
	#endregion

	// Menu principal
	public GameObject mainMenu			= null;	// Panel del menu principal (Primera pantalla en mostrarse)

	// Sub-menus durante el juego
	public FinalPanelBehaviour endPanel	= null;	// Panel de fin de juego (Dentro de la interfaz del juego)
	public Text scoreText				;						// Puntuacion del juego


	public void showMainMenu()
	{
		// Mostrar objeto mainMenu
		mainMenu.SetActive(true);
		// Ocultar endPanel
		hideEndPanel();
	}

	public void hideMainMenu()
	{
		// Ocultar objeto mainMenu
		mainMenu.SetActive(false);
	}

	public void showEndPanel(bool win)
	{
		// Mostrar panel fin de juego (ganar o perder)
		if (win)	endPanel.showWin();
		else		endPanel.showLose();	
	}

	public void hideEndPanel()
	{
		// Ocultar el panel
		endPanel.hide();
	}

	public void updateScore(int score)
	{
		// Actualizar el 'UI text' con la puntuacion 
		scoreText.text = score.ToString();
	}

}
