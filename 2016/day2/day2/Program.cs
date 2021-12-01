using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace day2
{
	/// <summary>
	/// http://adventofcode.com/2016/day/2
	/// The document goes on to explain that each button to be pressed can be found by starting on the previous button 
	/// and moving to adjacent buttons on the keypad: U moves up, D moves down, L moves left, and R moves right. 
	/// Each line of instructions corresponds to one button, starting at the previous button (or, for the first line, the "5" button); 
	/// press whatever button you're on at the end of each line. If a move doesn't lead to a button, ignore it.
	/// </summary>
	class Program
	{
		private const string TestInput = "ULL\nRRDDD\nLURDL\nUUUUD";

		private const string Input =
			"DDDURLURURUDLDURRURULLRRDULRRLRLRURDLRRDUDRUDLRDUUDRRUDLLLURLUURLRURURLRLUDDURUULDURDRUUDLLDDDRLDUULLUDURRLUULUULDLDDULRLDLURURUULRURDULLLURLDRDULLULRRRLRLRULLULRULUUULRLLURURDLLRURRUUUDURRDLURUURDDLRRLUURLRRULURRDDRDULLLDRDDDDURURLLULDDULLRLDRLRRDLLURLRRUDDDRDLLRUDLLLLRLLRUDDLUUDRLRRRDRLRDLRRULRUUDUUDULLRLUDLLDDLLDLUDRURLULDLRDDLDRUDLDDLDDDRLLDUURRUUDLLULLRLDLUURRLLDRDLRRRRUUUURLUUUULRRUDDUDDRLDDURLRLRLLRRUDRDLRLDRRRRRRUDDURUUUUDDUDUDU\nRLULUULRDDRLULRDDLRDUURLRUDDDUULUUUDDRDRRRLDUURDURDRLLLRDDRLURLDRRDLRLUURULUURDRRULRULDULDLRRDDRLDRUDUDDUDDRULURLULUDRDUDDDULRRRURLRRDLRDLDLLRLUULURLDRURRRLLURRRRRLLULRRRDDLRLDDUULDLLRDDRLLUUDRURLRULULRLRUULUUUUUDRURLURLDDUDDLRDDLDRRLDLURULUUDRDLULLURDLLLRRDRURUDDURRLURRDURURDLRUDRULUULLDRLRRDRLDDUDRDLLRURURLUDUURUULDURUDULRLRDLDURRLLDRDUDRUDDRLRURUDDLRRDLLLDULRRDRDRRRLURLDLURRDULDURUUUDURLDLRURRDRULLDDLLLRUULLLLURRRLLLDRRUDDDLURLRRRDRLRDLUUUDDRULLUULDURLDUUURUDRURUDRDLRRLDRURRLRDDLLLULUDDUULDURLRUDRDDD\nRDDRUDLRLDDDRLRRLRRLUULDRLRUUURULRRLUURLLLRLULDDLDLRLULULUUDDDRLLLUDLLRUDURUDDLLDUDLURRULLRDLDURULRLDRLDLDRDDRUDRUULLLLRULULLLDDDULUUDUUDDLDRLRRDLRLURRLLDRLDLDLULRLRDLDLRLUULLDLULRRRDDRUULDUDLUUUUDUDRLUURDURRULLDRURUDURDUULRRULUULULRLDRLRLLRRRLULURLUDULLDRLDRDRULLUUUDLDUUUDLRDULRDDDDDDDDLLRDULLUDRDDRURUDDLURRUULUURURDUDLLRRRRDUDLURLLURURLRDLDUUDRURULRDURDLDRUDLRRLDLDULRRUDRDUUDRLURUURLDLUDLLRDDRDU\nLLDDDDLUDLLDUDURRURLLLLRLRRLDULLURULDULDLDLLDRRDLUDRULLRUUURDRLLURDDLLUDDLRLLRDDLULRLDDRURLUDRDULLRUDDLUURULUUURURLRULRLDLDDLRDLDLLRUURDLUDRRRDDRDRLLUDDRLDRLLLRULRDLLRLRRDDLDRDDDUDUDLUULDLDUDDLRLDUULRULDLDULDDRRLUUURUUUDLRDRULDRRLLURRRDUDULDUDUDULLULLULULURLLRRLDULDULDLRDDRRLRDRLDRLUDLLLUULLRLLRLDRDDRUDDRLLDDLRULLLULRDDDLLLDRDLRULDDDLULURDULRLDRLULDDLRUDDUDLDDDUDRDRULULDDLDLRRDURLLRLLDDURRLRRULLURLRUDDLUURULULURLRUDLLLUDDURRLURLLRLLRRLDULRRUDURLLDDRLDLRRLULUULRRUURRRDULRLRLRDDRDULULUUDULLLLURULURRUDRLL\nUULLULRUULUUUUDDRULLRLDDLRLDDLULURDDLULURDRULUURDLLUDDLDRLUDLLRUURRUDRLDRDDRRLLRULDLLRUUULLLDLDDULDRLRURLDRDUURLURDRUURUULURLRLRRURLDDDLLDDLDDDULRUDLURULLDDRLDLUDURLLLLLRULRRLLUDRUURLLURRLLRDRLLLRRDDDRRRDLRDRDUDDRLLRRDRLRLDDDLURUUUUULDULDRRRRLUDRLRDRUDUDDRULDULULDRUUDUULLUDULRLRRURDLDDUDDRDULLUURLDRDLDDUURULRDLUDDLDURUDRRRDUDRRDRLRLULDRDRLRLRRUDLLLDDDRURDRLRUDRRDDLDRRLRRDLUURLRDRRUDRRDLDDDLRDDLRDUUURRRUULLDDDLLRLDRRLLDDRLRRRLUDLRURULLDULLLUDLDLRLLDDRDRUDLRRDDLUU";
		static void Main(string[] args)
		{
			var currentButton = Prepare3X3Buttons();
			SolveButtons(currentButton);
			currentButton = Prepare5X5ButtonsCross();
			SolveButtons(currentButton);
		}

		private static Button Prepare3X3Buttons()
		{
			int shiftRow = 3;
			int shiftCol = 1;
			var Buttons = new Button[3*3+1];
			for (int i = 1; i < Buttons.Length; i++)
			{
				Buttons[i] = new Button(i.ToString());
			}
			for (int b = 0; b <= 1; b++)
			{
				for (int i = 1; i <= 3; i++)
				{
					Buttons[i + b*shiftRow].Down = Buttons[i + b*shiftRow + shiftRow];
					Buttons[i + b*shiftRow + shiftRow].Up = Buttons[i + b*shiftRow];
					Buttons[1 + shiftRow*(i - 1) + b*shiftCol].Right = Buttons[1 + shiftRow*(i - 1) + b*shiftCol + shiftCol];
					Buttons[1 + shiftRow*(i - 1) + b*shiftCol + shiftCol].Left = Buttons[1 + shiftRow*(i - 1) + b*shiftCol];
				}
			}

			var currentButton = Buttons[5];
			return currentButton;
		}

		private static Button Prepare5X5ButtonsCross()
		{
			int shiftRow = 5;
			int shiftCol = 1;
			var buttons = new Button[5*5+1];
			buttons[3] = new Button("1");

			buttons[7] = new Button("2");
			buttons[8] = new Button("3");
			buttons[9] = new Button("4");
			buttons[11] = new Button("5");
			buttons[12] = new Button("6");
			buttons[13] = new Button("7");
			buttons[14] = new Button("8");
			buttons[15] = new Button("9");
			buttons[17] = new Button("A");
			buttons[18] = new Button("B");
			buttons[19] = new Button("C");
			buttons[23] = new Button("D");


			for (int b = 0; b <= 3; b++)
			{
				for (int i = 1; i <= 5; i++)
				{
					var up = i + b*shiftRow;
					var down = i + b*shiftRow + shiftRow;
					if (buttons[up] != null)
					{
						buttons[up].Down = buttons[down];
					}
					if (buttons[down] != null)
					{
						buttons[down].Up = buttons[up];
					}
					var left = 1 + shiftRow*(i - 1) + b*shiftCol;
					var right = 1 + shiftRow*(i - 1) + b*shiftCol + shiftCol;

					if (buttons[left] != null)
					{
						buttons[left].Right = buttons[right];
					}
					if (buttons[right] != null)
					{
						buttons[right].Left = buttons[left];
					}
				}
			}
			//starts from 5
			var currentButton = buttons[11];
			return currentButton;
		}

		private static void SolveButtons(Button currentButton)
		{
			foreach (var nextButton in Input.Split('\n'))
			{
				foreach (char t in nextButton)
				{
					switch (t)
					{
						case 'U':
						{
							currentButton = currentButton.Up ?? currentButton;
							break;
						}
						case 'D':
						{
							currentButton = currentButton.Down ?? currentButton;
							break;
						}
						case 'L':
						{
							currentButton = currentButton.Left ?? currentButton;
							break;
						}
						case 'R':
						{
							currentButton = currentButton.Right ?? currentButton;
							break;
						}
						default:
						{
							throw new FormatException($"Error in command {nextButton}");
						}
					}
				}
				Console.WriteLine(currentButton.Name);
			}
			Console.ReadLine();
		}
	}

	public class Button
	{
		public string Name;
		public Button Up = null;
		public Button Down = null;
		public Button Left = null;
		public Button Right = null;

		public Button(string name)
		{
			Name = name;
		}
	}
}
