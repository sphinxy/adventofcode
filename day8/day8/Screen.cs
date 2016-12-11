using System;

namespace day8
{
	public class Screen
	{
		private char offChar = '.';
		private char onChar = '#';
		private char[,] screen;

		private int RowMax => screen?.GetLength(0) ?? 0;

		private int ColMax => screen?.GetLength(1) ?? 0;

		public Screen(int row, int col)
		{
			screen = new char[row, col];
			SetSegment(RowMax, ColMax, offChar);
		}

		private void SetSegment(int toRow, int toCol, char value)
		{
			for (int r = 0; r < toRow; r++)
			{
				for (int c = 0; c < toCol; c++)
				{
					screen[r, c] = value;
				}
			}
		}

		public void Print()
		{
			var on = 0;
			Console.WriteLine(">>>");
			for (int r = 0; r < RowMax; r++)
			{
				for (int c = 0; c < ColMax; c++)
				{
					if (screen[r, c] == onChar)
					{
						on++;
					}
					
						Console.Write(screen[r, c]);
				}
				Console.WriteLine();
			}
			Console.WriteLine($"On count is {on}");
		}

		public void DrawRect(int toRow, int toCol)
		{
			SetSegment(toRow, toCol, onChar);
		}


		public void Rotate(bool isRow, int rowcol, int shift)
		{
			var max = isRow ? ColMax : RowMax;
			var temp = new char[max];
			for (int i = 0; i < max; i++)
			{
				temp[i] = isRow ? screen[rowcol, i] : screen[i, rowcol];
			}
			shift = shift % max;

			char tmp;
			for (int i = 0; i < max - shift; i++)
			{
				tmp = temp[i];
				if (isRow)
				{
					screen[rowcol, i + shift] = tmp;
				}
				else
				{
					screen[i + shift, rowcol] = tmp;
				}
			}
					
			for (int i = 0; i < shift; i++)
			{
				tmp = temp[max - shift + i];
				if (isRow)
				{
					screen[rowcol, i] = tmp;
				}
				else
				{
					screen[i, rowcol] = tmp;
				}
			}
		}
	}
}