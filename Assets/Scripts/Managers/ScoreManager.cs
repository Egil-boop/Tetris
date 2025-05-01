using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Managers
{
	public class ScoreManager : MonoBehaviour
	{
		public Action onScoreGained;
		public Action onScoreLost;

		[SerializeField]
		private TMP_Text scoreText;

		[SerializeField]
		private float countSpeed = 0.5f;

		private readonly Queue<IEnumerator> scoreQueue = new();
		private bool isRunningQueue;
		private int score;

		private void Start()
		{
			onScoreGained += ScoreGained;
			onScoreLost += ScoreLost;
			scoreText.text = score.ToString();
		}

		private void ScoreLost()
		{
			StopCoroutine(ProcessQueue());
			StopCoroutine(GainScore());
			scoreQueue.Clear();
			isRunningQueue = false;
			score = 0;
			scoreText.text = score.ToString();
		}

		private void ScoreGained()
		{
			scoreQueue.Enqueue(GainScore());

			if (!isRunningQueue)
			{
				StartCoroutine(ProcessQueue());
			}
		}

		private IEnumerator ProcessQueue()
		{
			isRunningQueue = true;

			while (scoreQueue.Count > 0)
			{
				IEnumerator next = scoreQueue.Dequeue();
				countSpeed -= 0.2f;
				yield return StartCoroutine(next);
			}

			countSpeed = 0.5f;
			isRunningQueue = false;
		}

		private IEnumerator GainScore()
		{
			for (int i = 0; i < 10; i++)
			{
				score++;
				scoreText.text = score.ToString();
				yield return new WaitForSeconds(countSpeed);
			}
		}
	}
}