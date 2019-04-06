using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {
	Image timerBar;
	public float maxTime = 5f;
	float timeLeft;
	public GameObject timesUpText;
	public Text TimeDisplay;

	public Color StartColour;
	public Color EndColour;

	void Start()
	{
		timesUpText.SetActive(false);
		timerBar = GetComponent<Image>();
		timeLeft = maxTime;
	}

	void Update()
	{
		if (timeLeft > 0)
		{
			timeLeft -= Time.deltaTime;
			timerBar.fillAmount = timeLeft/maxTime;

			TimeDisplay.text = ((timeLeft >= 600) ? "" : "0") + Mathf.FloorToInt(timeLeft / 60) + ":" + ((timeLeft % 60 > 10) ? "" : "0") + Mathf.FloorToInt(timeLeft % 60);
			timerBar.color = Color.Lerp(EndColour, StartColour, timeLeft / maxTime);
		}
		else
		{
			TimeDisplay.text = "00:00";
			timerBar.color = EndColour;

			timesUpText.SetActive(true);
			Time.timeScale = 0;
		}
	}
}
