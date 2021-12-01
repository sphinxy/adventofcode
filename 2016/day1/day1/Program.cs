using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day1_1
{
	/// <summary>
	/// The Document indicates that you should start at the given coordinates (where you just landed) and face North. 
	/// Then, follow the provided sequence: either turn left (L) or right (R) 90 degrees, 
	/// then walk forward the given number of blocks, ending at a new intersection.
	/// </summary>
	class Program
	{
		private const string TestInput = "R8, R4, R4, R8";
		private const string Input =
			"L3, R2, L5, R1, L1, L2, L2, R1, R5, R1, L1, L2, R2, R4, L4, L3, L3, R5, L1, R3, L5, L2, R4, L5, R4, R2, L2, L1, R1, L3, L3, R2, R1, L4, L1, L1, R4, R5, R1, L2, L1, R188, R4, L3, R54, L4, R4, R74, R2, L4, R185, R1, R3, R5, L2, L3, R1, L1, L3, R3, R2, L3, L4, R1, L3, L5, L2, R2, L1, R2, R1, L4, R5, R4, L5, L5, L4, R5, R4, L5, L3, R4, R1, L5, L4, L3, R5, L5, L2, L4, R4, R4, R2, L1, L3, L2, R5, R4, L5, R1, R2, R5, L2, R4, R5, L2, L3, R3, L4, R3, L2, R1, R4, L5, R1, L5, L3, R4, L2, L2, L5, L5, R5, R2, L5, R1, L3, L2, L2, R3, L3, L4, R2, R3, L1, R2, L5, L3, R4, L4, R4, R3, L3, R1, L3, R5, L5, R1, R5, R3, L1";
		
		static void Main(string[] args)
		{
			var turns = new Coords[4];
			//up
			turns[0] = new Coords(0, 1);
			//down
			turns[2] = new Coords(0, -1);
			//right
			turns[1] = new Coords(1, 0);
			//left
			turns[3] = new Coords(-1, 0);
			var visitedPlaces = new HashSet<Coords>();

			var currentCoords = new Coords(0, 0);
			var currentTurn = 0;
			var commandList = Input.Replace(" ",String.Empty).Split(',');
			foreach (var command in commandList)
			{
				
				switch (command[0])
				{
					case 'R':
					{
						currentTurn = currentTurn + 1;
						break;
					}
					case 'L':
					{
						currentTurn = currentTurn - 1;
						break;
					}
					default:
						{ throw new FormatException($"Unknown turn for input {command}");}
				}
				if (currentTurn < 0)
				{
					currentTurn = turns.Length-1;
				}
				if (currentTurn > turns.Length-1)
				{
					currentTurn = 0;
				}
				var steps = Int32.Parse(command.Substring(1));
				var newX = currentCoords.X + turns[currentTurn].X * steps;
				var newY = currentCoords.Y + turns[currentTurn].Y * steps;
				for (int i = currentCoords.X; i != newX; i = i + turns[currentTurn].X)
				{
					var newVisited = new Coords(i, currentCoords.Y);
					if (AddVisitedAndCheckForTwice(visitedPlaces, newVisited, currentCoords)) break;
				}
				for (int i = currentCoords.Y; i != newY; i = i + turns[currentTurn].Y)
				{
					var newVisited = new Coords(currentCoords.X, i);
					if (AddVisitedAndCheckForTwice(visitedPlaces, newVisited, currentCoords)) break;
				}
				currentCoords.X = newX;
				currentCoords.Y = newY;

				

			}
			Console.Write($"We are in [{currentCoords.X},{currentCoords.Y}]");
			Console.ReadLine();

		}

		private static bool AddVisitedAndCheckForTwice(HashSet<Coords> visitedPlaces, Coords newVisited, Coords currentCoords)
		{
			if (visitedPlaces.Any(v => v.X == newVisited.X && v.Y == newVisited.Y))
			{
				Console.Write($"Place [{newVisited.X},{newVisited.Y}] visited again");
				return true;
			}
			visitedPlaces.Add(newVisited);
			return false;
		}
	}

	public class Coords
	{
		public int X;
		public int Y;

		public Coords(int x, int y)
		{
			X = x;
			Y = y;
		}

	}
}
