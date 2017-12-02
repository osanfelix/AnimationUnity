using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBehaviour : MonoBehaviour {

	public GameObject textWin = null;
	public GameObject textLose = null;

	Animator anim = null;

	private void Start()
	{
		if (anim == null)
			anim = GetComponent<Animator>();
	}

	public void showWin()
	{
		showPanel();
		textWin.SetActive(true);
		textLose.SetActive(false);
		startAnimation();
	}

	public void showLose()
	{
		showPanel();
		textWin.SetActive(false);
		textLose.SetActive(true);
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
