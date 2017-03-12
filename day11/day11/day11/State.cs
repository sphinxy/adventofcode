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
		public const sbyte ElevatorIndex = 0;
		public const sbyte ElevatorCode = 100;
		public sbyte ItemIndex = 1;
		public State(sbyte floorCount, sbyte itemCount)
		{
			FloorCount = floorCount;
			ItemCount = itemCount;
			Data = new sbyte[floorCount, itemCount];
			DataMap = new Dictionary<sbyte, string>();
		}

		public State(State state)
		{
			FloorCount = state.FloorCount;
			ItemCount = state.ItemCount;
			Data = new sbyte[FloorCount, ItemCount];
			Array.Copy(state.Data, Data, state.Data.Length);
			ItemIndex = state.ItemIndex;
			DataMap = state.DataMap;
		}

		public override string ToString()
		{
			var result = "";
			sbyte maxFloor = (sbyte) (FloorCount - 1);
			for (sbyte f = maxFloor; f >= 0; f--)
			{
				result += "F" + (f+1) + " ";
				for (sbyte i = 0; i < ItemCount; i++)
				{
					result += Data[f, i] == 0 ? ".  " : Data[f, i]+ new string(' ', (3 - DataMap[Data[f, i]].Length));
				}
				result += "\n\r";
			}

			return result;
		}

		private sbyte[,] Data { get; }
		private Dictionary<sbyte, string> DataMap { get; }
		public sbyte FloorCount { get; }
		public sbyte ItemCount { get; }


		public void AddItem(ItemTypes itemType, sbyte floor, char? itemElement)
		{
			ValidateFloor(floor);
			if (ItemIndex == ItemCount)
			{
				throw new IndexOutOfRangeException("No more items allowed");
			}
			if (itemType == ItemTypes.Elevator)
			{
				DataMap.Add(ElevatorCode, $"{(char) ItemTypes.Elevator}");
				Data[floor, ElevatorIndex] = ElevatorCode;
				return;
			}

			var item = $"{itemElement}{(char) itemType}";

			DataMap.Add(ItemIndex, item);
			Data[floor, ItemIndex] = ItemIndex;
			ItemIndex++;
		}

		private void MoveItem(sbyte fromFloor, sbyte toFloor, string item)
		{
			ValidateFloor(fromFloor);
			ValidateFloor(toFloor);
			ValidateFloorsNearby(fromFloor, toFloor);
			for (sbyte i = 0; i < ItemIndex; i++)
			{
				if (Data[fromFloor, i] != 0 && DataMap[Data[fromFloor,i]] == item)
				{
					if (Data[toFloor,i] != 0)
					{
						throw new IndexOutOfRangeException("Something wrong, item place is busy");
					}
					Data[toFloor, i] = Data[fromFloor, i];
					Data[fromFloor, i] = 0;
				}
			}
			
		}

		private static void ValidateFloorsNearby(sbyte fromFloor, sbyte toFloor)
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
		public List<string> FloorItems(sbyte floor)
		{
			ValidateFloor(floor);

			var result = new List<string>();
			for (sbyte j = 1; j < ItemCount; j++)
			{
				if (Data[floor, j] != 0)
				{
					result.Add(DataMap[Data[floor, j]]);
				}
			}

			return result;
		}

		private void ValidateFloor(sbyte floor)
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
		public sbyte ElevatorFloor()
		{
			for (sbyte f = 0; f < FloorCount; f++)
			{
				if (Data[f, ElevatorIndex] == ElevatorCode)
				{
					return f;
				}
			}
			return -1;
		}

		//+1 -1 floors aroud elevator
		public List<sbyte> ElevatorNearbyFloors()
		{
			var nearbyFloors = new List<sbyte>();
			
			var currentElevatorFloor = ElevatorFloor();
			for (sbyte f = 0; f < FloorCount; f++)
			{
				if (Math.Abs(currentElevatorFloor - f) == 1)
				{
					nearbyFloors.Add(f);
				}
			}
			return nearbyFloors;
		}


		/// <summary>
		/// Is state safe for exposion
		/// </summary>
		public bool IsSafe()
		{
			bool result = true;
			for (sbyte f = 0; f < FloorCount; f++)
			{
				result = result && IsFloorSafe(f);
			}
			return result;
		}


		/// <summary>
		/// Is all items on last floor
		/// </summary>
		/// <returns></returns>
		public bool IsSolved()
		{
			bool result = true;
			
			//elevator on last floor
			if (ElevatorFloor() != FloorCount-1)
				result = false;
			

			//all floors below last are empty
			for (sbyte f = 0; f < FloorCount-1; f++)
			{
				result = result && FloorItems(f).Count == 0;
			}
			//all items are on last floot
			result = result && FloorItems((sbyte) (FloorCount-1)).Count == ItemCount-1;
			

			return result;

		}


		/// <summary>
		/// Is floor safe for exposion
		/// </summary>
		/// <param name="floor"></param>
		/// <returns></returns>
		public bool IsFloorSafe(sbyte floor)
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
		public State MoveItems(List<string> moveItems, sbyte toFloor)
		{
			ValidateFloor(toFloor);
			var fromFloor = ElevatorFloor();
			ValidateFloorsNearby(fromFloor, toFloor);
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