using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace day5
{
	class Program
	{
		const string input = "wtnhxymk";
		static void Main(string[] args)
		{

			var result = "";
			var resultPart2 = "--------";
			var index = 0;
			var watch = Stopwatch.StartNew();
			using (MD5 md5Hash = MD5.Create())
			{
				while (true)
				{
					while (true)
					{
						if (index%(1000000) == 0 && index > 0)
						{
							Console.WriteLine($"Elapsed {watch.ElapsedMilliseconds}");
							watch.Reset();
							watch.Start();
							
							Console.WriteLine(index);
						}
						byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input + index++));
						var x = BitConverter.ToInt32(data, 0);
						//first two check as is, last one check only high nibble
						if ((data[0] == 0) && (data[1] ==0)  && (data[2] >> 4 == 0))
						{
							var char6 = data[2].ToString("x2")[1];
							if (result.Length < 8)
							{
								result = result + char6;
							}
							
							var char7 = data[3].ToString("x2")[0];
							int pos;
							if (int.TryParse(char6.ToString(), out pos))
							{
								if (pos >= 0 && pos <= 7 && resultPart2[pos] == '-')
								{
									StringBuilder sb = new StringBuilder(resultPart2);
									sb[pos] = char7;
									resultPart2 = sb.ToString();
								}
							}



							Console.WriteLine($"Current results at {index - 1} are {result} and {resultPart2}");
							break;
						}
					}
				if (!resultPart2.Contains('-'))
				{
					break;
				}
				}
			}
			Console.WriteLine($"Found!");
			Console.ReadLine();

		}
	}
}
