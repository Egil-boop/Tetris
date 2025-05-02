using System;
using System.Collections;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Managers
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] prefabs;

		[SerializeField]
		private GameObject replacementPrefab;

		[SerializeField]
		private float speed = 1;

		[SerializeField]
		private Controller controller;

		[SerializeField]
		private GamblingManager gamblingManager;

		[SerializeField]
		private ScoreManager scoreManager;

		private GameObject[] spawnedPrefabs;
		private IRotatable[] rotatables;
		private IOccupiedSpace[] occupiedSpaces;
		private GridPos[] gridPosArray;

		private GameObject[] replacements;

		private float gridSpacing;

		private GameObject activePrefab;
		private IRotatable activeRotatable;
		private IOccupiedSpace activeOccupiedSpace;

		private float moveTimer;
		private int currentX;
		private int currentY;
		private Grid grid;

		private int width;
		private int height;

		private bool isWatingForSlot;

		private void Start()
		{
			// Controller setup
			controller.onMoveLeft += TryMoveLeft;
			controller.onMoveRight += TryMoveRight;
			controller.onRotate += TryRotate;
			controller.onSpeedUpStart += Speedup;
			controller.onSpeedUpStop += SpeedDown;

			// Gambling manager setup
			gamblingManager.onJackpot += () => StartCoroutine(SlotClear(false));
			gamblingManager.onLosePoints += () => scoreManager.onScoreLost?.Invoke();
			gamblingManager.onLosePoints += () => StartCoroutine(SlotClear(true));

			//Grid flat setup
			grid = new Grid();
			gridPosArray = grid.CreateGridFlat();
			gridSpacing = grid.GetGridMoveAmount();

			replacements = new GameObject[gridPosArray.Length];

			spawnedPrefabs = new GameObject[prefabs.Length];
			rotatables = new IRotatable[prefabs.Length];
			occupiedSpaces = new IOccupiedSpace[prefabs.Length];

			width = grid.gridSize / 2;
			height = grid.gridSize / 2;

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Vector3 spawnPos = GetNewPosFromCurrentXAndY(x, y);
					GameObject instance = Instantiate(replacementPrefab, spawnPos, Quaternion.identity);
					instance.GetComponent<ColorSetter>().SetColor(y + x);
					replacements[GetIndex(x, y)] = instance;
					instance.SetActive(false);
					gridPosArray[GetIndex(x, y)].taken = 0;
				}
			}

			SpawnNewPeace();
		}

		private void Update()
		{
			if (!activePrefab)
			{
				return;
			}

			if (isWatingForSlot)
			{
				return;
			}

			moveTimer += Time.deltaTime * speed;

			if (!(moveTimer >= 1f))
			{
				return;
			}

			moveTimer = 0f;

			(int, int)[] currentPeacePositions = activeOccupiedSpace.GetCurrentPeacePositions(); // Item1 = x, item2 = y

			bool blockedSpot = false;
			bool reachedBottom = false;

			for (int k = 0; k < currentPeacePositions.Length; k++)
			{
				// Is the spot below you taken, then set blockedSpot to true.
				blockedSpot = IsTaken(currentX + currentPeacePositions[k].Item1, currentY - 1 + currentPeacePositions[k].Item2);
				reachedBottom = currentY + currentPeacePositions[k].Item2 == 0;

				if (blockedSpot || reachedBottom)
				{
					break;
				}
			}

			if (reachedBottom || blockedSpot)
			{
				for (int i = 0; i < activeOccupiedSpace.GetLengthOfOccupiedSpace(); i++)
				{
					SetTaken(currentX + activeOccupiedSpace.GetOccupiedSpace(i).Item1, currentY + activeOccupiedSpace.GetOccupiedSpace(i).Item2, 1);
					replacements[GetIndex(currentX + activeOccupiedSpace.GetOccupiedSpace(i).Item1, currentY + activeOccupiedSpace.GetOccupiedSpace(i).Item2)].SetActive(true);
				}

				SpawnNewPeace();
				return;
			}

			currentY--;
			Vector3 newPos = new(currentX * gridSpacing + transform.position.x, currentY * gridSpacing + transform.position.y, transform.position.z);
			activePrefab.transform.position = newPos;

			ClearFullRows();
		}

		private void SpawnNewPeace()
		{
			if (activePrefab)
			{
				activePrefab.transform.position = new Vector3(0, 100, 0);
			}

			currentX = Random.Range(2, width - 2);
			currentY = height - 1;
			Vector3 spawnPos = new(currentX * gridSpacing + transform.position.x, currentY * gridSpacing + transform.position.y, transform.position.z);

			int nextPrefab = Random.Range(0, prefabs.Length);

			// First time a peace is spawned we get and set the data.
			if (!activePrefab || !spawnedPrefabs[nextPrefab])
			{
				activePrefab = Instantiate(prefabs[nextPrefab], spawnPos, Quaternion.identity);
				activeRotatable = activePrefab.GetComponent<IRotatable>();
				activeOccupiedSpace = activePrefab.GetComponent<IOccupiedSpace>();

				spawnedPrefabs[nextPrefab] = activePrefab;
				rotatables[nextPrefab] = activeRotatable;
				occupiedSpaces[nextPrefab] = activeOccupiedSpace;
			}
			else
			{
				// If this peace has been spawned once we just fetch the data for that peace and set it to the active prefab.
				activePrefab = spawnedPrefabs[nextPrefab];

				activeRotatable = rotatables[nextPrefab];
				activeOccupiedSpace = occupiedSpaces[nextPrefab];
				activeRotatable.RotateToStandardRotation();

				activePrefab.transform.position = spawnPos;

				(int, int)[] currentPeacePositions = activeOccupiedSpace.GetCurrentPeacePositions(); // Item1 = x, item2 = y
				// If the peace is blocked by something when spawned we lose the game and reset.
				for (int k = 0; k < currentPeacePositions.Length; k++)
				{
					bool blockedSpot = IsTaken(currentX + currentPeacePositions[k].Item1, currentY - 1 + currentPeacePositions[k].Item2);
					if (!blockedSpot)
					{
						continue;
					}

					StartCoroutine(SlotClear(true));
					break;
				}
			}

			activePrefab.transform.localScale = new Vector3(gridSpacing, gridSpacing, gridSpacing);
		}

		private bool IsTaken(int x, int y)
		{
			try
			{
				if (y < 0 || y > height)
				{
					throw new Exception();
				}

				if (x < 0 || x >= width)
				{
					// We have a peace that is going to land outside of the grid.
					throw new Exception();
				}

				return gridPosArray[GetIndex(x, y)].taken == 1; //gridPosArray[x][y].taken == 1;
			}
			catch (Exception)
			{
				// If we somehow end up outside the grid space, we call that as the spot is taken.
				return true;
			}
		}

		private void SetTaken(int x, int y, int value)
		{
			if (y < 0)
			{
				y = 0;
			}

			int index = GetIndex(x, y);

			gridPosArray[index].taken = value;
		}

		private void ClearFullRows()
		{
			for (int y = 0; y < height; y++)
			{
				bool isRowFull = true;

				for (int x = 0; x < width; x++)
				{
					int index = GetIndex(x, y);
					if (gridPosArray[index].taken != 0)
					{
						continue;
					}

					isRowFull = false;
					break;
				}

				if (!isRowFull)
				{
					continue;
				}

				ClearRow(y);
				scoreManager.onScoreGained?.Invoke();
			}
		}

		private void ClearRow(int rowIndex)
		{
			for (int x = 0; x < width; x++)
			{
				gridPosArray[GetIndex(x, rowIndex)].taken = 0;
				replacements[GetIndex(x, rowIndex)].SetActive(false);
			}

			for (int y = rowIndex; y < height - 1; y++)
			{
				for (int x = 0; x < width; x++)
				{
					int currentIndex = GetIndex(x, y);
					int aboveIndex = GetIndex(x, y + 1);

					gridPosArray[currentIndex].taken = gridPosArray[aboveIndex].taken;
					gridPosArray[aboveIndex].taken = 0;

					replacements[GetIndex(x, y)].SetActive(gridPosArray[GetIndex(x, y)].taken == 1);
				}
			}

			for (int x = 0; x < width; x++)
			{
				int topIndex = GetIndex(x, height - 1);
				gridPosArray[topIndex].taken = 0;
			}
		}

