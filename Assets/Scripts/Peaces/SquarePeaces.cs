using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Peaces
{
	public class SquarePeaces : MonoBehaviour, IRotatable, IOccupiedSpace
	{
		[SerializeField]
		private SquarePeace[] squarePeaces;

		public void RotatePeaces()
		{
			// This type of peace does not need to rotate as it retains it form during rotation.
		}

		public RotateInstruction[] CheckIfValidRotations()
		{
			RotateInstruction[] rotateInstructions = new RotateInstruction[squarePeaces.Length];

			for (int i = 0; i < squarePeaces.Length; i++)
			{
				rotateInstructions[i] = squarePeaces[i].CheckValidRotation();
			}

			return rotateInstructions;
		}

		public void RotateToStandardRotation()
		{
			foreach (var peace in squarePeaces)
			{
				peace.ResetToStandardRotation();
			}
		}

		public (int, int) GetOccupiedSpace(int index)
		{
			return (squarePeaces[index].currentPosX, squarePeaces[index].currentPosY);
		}

		public int GetLengthOfOccupiedSpace()
		{
			return squarePeaces.Length;
		}

		public (int , int )[] GetCurrentPeacePositions()
		{
			List<(int x, int y)> result = new List<(int, int)>();

			foreach (var peace in squarePeaces)
			{
				result.Add((peace.currentPosX, peace.currentPosY));
			}

			return result.ToArray();
		}
	}
}