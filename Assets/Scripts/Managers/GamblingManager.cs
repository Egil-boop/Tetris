using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Managers
{
	public class GamblingManager : MonoBehaviour
	{
		public Action onJackpot;
		public Action onLosePoints;

		[SerializeField]
		private Button button;

		private const float SpinnCooldown = 8f;

		private TMP_Text buttonText;
		private bool canGamble = true;

		private void Start()
		{
			button.onClick.AddListener(Gambling);
			buttonText = button.GetComponentInChildren<TMP_Text>();
			StartCoroutine(PulsateTextColor());
		}

		private void Gambling()
		{
			// Cooldown
			if (!canGamble)
			{
				return;
			}

			canGamble = false;

			int chance = Random.Range(1, 8);

			if (chance < 3)
			{
				onJackpot?.Invoke();
				buttonText.text = "Jackpot!";
			}
			else if (chance is > 3 and < 5)
			{
				onLosePoints?.Invoke();
				buttonText.text = "OH NO! YOU LOST";
			}
			else
			{
				buttonText.text = "Lets cool down!";
			}

			StopCoroutine(PulsateTextColor());
			StartCoroutine(UnLockGambling());
			button.interactable = false;
		}

		private IEnumerator UnLockGambling()
		{
			yield return new WaitForSeconds(SpinnCooldown);
			canGamble = true;
			button.interactable = true;
			buttonText.text = "TRY AGAIN!";
			StartCoroutine(PulsateTextColor());
		}

		private IEnumerator PulsateTextColor()
		{
			while (canGamble)
			{
				float hue = Mathf.PingPong(Time.time * 0.5f, 1f);
				buttonText.color = Color.HSVToRGB(hue, 1f, 1f);
				yield return null;
			}
		}
	}
}