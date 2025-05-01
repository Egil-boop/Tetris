using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Peaces
{
	public class StraightPeaces : MonoBehaviour, IRotatable, IOccupiedSpace
	{
		public StraightPeace[] straightPeaces;

		public void RotatePeaces()
		{
			foreach (var peace in straightPeaces)
			{
				RotateInstruction rotateInstruction = peace.Rotate();
				peace.transform.localPosition = new Vector3(rotateInstruction.xPos, rotateInstruction.yPos, transform.position.z);
				peace.currentPos++;
				if (peace.currentPos > 3)
				{
					peace.currentPos = 0;
				}
			}
		}

		public RotateInstruction[] CheckIfValidRotations()
		{
			RotateInstruction[] rotateInstructions = new RotateInstruction[straightPeaces.Length];

			for (int i = 0; i < straightPeaces.Length; i++)
			{
				rotateInstructions[i] = straightPeaces[i].CheckValidRotation();
			}

			return rotateInstructions;
		}

		public void RotateToStandardRotation()
		{
			foreach (StraightPeace peace in straightPeaces)
			{
				RotateInstruction rotateInstruction = peace.ResetToStandardRotation();
				peace.transform.localPosition = new Vector3(rotateInstruction.xPos, rotateInstruction.yPos, transform.position.z);
			}
		}

		public (int, int) GetOccupiedSpace(int index)
		{
			return (straightPeaces[index].currentPosX, straightPeaces[index].currentPosY);
		}

		public int GetLengthOfOccupiedSpace()
		{
			return straightPeaces.Length;
		}

		public (int , int )[] GetCurrentPeacePositions()
		{
			List<(int x, int y)> result = new();

			foreach (var peace in straightPeaces)
			{
				result.Add((peace.currentPosX, peace.currentPosY));
			}

			return result.ToArray();
		}
	}
}