#if UNITY_EDITOR
		[ContextMenu("RunSlot")]
		public void RuntSlot()
		{
			StartCoroutine(SlotClear(false));
		}
#endif

		private IEnumerator SlotClear(bool onLost)
		{
			// Fill all the spots as taken
			isWatingForSlot = true;


			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					SetTaken(x, y, 1);
					replacements[GetIndex(x, y)].SetActive(true);
				}
			}

			yield return new WaitForSeconds(0.5f);

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					gridPosArray[GetIndex(x, y)].taken = 0;
					replacements[GetIndex(x, y)].SetActive(false);
				}

				// Give points
				if (!onLost)
				{
					scoreManager.onScoreGained?.Invoke();
				}

				yield return new WaitForSeconds(0.08f);
			}

			if (onLost)
			{
				scoreManager.onScoreLost?.Invoke();
			}

			isWatingForSlot = false;
			yield return null;
		}

		private Vector3 GetNewPosFromCurrentXAndY(int x, int y)
		{
			return new Vector3(x * gridSpacing + transform.position.x, y * gridSpacing + transform.position.y, transform.position.z);
		}

		private void Speedup()
		{
			speed = 8;
		}

		private void SpeedDown()
		{
			speed = 2;
		}

		private int GetIndex(int x, int y)
		{
			return y * width + x;
		}

		private void TryRotate()
		{
			RotateInstruction[] rotationsInstuctions = activeRotatable.CheckIfValidRotations();
			for (int i = 0; i < rotationsInstuctions.Length; i++)
			{
				if (!IsTaken(currentX + rotationsInstuctions[i].xPos, currentY + rotationsInstuctions[i].yPos))
				{
					continue;
				}

				// We can't rotate
				return;
			}

			activeRotatable.RotatePeaces();
		}

		private void TryMoveLeft()
		{
			(int, int)[] currentPeacePositions = activeOccupiedSpace.GetCurrentPeacePositions(); // Item1 = x, item2 = y

			for (int k = 0; k < currentPeacePositions.Length; k++)
			{
				int targetX = currentX + currentPeacePositions[k].Item1 - 1; // Predict position after move

				if (targetX <= -1)
				{
					return;
				}

				if (!IsTaken(targetX, currentY + currentPeacePositions[k].Item2))
				{
					continue;
				}

				return;
			}

			currentX--;
			activePrefab.transform.position = GetNewPosFromCurrentXAndY(currentX, currentY);
		}

		private void TryMoveRight()
		{
			(int, int)[] currentPeacePositions = activeOccupiedSpace.GetCurrentPeacePositions(); // Item1 = x, item2 = y


			for (int k = 0; k < currentPeacePositions.Length; k++)
			{
				int targetX = currentX + currentPeacePositions[k].Item1 + 1; // Predict position after move

				if (targetX > width - 1)
				{
					return;
				}

				if (!IsTaken(targetX, currentY + currentPeacePositions[k].Item2))
				{
					continue;
				}

				return;
			}

			currentX++;
			activePrefab.transform.position = GetNewPosFromCurrentXAndY(currentX, currentY);
		}
	}
}