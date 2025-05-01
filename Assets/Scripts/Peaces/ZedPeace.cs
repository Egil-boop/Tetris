using UnityEngine;
using Utility;

namespace Peaces
{
	public class ZedPeace : MonoBehaviour, IRotate
	{
		[SerializeField]
		private ZedPeacePos zedPeacePos;

		public int currentPosX;
		public int currentPosY;

		private int startXPos;
		private int startYPos;

		private enum ZedPeacePos
		{
			NoRotation,
			YRotation,
			XRotation
		}

		private void Awake()
		{
			startXPos = currentPosX;
			startYPos = currentPosY;
		}

		public RotateInstruction Rotate()
		{
			if (zedPeacePos == ZedPeacePos.NoRotation)
			{
				return new RotateInstruction { xPos = startXPos, yPos = startYPos };
			}

			if (zedPeacePos == ZedPeacePos.YRotation)
			{
				currentPosY *= -1;
				return new RotateInstruction { xPos = startXPos, yPos = currentPosY };
			}

			if (zedPeacePos == ZedPeacePos.XRotation)
			{
				currentPosX *= -1;
				return new RotateInstruction { xPos = currentPosX, yPos = startYPos };
			}

			throw new System.NotImplementedException();
		}

		public RotateInstruction CheckValidRotation()
		{
			if (zedPeacePos == ZedPeacePos.NoRotation)
			{
				return new RotateInstruction { xPos = currentPosX, yPos = currentPosY };
			}

			if (zedPeacePos == ZedPeacePos.YRotation)
			{
				return new RotateInstruction { xPos = currentPosX, yPos = currentPosY * -1 };
			}

			if (zedPeacePos == ZedPeacePos.XRotation)
			{
				return new RotateInstruction { xPos = currentPosX * -1, yPos = currentPosY };
			}

			throw new System.NotImplementedException();
		}

		public RotateInstruction ResetToStandardRotation()
		{
			currentPosX = startXPos;
			currentPosY = startYPos;
			return new RotateInstruction { xPos = currentPosX, yPos = currentPosY };
		}
	}
}