using UnityEngine;

namespace Utility
{
	/// <summary>
	/// Class is used to set UIViews to center screens.
	/// </summary>
	public class SetTransformToZero : MonoBehaviour
	{
		private void Start()
		{
			RectTransform rectTransform = GetComponent<RectTransform>();
			rectTransform.anchoredPosition = Vector3.zero;
		}
	}
}