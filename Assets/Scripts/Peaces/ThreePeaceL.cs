using UnityEngine;
using Utility;

namespace Peaces
{
	public class ThreePeaceL : MonoBehaviour, IRotate
	{
		public int currentPosX;
		public int currentPosY;
		public int currentPos;

		private int startPos;
		private int startXPos;
		private int startYPos;

		private void Awake()
		{
			startPos = currentPos;
			startXPos = currentPosX;
			startYPos = currentPosY;
		}

		public RotateInstruction Rotate()
		{
			switch (currentPos)
			{
				case 0:
					currentPosX++;
					break;
				case 1:
					currentPosY--;
					break;
				case 2:
					currentPosX--;
					break;
				case 3:
					currentPosY++;
					break;
			}


			return new RotateInstruction { xPos = currentPosX, yPos = currentPosY };
		}

		public RotateInstruction CheckValidRotation()
		{
			int tempY = 0;
			int tempX = 0;

			switch (currentPos)
			{
				case 0:
					tempX = currentPosX + 1;
					tempY = 0;
					break;
				case 1:
					tempY = currentPosY - 1;
					tempX = 1;
					break;
				case 2:
					tempX = currentPosX - 1;
					tempY = -1;
					break;
				case 3:
					tempY = currentPosY + 1;
					tempX = 0;
					break;
			}


			return new RotateInstruction { xPos = tempX, yPos = tempY };
		}

		public RotateInstruction ResetToStandardRotation()
		{
			currentPos = startPos;
			currentPosX = startXPos;
			currentPosY = startYPos;
			return new RotateInstruction { xPos = startXPos, yPos = startYPos };
		}
	}
}