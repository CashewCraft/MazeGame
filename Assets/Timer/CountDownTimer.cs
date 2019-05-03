using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {
	Image timerBar;
	public float maxTime = 5f;
	public float timeLeft;
	public GameObject timesUpText;
	public GameObject GoalText;
	public GameObject TimeText;
	public GameObject GoalDisp;
	public Text TimeDisplay;

	public Color StartColour;
	public Color EndColour;

	public static CountDownTimer ins;

	void Start()
	{
		ins = this;

		timesUpText.SetActive(false);
		GoalText.SetActive(false);
		TimeText.SetActive(false);
		timerBar = GetComponent<Image>();
		timeLeft = maxTime;
	}

	public void Win()
	{
		GoalText.SetActive(true);
		TimeText.SetActive(true);
		TimeText.GetComponent<Text>().text = ((timeLeft/ maxTime) * 100) + "% time left";
		Time.timeScale = 0;
	}

	public void SetGoal(string name, float Mt)
	{
		maxTime = Mt;
		timeLeft = Mt;
		GoalDisp.GetComponent<Text>().text = "Goal: "+name;
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
