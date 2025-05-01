using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Peaces
{
	public class ThreeLPeaces : MonoBehaviour, IRotatable, IOccupiedSpace
	{
		[SerializeField]
		private ThreePeaceL[] threePeaces;

		public void RotatePeaces()
		{
			foreach (var peace in threePeaces)
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
			RotateInstruction[] rotateInstructions = new RotateInstruction[threePeaces.Length];

			for (int i = 0; i < threePeaces.Length; i++)
			{
				rotateInstructions[i] = threePeaces[i].CheckValidRotation();
			}

			return rotateInstructions;
		}

		public void RotateToStandardRotation()
		{
			foreach (var peace in threePeaces)
			{
				RotateInstruction rotateInstruction = peace.ResetToStandardRotation();
				peace.transform.localPosition = new Vector3(rotateInstruction.xPos, rotateInstruction.yPos, transform.position.z);
			}
		}

		public (int, int) GetOccupiedSpace(int index)
		{
			return (threePeaces[index].currentPosX, threePeaces[index].currentPosY);
		}

		public int GetLengthOfOccupiedSpace()
		{
			return threePeaces.Length;
		}

		public (int , int )[] GetCurrentPeacePositions()
		{
			List<(int x, int y)> result = new List<(int, int)>();

			foreach (var peace in threePeaces)
			{
				result.Add((peace.currentPosX, peace.currentPosY));
			}

			return result.ToArray();
		}
	}
}