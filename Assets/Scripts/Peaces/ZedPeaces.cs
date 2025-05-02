using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Peaces
{
	public class ZedPeaces : MonoBehaviour, IRotatable, IOccupiedSpace
	{
		[SerializeField]
		private ZedPeace[] zedPeaces;

		public void RotatePeaces()
		{
			foreach (ZedPeace peace in zedPeaces)
			{
				RotateInstruction rotateInstruction = peace.Rotate();
				peace.transform.localPosition = new Vector3(rotateInstruction.xPos, rotateInstruction.yPos, transform.position.z);
			}
		}

		public RotateInstruction[] CheckIfValidRotations()
		{
			RotateInstruction[] rotateInstructions = new RotateInstruction[zedPeaces.Length];

			for (int i = 0; i < zedPeaces.Length; i++)
			{
				rotateInstructions[i] = zedPeaces[i].CheckValidRotation();
			}

			return rotateInstructions;
		}

		public void RotateToStandardRotation()
		{
			foreach (ZedPeace peace in zedPeaces)
			{
				RotateInstruction rotateInstruction = peace.ResetToStandardRotation();
				peace.transform.localPosition = new Vector3(rotateInstruction.xPos, rotateInstruction.yPos, transform.position.z);
			}
		}

		public (int, int) GetOccupiedSpace(int index)
		{
			return (zedPeaces[index].currentPosX, zedPeaces[index].currentPosY);
		}

		public int GetLengthOfOccupiedSpace()
		{
			return zedPeaces.Length;
		}

		public (int, int )[] GetCurrentPeacePositions()
		{
			List<(int x, int y)> result = new();

			foreach (ZedPeace peace in zedPeaces)
			{
				result.Add((peace.currentPosX, peace.currentPosY));
			}

			return result.ToArray();
		}
	}
}