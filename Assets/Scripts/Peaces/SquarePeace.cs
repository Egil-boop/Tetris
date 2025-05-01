using UnityEngine;
using Utility;

namespace Peaces
{
	public class SquarePeace : MonoBehaviour, IRotate
	{
		public int currentPosX;
		public int currentPosY;

		public RotateInstruction Rotate()
		{
			return new RotateInstruction { xPos = currentPosX, yPos = currentPosY };
		}

		public RotateInstruction CheckValidRotation()
		{
			return new RotateInstruction { xPos = currentPosX, yPos = currentPosY };
		}

		public RotateInstruction ResetToStandardRotation()
		{
			return new RotateInstruction { xPos = currentPosX, yPos = currentPosY };
		}
	}
}