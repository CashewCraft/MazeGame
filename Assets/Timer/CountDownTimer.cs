using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	public float Countdown = 6;
	bool ended = false;

	public static CountDownTimer ins;

	void Awake()
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
		if (!ended)
		{
			GoalText.SetActive(true);
			TimeText.SetActive(true);
			TimeText.GetComponent<Text>().text = ((timeLeft / maxTime) * 100) + "% time left";
			ended = true;
			//Time.timeScale = 0;
		}
	}

	public void SetGoal(string name, float Mt)
	{
		maxTime = Mt;
		timeLeft = Mt;
		if (GoalDisp != null)
			GoalDisp.GetComponent<Text>().text = "Goal: "+name;
	}

	void Update()
	{
		if (timeLeft > 0 && !ended)
		{
			timeLeft -= Time.deltaTime;
			timerBar.fillAmount = timeLeft/maxTime;

			TimeDisplay.text = ((timeLeft >= 600) ? "" : "0") + Mathf.FloorToInt(timeLeft / 60) + ":" + ((timeLeft % 60 > 10) ? "" : "0") + Mathf.FloorToInt(timeLeft % 60);
			timerBar.color = Color.Lerp(EndColour, StartColour, timeLeft / maxTime);
		}
		else if (timeLeft <= 0)
		{
			TimeDisplay.text = "00:00";
			timerBar.color = EndColour;

			timesUpText.SetActive(true);
			//Time.timeScale = 0;

			ended = true;
		}

		if (ended)
		{
			Countdown -= Time.deltaTime;
			if (Countdown <= 0)
			{
				SceneManager.LoadScene(0);
			}
		}
	}
}
