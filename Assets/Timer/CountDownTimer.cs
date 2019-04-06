using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {
	Image timerBar;
	public float maxTime = 5f;
	float timeLeft;
	public GameObject timesUpText;

	void Start()
	{
		timesUpText.SetActive(false);
		timerBar = GetComponent<Image>();
		timeLeft = maxTime;
	}

	void Update()
	{
		if(timeLeft > 0)
		{
			timeLeft -= Time.deltaTime;
			timerBar.fillAmount = timeLeft/maxTime;
		}
		else
		{
			{
				timesUpText.SetActive(true);
				Time.timeScale = 0;
			}
		}
	}
}
