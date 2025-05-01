using Utility;

public class Grid
{
	public readonly int gridSize = 30;
	private const float GridSpacing = 1f;

	public GridPos[] CreateGridFlat()
	{
		int halfGridSize = gridSize / 2;
		int width = halfGridSize;
		int height = halfGridSize;

		GridPos[] gridPosArray = new GridPos[width * height];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				int index = y * width + x;
				gridPosArray[index] = new GridPos { x = x, y = y, taken = 0 };
			}
		}

		return gridPosArray;
	}

	public float GetGridMoveAmount()
	{
		return GridSpacing;
	}
}