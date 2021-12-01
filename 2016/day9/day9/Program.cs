using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace day9
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Solver("testDataV1.txt");
				Solver("testDataV2.txt");
				Solver("inputData.txt");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception catched: " + ex);
			}
			Console.WriteLine("Done.");
			Console.ReadLine();
		}

		private static void Solver(string inputFile)
		{
			foreach (string line in File.ReadLines(inputFile))
			{
				var decompressedLineV1 = DecompressV1(line);
				var decompressedLineLengthV2 = DecompressV2(line);
				Console.WriteLine($"Original line is {line}");
				Console.WriteLine($"Decompressed V1 line {decompressedLineV1} has length {decompressedLineV1.Length}");
				Console.WriteLine($"Decompressed V2 line has length {decompressedLineLengthV2}");

			}
		}

		private static string DecompressV1(string line)
		{
			var i = 0;
			var decodedLine = new StringBuilder(line.Length * 2);

			while (i < line.Length)
			{
				if (line[i] == '(')
				{
					var closeParenthesis = line.IndexOf(')', i);
					if (closeParenthesis > 0)
					{
						var commandText = line.Substring(i + 1, closeParenthesis - i - 1);
						var command = commandText.Split('x');
						var repeatPartLength = int.Parse(command[0]);
						var repeatPartCount = int.Parse(command[1]);
						var repeatPart = line.Substring(closeParenthesis + 1, repeatPartLength);
						for (int r = 0; r < repeatPartCount; r++)
						{
							decodedLine.Append(repeatPart);
						}
						i = closeParenthesis + 1 + repeatPartLength;
					}
				}
				else
				{
					decodedLine.Append(line[i]);
					i++;
				}
			}
			return decodedLine.ToString();
		}

		private static long DecompressV2(string line)
		{
			int i = 0;
			long decodedLength = 0;
			while (i < line.Length)
			{
				if (line[i] == '(')
				{
					var closeParenthesis = line.IndexOf(')', i);
					if (closeParenthesis > 0)
					{
						var commandText = line.Substring(i + 1, closeParenthesis - i-1);
						var command = commandText.Split('x');
						var repeatPartLength = int.Parse(command[0]);
						var repeatPartCount = long.Parse(command[1]);
						var repeatPart = line.Substring(closeParenthesis + 1, repeatPartLength);

						var internalDecompressedLength = DecompressV2(repeatPart);
						decodedLength += internalDecompressedLength*repeatPartCount;
						i = closeParenthesis + 1 + repeatPartLength;
					}
				}
				else
				{
					decodedLength++;
					i++;
				}
			}
			return decodedLength;
		}


	}
}
