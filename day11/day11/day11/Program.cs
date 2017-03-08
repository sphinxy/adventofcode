using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day11
{
	//http://adventofcode.com/2016/day/11
	//As a security measure, the elevator will only function if it contains at least one RTG or microchip.
	//The elevator always stops on each floor to recharge, and this takes long enough that the items within it 
	//and the items on that floor can irradiate each other.

	//In other words, if a chip is ever left in the same area as another RTG, and it's not connected to its own RTG, 
	//the chip will be fried. Therefore, it is assumed that you will follow procedure and keep chips connected to their 
	//corresponding RTG when they're in the same room, and away from other RTGs otherwise.
	public class Program
	{
		public const int F1 = 0;
		public const int F2 = 1;
		public const int F3 = 2;
		public const int F4 = 3;
		static void Main(string[] args)
		{
			var initialState = FillTestState();
		}

		//As a diagram (F# for a Floor number, E for Elevator, H for Hydrogen, L for Lithium, 
		//M for Microchip, and G for Generator), the initial state looks like this:
		//F4	.	.	.	.	.
		//F3	.	.	.	LG	.
		//F2	.	HG	.	.	.
		//F1	E	.	HM	.	LM
		public static State FillTestState()
		{
			int floorCount = 4;
			int itemCount = 4 + 1;
			var state = new State(floorCount, itemCount);
			state.AddItem(ItemTypes.Elevator, F1, null);
			state.AddItem(ItemTypes.Generator, F2, 'H');
			state.AddItem(ItemTypes.Microchip, F1, 'H');
			state.AddItem(ItemTypes.Generator, F3, 'L');
			state.AddItem(ItemTypes.Microchip, F1, 'L');
			return state;
		}
	}
}
