using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace day4
{
	class Program
	{
		static void Main(string[] args)
		{
			var idSum = 0;
			foreach (string encryptedLine in File.ReadLines(@"inputData.txt"))
			{
				const int maxCheckSumLength = 5;
				const int idLength = 3;
				
				var symbols = new SortedDictionary<char, int>();
				var lastDashIndex = encryptedLine.LastIndexOf('-');
				var firstSquareIndex = encryptedLine.IndexOf('[');

				for (int i = 0; i < lastDashIndex; i++)
				{
					if (encryptedLine[i] != '-')
					{
						if (symbols.ContainsKey(encryptedLine[i]))
						{
							symbols[encryptedLine[i]]++;
						}
						else
						{
							symbols.Add(encryptedLine[i], 1);
						}
					}
				}
				var validCheckSum = encryptedLine.Substring(firstSquareIndex + 1, maxCheckSumLength);
				var id = int.Parse(encryptedLine.Substring(lastDashIndex + 1, idLength));
				var calculatedCheckSum = "";
				var currentMaximum = symbols.Values.Max();
				while (calculatedCheckSum.Length < maxCheckSumLength)
				{
					foreach (var symbol in symbols.ToArray())
					{
						if (symbol.Value == currentMaximum && calculatedCheckSum.Length < maxCheckSumLength)
						{
							calculatedCheckSum+= symbol.Key;
							symbols.Remove(symbol.Key);
						}
					}
					if (symbols.Values.Any())
					{
						currentMaximum = symbols.Values.Max();
					}
					else
					{
						break;
					}
				}
				if (calculatedCheckSum == validCheckSum)
				{
					idSum = idSum + id;
				}

				var decipher = RotateDecipher(encryptedLine.Substring(0, lastDashIndex), id);
				if (decipher.Contains("north"))
				{
					Console.WriteLine(decipher);
				}

			}
			Console.WriteLine(idSum);
			Console.ReadLine();
		}

		public static string RotateDecipher(string line, int rotateNumber)
		{
			var result = rotateNumber.ToString() + ' ';
			var minSymbolCode = (int)'a';
			var maxSymbolCode = (int)'z';
			var validRange = (maxSymbolCode - minSymbolCode);
			rotateNumber = rotateNumber % (validRange + 1);

			foreach (var symbol  in line)
			{
				if (symbol == '-')
				{
					result = result + ' ';
				}
				else
				{
					var symbolCode = (int) symbol;
					symbolCode = symbolCode + rotateNumber;
					if (symbolCode > maxSymbolCode)
					{
						symbolCode = symbolCode - validRange-1;
					}
					result = result + (char) symbolCode;
				}
			}
			return result;
		}
	}
}
