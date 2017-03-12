using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace day11
{
	public class State
	{
		public const int ElevatorIndex = 0;
		public int ItemIndex = 1;
		public State(int floorCount, int itemCount)
		{
			FloorCount = floorCount;
			ItemCount = itemCount;
			Data = new string[floorCount, itemCount];
		}

		public State(State state)
		{
			FloorCount = state.FloorCount;
			ItemCount = state.ItemCount;
			Data = new string[FloorCount, ItemCount];
			Array.Copy(state.Data, Data, state.Data.Length);
			ItemIndex = state.ItemIndex;
		}

		public override string ToString()
		{
			var result = "";
			for (int f = FloorCount-1; f >= 0; f--)
			{
				result += "F" + (f+1) + " ";
				for (int i = 0; i < ItemCount; i++)
				{
					result += Data[f, i] == null ? ".  " : Data[f, i]+ new string(' ', (3 - Data[f, i].Length));
				}
				result += "\n\r";
			}

			return result;
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
			ValitateFloorsNearby(fromFloor, toFloor);
			for (int i = 0; i < ItemIndex; i++)
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

		private static void ValitateFloorsNearby(int fromFloor, int toFloor)
		{
			if (Math.Abs(fromFloor - toFloor) > 1)
			{
				throw new IndexOutOfRangeException("Can move only to next floor");
			}
		}


		/// <summary>
		/// All items on floor except Elevator
		/// </summary>
		/// <param name="floor"></param>
		/// <returns></returns>
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

		//+1 -1 floors aroud elevator
		public List<int> ElevatorNearbyFloors()
		{
			var nearbyFloors = new List<int>();
			
			var currentElevatorFloor = ElevatorFloor();
			for (int f = 0; f < FloorCount; f++)
			{
				if (Math.Abs(currentElevatorFloor - f) == 1)
				{
					nearbyFloors.Add(f);
				}
			}
			return nearbyFloors;
		}


		// <summary>
		/// Is state safe for exposion
		/// </summary>
		public bool IsSafe()
		{
			bool result = true;
			for (int f = 0; f < FloorCount; f++)
			{
				result = result && IsFloorSafe(f);
			}
			return result;
		}


		/// <summary>
		/// Is all items on last floor
		/// </summary>
		/// <returns></returns>
		public bool isSolved()
		{
			bool result = true;
			

			//elevator on last floor
			if (ElevatorFloor() != FloorCount-1)
				result = false;
			

			//all floors below last are empty
			for (int f = 0; f < FloorCount-1; f++)
			{
				result = result && FloorItems(f).Count == 0;
			}
			//all items are on last floot
			result = result && FloorItems(FloorCount-1).Count == ItemCount-1;
			

			return result;

		}


		/// <summary>
		/// Is floor safe for exposion
		/// </summary>
		/// <param name="floor"></param>
		/// <returns></returns>
		public bool IsFloorSafe(int floor)
		{
			ValidateFloor(floor);
			bool generatorExists = false;
			bool freeMicrochipExists = false;
			var floorItems = FloorItems(floor);
			foreach (var item in floorItems)
			{
				var pairedItem = PairedItemName(item);
				switch (ItemType(item))
				{
					case ItemTypes.Generator:
						generatorExists = true;
						break;
					case ItemTypes.Microchip:
						if (!floorItems.Contains(pairedItem))
						{
							freeMicrochipExists = true;
						}
						break;
				}
			}
			if (generatorExists && freeMicrochipExists)
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


		/// <summary>
		/// Generate reverser pair name, e.g HG for HM
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static string PairedItemName(string item)
		{
			if (item.Length == 2)
			{
				switch (item[1])
				{
					case (char)ItemTypes.Generator:
						return $"{item[0]}{(char)ItemTypes.Microchip}";
					case (char)ItemTypes.Microchip:
						return $"{item[0]}{(char)ItemTypes.Generator}";
					default:
						throw new ArgumentOutOfRangeException(nameof(item));
				}
			}
			throw new ArgumentOutOfRangeException($"{nameof(item)} as \"{item}\"");
		}


		//Element item, e.g. H for HG
		private char ItemElement(string item)
		{
			if (item.Length == 2)
			{
				return item[0];
			}
			throw new ArgumentOutOfRangeException(nameof(item));
		}


		//move items and elevator from one floor to another, up to two items 
		public State MoveItems(List<string> moveItems, int toFloor)
		{
			ValidateFloor(toFloor);
			var fromFloor = ElevatorFloor();
			ValitateFloorsNearby(fromFloor, toFloor);
			if (moveItems.Count > 2)
			{
				throw new IndexOutOfRangeException("Can move up to two items");
			}

			var floorItems = FloorItems(fromFloor);
			var newState = new State(this);

			foreach (var moveItem in moveItems)
			{
				if (!floorItems.Contains(moveItem))
				{
					throw new IndexOutOfRangeException($"Item {moveItem} is not on floor with elevator");
				}
				newState.MoveItem(fromFloor, toFloor, moveItem);
			}

			//Move elevator too!
			newState.MoveItem(fromFloor, toFloor, ((char)ItemTypes.Elevator).ToString());
			return newState;
		}
	}
}