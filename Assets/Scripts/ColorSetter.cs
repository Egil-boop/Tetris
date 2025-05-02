using UnityEngine;
using UnityEngine.Assertions;

public class ColorSetter : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer material;

	[SerializeField]
	private Color unEvenColor;

	[SerializeField]
	private Color evenColor;

	private void Awake()
	{
		Assert.IsNotNull(material);
	}

	public void SetColor(int rowAndColumn)
	{
		material.material.color = rowAndColumn % 2 == 0 ? evenColor : unEvenColor;
	}
}