using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day12
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				CodeSolver("testData.txt", 0);
				CodeSolver("inputData.txt", 0);
				CodeSolver("inputData.txt", 1);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception catched: " + ex);
			}
			Console.WriteLine("Done.");
			Console.ReadLine();
		}

		public static void CodeSolver(string inputFile, int initialCValue)
		{
			Dictionary<char, int> registers = new Dictionary<char, int>
			{
				{'a', 0}, 
				{'b', 0}, 
				{'c', initialCValue}, 
				{'d', 0}
			};
			var lines = File.ReadLines(inputFile).ToArray();

			int l = 0;

			while (l < lines.Length)
			{
				var line = lines[l];
				PrintRegisters(registers, line);
				var commandLine = line.Split(' ');
				var command = commandLine[0];
				var firstParam = commandLine[1];
					
				switch (command)
				{
					//cpy x y copies x (either an integer or the value of a register) into register y.
					case "cpy":
					{
						l++;
						var secondParam = commandLine[2];

							if (firstParam.Length == 1 && registers.ContainsKey(firstParam[0]))
						{
							registers[secondParam[0]] = registers[firstParam[0]];
							break;
						}
						registers[secondParam[0]] = Convert.ToInt32(firstParam);
						break;
					}
					//inc x increases the value of register x by one.
					case "inc":
					{
						l++;
						registers[firstParam[0]]++;
						break;
					}
					//dec x decreases the value of register x by one.
					case "dec":
					{
						l++;
						registers[firstParam[0]]--;
						break;
					}
					//jnz x y jumps to an instruction y away (positive means forward; negative means backward), but only if x is not zero.
					case "jnz":
					{
							var secondParam = commandLine[2];
							var IsRegisterAndAboveZero = registers.ContainsKey(firstParam[0]) && registers[firstParam[0]] > 0;
							var IsIntAndAboveZero = !registers.ContainsKey(firstParam[0]) && firstParam[0] > 0;
							if (IsRegisterAndAboveZero || IsIntAndAboveZero)
							{
								l += Convert.ToInt32(secondParam);
							}
							else
							{
								l++;
							}

						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException($"{commandLine[0]} is ukknown");
					}
				}

				PrintRegisters(registers);
			}
			Console.WriteLine($"Register 'a' value is {registers['a']}");
		}

		private static void PrintRegisters(Dictionary<char, int> registers, string line = null)
		{
			//Console.WriteLine(line);
			//Console.Write("   {");
			//foreach (var key in registers.Keys)
			//{
			//	Console.Write($"{key}:{registers[key]} ");
			//}
			//Console.WriteLine("}");
		}
	}
}
