namespace Utility
{
	interface IRotate
	{
		RotateInstruction Rotate();
		RotateInstruction CheckValidRotation();
		RotateInstruction ResetToStandardRotation();
	}

	interface IRotatable
	{
		void RotatePeaces();

		RotateInstruction[] CheckIfValidRotations();

		void RotateToStandardRotation();
	}

	interface IOccupiedSpace
	{
		(int, int) GetOccupiedSpace(int index);
		int GetLengthOfOccupiedSpace();
		(int, int)[] GetCurrentPeacePositions();
	}

	public struct RotateInstruction
	{
		public int xPos;
		public int yPos;
	}

	public struct GridPos
	{
		public int x;
		public int y;
		public int taken;
	}
}