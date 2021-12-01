using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day8
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				ScreenSolver("testData.txt");
				ScreenSolver("inputData.txt");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception catched: " + ex);
			}
			Console.WriteLine("Done.");
			Console.ReadLine();


		}
		/// <summary>
		/// The screen is 50 pixels wide and 6 pixels tall, all of which start off, and is capable of three somewhat peculiar operations:
		///
		///rect AxB turns on all of the pixels in a rectangle at the top-left of the screen which is A wide and B tall.
		///
		///rotate row y=A by B shifts all of the pixels in row A(0 is the top row) right by B pixels.
		/// Pixels that would fall off the right end appear at the left end of the row.
		///
		///rotate column x= A by B shifts all of the pixels in column A (0 is the left column) down by B pixels.
		/// Pixels that would fall off the bottom appear at the top of the column.
		/// </summary>
		/// <param name="inputFile"></param>
		public static void ScreenSolver(string inputFile)
		{
			const int rowMax = 6;
			const int colMax = 50;
			var screen = new Screen(rowMax, colMax);

			screen.Print();

			foreach (string line in File.ReadLines(inputFile))
			{
				Console.WriteLine(line);
				var commandLine = line.Split(' ');
				switch (commandLine[0])
				{
					case "rect":
					{
						var rectSize = commandLine[1].Split('x');
						screen.DrawRect(int.Parse(rectSize[1]), int.Parse(rectSize[0]));
						break;
					}
					case "rotate":
						{
							var rowcol = commandLine[2].Split('=');
							screen.Rotate(rowcol[0] == 'y'.ToString(), int.Parse(rowcol[1]), int.Parse(commandLine[4]));
							break;
						}
					default:
					{
						break;
					}
				}
				
				screen.Print();

			}
		}
	}
}
