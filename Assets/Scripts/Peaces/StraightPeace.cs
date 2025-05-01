using System;
using UnityEngine;
using Utility;

namespace Peaces
{
	public class StraightPeace : MonoBehaviour, IRotate
	{

		public int currentPosX;
		public int currentPosY;
		public int currentPos;

		[SerializeField]
		private Position position;

		private int startPosX;
		private int startPosY;
		
		private enum Position
		{
			Mid,
			First,
			Second,
		}

		private void Awake()
		{
			startPosX = currentPosX;
			startPosY = currentPosY;
		}

		public RotateInstruction Rotate()
		{
			if (position == Position.Mid)
			{
				return new RotateInstruction { xPos = 0, yPos = 0 };
			}

			if (position == Position.First)
			{
				switch (currentPos)
				{
					case 0:
						currentPosX = 0;
						currentPosY = -1;
						break;
					case 1:
						currentPosX = -1;
						currentPosY = 0;
						break;
					case 2:
						currentPosX = 0;
						currentPosY = 1;
						break;
					case 3:
						currentPosX = 1;
						currentPosY = 0;
						break;
				}
			}

			if (position == Position.Second)
			{
				switch (currentPos)
				{
					case 0:
						currentPosX = 0;
						currentPosY = -2;
						break;
					case 1:
						currentPosX = -2;
						currentPosY = 0;
						break;
					case 2:
						currentPosX = 0;
						currentPosY = 2;
						break;
					case 3:
						currentPosX = 2;
						currentPosY = 0;
						break;
				}
			}


			return new RotateInstruction { xPos = currentPosX, yPos = currentPosY };
		}

		public RotateInstruction CheckValidRotation()
		{
			int tempY = 0;
			int tempX = 0;

			if (position == Position.Mid)
			{
				return new RotateInstruction { xPos = 0, yPos = 0 };
			}

			if (position == Position.First)
			{
				switch (currentPos)
				{
					case 0:
						tempX = 0;
						tempY = currentPosY + -1;
						break;
					case 1:
						tempX = currentPosX + -1;
						tempY = 0;
						break;
					case 2:
						tempX = 0;
						tempY = currentPosY + 1;
						break;
					case 3:
						tempX = currentPosX + 1;
						tempY = 0;
						break;
				}
			}

			if (position == Position.Second)
			{
				switch (currentPos)
				{
					case 0:
						tempX = 0;
						tempY = currentPosY + -2;
						break;
					case 1:
						tempX = currentPosX + -2;
						tempY = 0;
						break;
					case 2:
						tempX = 0;
						tempY = currentPosY + 2;
						break;
					case 3:
						tempX = currentPosX + 2;
						tempY = 0;
						break;
				}
			}

			return new RotateInstruction { xPos = tempX, yPos = tempY };
		}

		public RotateInstruction ResetToStandardRotation()
		{
			currentPos = 0;
			currentPosY = startPosY;
			currentPosX = startPosX;
			return new RotateInstruction { xPos = startPosX, yPos = startPosY };
		}
	}
}