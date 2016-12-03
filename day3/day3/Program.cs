using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day3
{

	/// <summary>
	/// In a valid triangle, the sum of any two sides must be larger than the remaining side. 
	/// For example, the "triangle" given above is impossible, because 5 + 10 is not larger than 25.
	/// 
	/// Part 2
	/// Now that you've helpfully marked up their design documents, 
	/// it occurs to you that triangles are specified in groups of three vertically. 
	/// Each set of three numbers in a column specifies a triangle. Rows are unrelated.
	/// </summary>
	class Program
	{
	
		static void Main(string[] args)
		{
			var possibleTrianglesRow = 0;
			var possibleTrianglesCol = 0;
			var trianglesInFile = 0;

			var rowIndex = 0;
			var threeTriangles = new Triangle[3];
			foreach (string triangleData in File.ReadLines(@"inputData.txt"))
			{
				var triangle = new Triangle(triangleData);
				threeTriangles[rowIndex] = triangle;

				rowIndex++;
				if (rowIndex >= threeTriangles.Length)
				{
					rowIndex = 0;
					var triangleX = new Triangle(threeTriangles[0].x, threeTriangles[1].x, threeTriangles[2].x);
					possibleTrianglesCol += triangleX.IsValid() ? 1 : 0;
					var triangleY = new Triangle(threeTriangles[0].y, threeTriangles[1].y, threeTriangles[2].y);
					possibleTrianglesCol += triangleY.IsValid() ? 1 : 0;
					var triangleZ = new Triangle(threeTriangles[0].z, threeTriangles[1].z, threeTriangles[2].z);
					possibleTrianglesCol += triangleZ.IsValid() ? 1 : 0;
				}

				possibleTrianglesRow += triangle.IsValid() ? 1 : 0; 
				trianglesInFile++;
			}
			Console.WriteLine($"Possible triangles per row count is {possibleTrianglesRow} of {trianglesInFile}");
			Console.WriteLine($"Possible triangles per cols count is {possibleTrianglesCol} of {trianglesInFile}");
			Console.ReadLine();
		}
	}

	public class Triangle
	{
		public int x;
		public int y;
		public int z;
		public List<int> sortedXYZ;

		public bool IsValid()
		{
			return sortedXYZ[0] + sortedXYZ[1] > sortedXYZ[2];
		}

		public Triangle(string triangleData)
		{
			var trianglePoints = triangleData.Trim().Split(' ').Select(p => int.Parse(p)).ToList();
			SetXYZ(trianglePoints);
		}


		public Triangle(int x, int y, int z)
		{
			var trianglePoints = new List<int>();
			trianglePoints.Add(x);
			trianglePoints.Add(y);
			trianglePoints.Add(z);
			SetXYZ(trianglePoints);
		}

		private void SetXYZ(List<int> trianglePoints)
		{
			x = trianglePoints[0];
			y = trianglePoints[1];
			z = trianglePoints[2];
			trianglePoints.Sort();
			sortedXYZ = trianglePoints;
		}

	}


}
