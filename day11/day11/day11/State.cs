using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace day11
{
	public class State
	{
		public const int ElevatorIndex = 0;
		private int ItemIndex = 1;
		public State(int floorCount, int itemCount)
		{
			FloorCount = floorCount;
			ItemCount = itemCount;
			Data = new string[floorCount, itemCount];
		}
		private string[,] Data { get; }
		public int FloorCount { get; }
		public int ItemCount { get; }


		public void AddItem(ItemTypes itemType, int floor, char? itemElement)
		{
			ValidateFloor(floor);
			if (ItemIndex == ItemCount)
			{
				throw new IndexOutOfRangeException("No more items allowed");
			}
			if (itemType == ItemTypes.Elevator)
			{
				if (ElevatorFloor() == -1)
				{
					Data[floor, ElevatorIndex] = $"{(char)ItemTypes.Elevator}";
				}
				return;
			}

			var item = $"{itemElement}{(char) itemType}";
			Data[floor, ItemIndex++] = item;
		}

		private void MoveItem(int fromFloor, int toFloor, string item)
		{
			ValidateFloor(fromFloor);
			ValidateFloor(toFloor);
			if (Math.Abs(fromFloor - toFloor) > 1)
			{
				throw new IndexOutOfRangeException("Can move only to next floor");
			}
			for (int i = 1; i < ItemIndex; i++)
			{
				if (Data[fromFloor, i] == item)
				{
					if (Data[toFloor, i] != null)
					{
						throw new IndexOutOfRangeException("Something wrong, item place is busy");
					}
					Data[toFloor, i] = item;
					Data[fromFloor, i] = null;
				}
			}
			
		}

		public List<string> FloorItems(int floor)
		{
			ValidateFloor(floor);

			var result = new List<string>();
			for (int j = 1; j < ItemCount; j++)
			{
				if (Data[floor, j] != null)
				{
					result.Add(Data[floor, j]);
				}
			}

			return result;
		}

		private void ValidateFloor(int floor)
		{
			if (floor < 0 || floor > FloorCount)
			{
				throw new IndexOutOfRangeException("No such floor");
			}
		}

		/// <summary>
		/// Gives floor number where elevalor stays, -1 if no found
		/// </summary>
		/// <returns></returns>
		public int ElevatorFloor()
		{
			for (int f = 0; f < FloorCount; f++)
			{
				if (Data[f, ElevatorIndex] == ((char)ItemTypes.Elevator).ToString())
				{
					return f;
				}
			}
			return -1;
		}

		public bool IsSafe()
		{
			bool result = true;
			for (int f = 0; f < FloorCount; f++)
			{
				result = result && IsFloorSafe(f);
			}
			return result;
		}

		public bool IsFloorSafe(int floor)
		{
			ValidateFloor(floor);
			bool generatorExists = false;
			bool microchipExists = false;
			var floorItems = FloorItems(floor);
			foreach (var item in floorItems)
			{
				var pairedItem = PairedItemName(item);
				if (!floorItems.Contains(pairedItem))
				{
					switch (ItemType(item))
					{
						case ItemTypes.Generator:
							generatorExists = true;
							break;
						case ItemTypes.Microchip:
							microchipExists = true;
							break;
					}
				}
				
			}
			if (generatorExists && microchipExists)
			{
				return false;
			}
			return true;
		}

		public static ItemTypes ItemType(string item)
		{
			if (item.Length > 0 && item.Length <= 2)
			{
				switch (item[item.Length-1])
				{
					case (char)ItemTypes.Generator: 
					return ItemTypes.Generator;
					case (char)ItemTypes.Microchip:
						return ItemTypes.Microchip;
					case (char)ItemTypes.Elevator:
						return ItemTypes.Elevator;
					default:
						throw new ArgumentOutOfRangeException(nameof(item));
				}
			}
			throw new ArgumentOutOfRangeException($"{nameof(item)} as \"{item}\"");
		}

		public static string PairedItemName(string item)
		{
			if (item.Length == 2)
			{
				switch (item[1])
				{
					case (char)ItemTypes.Generator:
						return $"{item[0]}{ItemTypes.Microchip}";
					case (char)ItemTypes.Microchip:
						return $"{item[0]}{ItemTypes.Generator}";
					default:
						throw new ArgumentOutOfRangeException(nameof(item));
				}
			}
			throw new ArgumentOutOfRangeException($"{nameof(item)} as \"{item}\"");
		}

		public char ItemElement(string item)
		{
			if (item.Length == 2)
			{
				return item[0];
			}
			throw new ArgumentOutOfRangeException(nameof(item));
		}

		public void MoveItems(List<string> moveItems, int toFloor)
		{
			ValidateFloor(toFloor);
			if (moveItems.Count > 2)
			{
				throw new IndexOutOfRangeException("Can move up to two items");
			}
			var floorItems = FloorItems(ElevatorFloor());
			foreach (var moveItem in moveItems)
			{
				if (!floorItems.Contains(moveItem))
				{
					throw new IndexOutOfRangeException($"Item {moveItem} is not on floor with elevator");
				}
				MoveItem(ElevatorFloor(), toFloor, moveItem);
			}
		}
	}
}