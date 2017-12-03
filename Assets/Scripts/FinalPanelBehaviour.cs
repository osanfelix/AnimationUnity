using UnityEngine;
using UnityEngine.UI;

public class FinalPanelBehaviour : MonoBehaviour
{
	// Variables que apunta al texto
	public  Text finalText = null;

	Animator anim = null;

	private void Start()
	{
		if (anim == null)
			anim = GetComponent<Animator>();
	}

	public void showWin()
	{
		showPanel();
		finalText.text = "Mission completed!!";
		startAnimation();
	}

	public void showLose()
	{
		showPanel();
		finalText.text = "Mission failed!!";
		startAnimation();
	}

	public void hide()
	{
		gameObject.SetActive(false);
	}

	void showPanel()
	{
		gameObject.SetActive(true);
		anim.Play("Idle");
	}

	void startAnimation()
	{
		anim.SetTrigger("Appear");
	}
}
