using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day6
{
	class Program
	{
		/// <summary>
		/// All you need to do is figure out which character is most frequent for each position.
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			const int LineSize = 8;
			var positions = new Dictionary<char, int>[LineSize];
			for (int i = 0; i < LineSize; i++)
			{
				positions[i] = new Dictionary<char, int>();
			}
			foreach (string line in File.ReadLines(@"inputData.txt"))
			{
				for (int i = 0; i < LineSize; i++)
				{
					var currentPosition = positions[i];
					var currentChar = line[i];
					if (currentPosition.ContainsKey(currentChar))
					{
						currentPosition[currentChar]++;
					}
					else
					{
						currentPosition.Add(currentChar, 1);
					}
				}
			}
			foreach (var position in positions)
			{
				var maxItem = position.Aggregate((max, current) => max.Value > current.Value ? max : current);
				Console.Write(maxItem.Key);
			}
			Console.WriteLine();

			foreach (var position in positions)
			{
				var minItem = position.Aggregate((min, current) => min.Value < current.Value ? min : current);
				Console.Write(minItem.Key);
			}
			Console.WriteLine();
			Console.WriteLine("Done!");
			Console.ReadLine();
		}
	}
}
