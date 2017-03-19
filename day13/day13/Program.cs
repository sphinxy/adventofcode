	using System;
using System.Collections.Generic;
using System.Linq;
	using System.Runtime.InteropServices.WindowsRuntime;
	using System.Text;
using System.Threading.Tasks;

namespace day13
{
	class Program
	{
		static void Main(string[] args)
		{
			var testNumber = 10;
			var labirint = new ShortPathFinder(testNumber);
			var result = labirint.FindShortPath(7, 4);
			Console.WriteLine($"Answer is {result}");

			var part1number = 1362;
			labirint = new ShortPathFinder(part1number);
			result = labirint.FindShortPath(31, 39);
			Console.WriteLine($"Answer is {result}");
			Console.ReadLine();
		}
	}
	public class ShortPathFinder
	{
		private LabirintNode[,] labirintNodes;
		public int size { get; protected set; }

		public ShortPathFinder(int size)
		{
			labirintNodes = new LabirintNode[size, size];
			this.size = size;
			for (int r = 0; r < size; r++)
			{
				for (int c = 0; c < size; c++)
				{
					labirintNodes[r,c] = new LabirintNode(r, c);
				}
			}
		}

		public int FindShortPath(int destRow, int destCol)
		{

			int curRow = 1;
			int curCol = 1;
			int pathLength = 0;
			var curNode = labirintNodes[curRow, curCol];
			curNode.Visited = true;
			curNode.PathLength = pathLength;
			List<LabirintNode> nextNodes = new List<LabirintNode>();
			nextNodes.Add(curNode);
			while (nextNodes.Any())
			{
//				PrintLabirint();
				var currentLevelNodes = nextNodes.ToList();
				nextNodes.Clear();
				foreach (var node in currentLevelNodes)
				{
					node.Visited = true;
					node.PathLength = pathLength;
					if (node.Row == destRow && node.Col == destCol)
					{
						return pathLength; 
					}
					nextNodes.AddRange(NodesAround(node.Row, node.Col).Where(n => n.Visited == false && IsNodeOpen(n.Row, n.Col)).ToList());
				}
				pathLength++;

			}

			return 0;
		}

		public bool IsNodeOpen(int row, int col)
		{
			int temp = row*row + 3*row + 2*row*col + col + col*col;
			temp += size;
			var binaryTemp = Convert.ToString(temp, 2);
			var count1 = binaryTemp.Count(c=>c == '1');
			var count1Even = count1%2 == 0;
			return count1Even;
		}

		public void PrintLabirint()
		{
			Console.WriteLine();
			for (int r = 0; r < size; r++)
			{
				for (int c = 0; c < size; c++)
				{
					var node = labirintNodes[c, r];
					if (node.Visited)
					{
						Console.BackgroundColor = ConsoleColor.Blue;
					}
					Console.Write(IsNodeOpen(c, r) ? '.' : '#');
					Console.ResetColor();
				}
				Console.WriteLine();
			}
		}

		public List<LabirintNode> NodesAround(int row, int col)
		{
			var result = new List<LabirintNode>();
			for (int diff = -1; diff <= 1; diff = diff + 2)
			{
				var aroundRow = row + diff;
				var aroundCol = col + diff;
				if (aroundRow >= 0 && aroundRow < size)
				{
					result.Add(labirintNodes[aroundRow, col]);
				}
				if (aroundCol >= 0 && aroundCol < size)
				{
					result.Add(labirintNodes[row, aroundCol]);
				}
			}
			return result;
		}
	}
	public class LabirintNode
	{
		public bool Visited { get; set; }
		public int PathLength { get; set; }

		public LabirintNode(int row, int col)
		{
			Visited = false;
			PathLength = 0;
			Row = row;
			Col = col;
		}

		public int Row { get; set; }
		public int Col { get; set; }
	}

}

