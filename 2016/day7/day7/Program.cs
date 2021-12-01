using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace day7
{
	class Program
	{
		/// <summary>
		/// An IP supports TLS if it has an Autonomous Bridge Bypass Annotation, or ABBA. 
		/// An ABBA is any four-character sequence which consists of a pair of two different characters followed 
		/// by the reverse of that pair, such as xyyx or abba. 
		/// However, the IP also must not have an ABBA within any hypernet sequences, 
		/// which are contained by square brackets.
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			try
			{
				AbbaSolver("testDataAbba.txt");
				AbbaSolver("testDataAba.txt");
				AbbaSolver("inputData.txt");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception catched: " + ex);
			}
			Console.WriteLine("Done.");
			Console.ReadLine();


		}

		public static void AbbaSolver(string inputFile)
		{
			var supportTls = 0;
			var supportSsl = 0;
			foreach (string line in File.ReadLines(inputFile))
			{
				var outSquare = new StringBuilder(line.Length);
				var inSquare = new StringBuilder(line.Length);
				var openedSquare = false;
				var i = 0;
				var outSquares = new List<string>();
				var inSquares = new List<string>();
				var abbaFoundOutSquare = false;
				var abbaFoundInSquare = false;
				var babFoundInSquare = false;
				while (i < line.Length)
				{
					switch (line[i])
					{
						case '[':
						{
							openedSquare = true;
							break;
						}
						case ']':
						{
							openedSquare = false;
							outSquares.Add(outSquare.ToString());
							inSquares.Add(inSquare.ToString());
							outSquare.Clear();
							inSquare.Clear();
							break;
						}
						default:
						{
							if (openedSquare)
							{
								inSquare.Append(line[i]);
							}
							else
							{
								outSquare.Append(line[i]);
							}
							break;
						}
					}
					i++;
				}
				//add last missing part 
				outSquares.Add(outSquare.ToString());

				var babList = new List<string>();
				foreach (var data in outSquares)
				{
					//abba part
					abbaFoundOutSquare = abbaFoundOutSquare || CheckAbba(data);
					//aba part
					babList.AddRange(GetBabForFoundAba(data));
				}
				
				foreach (var data in inSquares)
				{
					//abba part
					abbaFoundInSquare = abbaFoundInSquare || CheckAbba(data);
					//aba part
					foreach (var bab in babList)
					{
						babFoundInSquare = babFoundInSquare || CheckBab(data, bab);
					}
				}

				if (abbaFoundOutSquare && !abbaFoundInSquare)
				{
					supportTls++;
				}
				if (babList.Count>0 && babFoundInSquare)
				{
					supportSsl++;
				}

			}
			Console.WriteLine($"In {inputFile} there are {supportTls} rows supporting TLS ");
			Console.WriteLine($"In {inputFile} there are {supportSsl} rows supporting SLS");
		}

		private static bool CheckAbba(string line)
		{
			for (int i = 0; i < line.Length - 3; i++)
			{
				if (line[i] ==  line[i + 3] && line[i + 1] == line[i + 2] && line[i] != line[i + 1])
				{
					//Console.WriteLine($"{line.Substring(i, 4)} in {line}");
					return true;
				}
			}
			return false;
		}

		private static List<string> GetBabForFoundAba(string line)
		{
			var result = new List<string>();
			for (int i = 0; i < line.Length - 2; i++)
			{
				// check for aba
				if (line[i] == line[i + 2]  && line[i] != line[i + 1])
				{
					//Console.WriteLine($"{line.Substring(i, 3)} in {line}");
					//calculate related bab for found aba
					result.Add($"{line[i+1]}{line[i]}{line[i+1]}");
				}
			}
			return result;
		}

		private static bool CheckBab(string line, string bab)
		{
			for (int i = 0; i < line.Length - 2; i++)
			{
				if (line.Substring(i, 3) == bab)
				{
					return true;
				}
			}
			return false;
		}
	}
}
