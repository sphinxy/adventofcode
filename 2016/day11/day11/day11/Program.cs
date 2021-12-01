using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
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
		public const sbyte F1 = 0;
		public const sbyte F2 = 1;
		public const sbyte F3 = 2;
		public const sbyte F4 = 3;

		static void Main(string[] args)
		{

			State initialState;
			//initialState = FillTestState();
			//Solve(initialState);
			//initialState = FillRealStateOne();
			//Solve(initialState);
			initialState = FillRealStateTwo();
			Solve(initialState);

			Console.ReadLine();
		}

		private static void Solve(State initialState)
		{
			var root = new NTree<State>(initialState, null);
			var state = initialState;
			var hashes = new HashSet<string>();
			Console.WriteLine("Start solving ");


			//simple breadth-first search tree search
			int level = 1;
			int maxlevel = 5000;
			var levelNodes = MoveElevatorAllCorrectWays(state, root, hashes);
			while (levelNodes.Any() && level < maxlevel)
			{
				Console.WriteLine($"Checking level {level} with {levelNodes.Count} nodes");
				if (levelNodes.Any(n => n.data.IsSolved()))
				{
					break;
				}
				var newLevelNodes = new LinkedList<NTree<State>>();

				foreach (var child in levelNodes)
				{
					var newNodes = MoveElevatorAllCorrectWays(child.data, child, hashes);
					newLevelNodes = new LinkedList<NTree<State>>(newLevelNodes.Concat(newNodes));
				}
				Console.WriteLine("Move done");
				level++;
				levelNodes = newLevelNodes;
			}
			
			Console.WriteLine("Solving complete, here is results:");
			var solutionNode = PrintDoneNodes(root, 1);
			PrintSolutionPath(solutionNode);
			Console.WriteLine("End solving");
		}

		private static void PrintSolutionPath(NTree<State> solutionNode)
		{
			var level = 0;
			Console.WriteLine("Level " + level + "\r\n" + (solutionNode?.data.ToString() ?? "No solution.."));
			var parentState = solutionNode?.parent;
			while (parentState != null)
			{
				level++;
				Console.WriteLine("Level " + level + "\r\n" + parentState.data);
				parentState = parentState.parent;
			}
		}

		static NTree<State> PrintDoneNodes(NTree<State> node, sbyte level)
		{
			
			if (node.data.IsSolved())
			{
				return node;

			}
			NTree<State> result;
			foreach (var child in node.children)
			{
				level = (sbyte) (level + 1);
				result = PrintDoneNodes(child, level);
				if (result != null)
				{
					return result;
				}
			}
			return null;
		}


		private static LinkedList<NTree<State>> MoveElevatorAllCorrectWays(State state, NTree<State> treeNode, HashSet<string> hashes)
		{
			var itemsOnFloorWithElevator = state.FloorItems(state.ElevatorFloor);

			//all unique possible pairs of items from state, in assumption that  HG LM eq LM HG
			var combinations = (from item1 in itemsOnFloorWithElevator
				from item2 in itemsOnFloorWithElevator
				where string.CompareOrdinal(item1, item2) < 0
				select Tuple.Create(item1, item2)).ToList();
		
			foreach (var nearbyFloor in state.ElevatorNearbyFloors())
			{
				//concat of single item and pairs of item
				var allPossibleItems = itemsOnFloorWithElevator.Select(item => new List<string> {item})
						.Concat(combinations.Select(c => new List<string> {c.Item1, c.Item2}));
				foreach (var item in allPossibleItems)
				{
					var newState = state.MoveItems(item, nearbyFloor);
					
					var newStateHash = newState.ToHashString();
					if (!hashes.Contains(newStateHash))
					{
						hashes.Add(newStateHash);
						if (newState.IsSafe())
						{
							treeNode.AddChild(newState);
						}
					}
				}
			}

			return treeNode.children;


		}

		public static System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

		public static string CreateMD5(string input)
		{
			
			byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
			byte[] hashBytes = md5.ComputeHash(inputBytes);

			// Convert the byte array to hexadecimal string
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hashBytes.Length; i++)
			{
				sb.Append(hashBytes[i].ToString("X2"));
			}
			return sb.ToString();
		}

		//As a diagram (F# for a Floor number, E for Elevator, H for Hydrogen, L for Lithium, 
		//M for Microchip, and G for Generator), the initial state looks like this:
		//F4	.	.	.	.	.
		//F3	.	.	.	LG	.
		//F2	.	HG	.	.	.
		//F1	E	.	HM	.	LM
		public static State FillTestState()
		{
			sbyte floorCount = 4;
			sbyte itemCount = 4;
			var state = new State(floorCount, itemCount);
			state.AddItem(ItemTypes.Elevator, F1, null);
			state.AddItem(ItemTypes.Generator, F2, 'H');
			state.AddItem(ItemTypes.Microchip, F1, 'H');
			state.AddItem(ItemTypes.Generator, F3, 'L');
			state.AddItem(ItemTypes.Microchip, F1, 'L');
			return state;
		}

		//The first floor contains a promethium(M) generator and a promethium-compatible microchip.
		//The second floor contains a cobalt(C) generator, a curium(U) generator, a ruthenium(R) generator, and a plutonium(P) generator.
		//The third floor contains a cobalt-compatible microchip, a curium-compatible microchip, a ruthenium-compatible microchip, and a plutonium-compatible microchip.
		//The fourth floor contains nothing relevant.
		public static State FillRealStateOne()
		{
			sbyte floorCount = 4;
			sbyte itemCount = 10;
			var state = new State(floorCount, itemCount);
			state.AddItem(ItemTypes.Elevator, F1, null);
			state.AddItem(ItemTypes.Generator, F1, 'M');
			state.AddItem(ItemTypes.Microchip, F1, 'M');
			state.AddItem(ItemTypes.Generator, F2, 'C');
			state.AddItem(ItemTypes.Generator, F2, 'U');
			state.AddItem(ItemTypes.Generator, F2, 'R');
			state.AddItem(ItemTypes.Generator, F2, 'P');
			state.AddItem(ItemTypes.Microchip, F3, 'C');
			state.AddItem(ItemTypes.Microchip, F3, 'U');
			state.AddItem(ItemTypes.Microchip, F3, 'R');
			state.AddItem(ItemTypes.Microchip, F3, 'P');


			return state;
		}

		//On first floor added:
		//An elerium (E) generator.
		//An elerium-compatible microchip.
		//A dilithium (D)generator.
		//A dilithium-compatible microchip.
		public static State FillRealStateTwo()
		{
			sbyte floorCount = 4;
			sbyte itemCount = 14;
			var state = new State(floorCount, itemCount);
			state.AddItem(ItemTypes.Elevator, F1, null);
			state.AddItem(ItemTypes.Generator, F1, 'M');
			state.AddItem(ItemTypes.Microchip, F1, 'M');
			
			state.AddItem(ItemTypes.Generator, F1, 'E');
			state.AddItem(ItemTypes.Microchip, F1, 'E');
			state.AddItem(ItemTypes.Generator, F1, 'D');
			state.AddItem(ItemTypes.Microchip, F1, 'D');

			state.AddItem(ItemTypes.Generator, F2, 'C');
			state.AddItem(ItemTypes.Generator, F2, 'U');
			state.AddItem(ItemTypes.Generator, F2, 'R');
			state.AddItem(ItemTypes.Generator, F2, 'P');
			state.AddItem(ItemTypes.Microchip, F3, 'C');
			state.AddItem(ItemTypes.Microchip, F3, 'U');
			state.AddItem(ItemTypes.Microchip, F3, 'R');
			state.AddItem(ItemTypes.Microchip, F3, 'P');


			return state;
		}
	}
}
