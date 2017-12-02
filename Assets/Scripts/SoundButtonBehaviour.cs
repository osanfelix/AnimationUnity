using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtonBehaviour : MonoBehaviour {

	public Sprite SoundOn;
	public Sprite SoundOff;

	public Image buttonImage;
	
	public void toggleSound()
	{
		if (GameManager.instance.sound)
			buttonImage.sprite = SoundOff;
		else
			buttonImage.sprite = SoundOn;
		GameManager.instance.sound = !GameManager.instance.sound;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
