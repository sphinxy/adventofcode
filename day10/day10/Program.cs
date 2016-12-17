using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace day10
{
	class Program
	{
		private static List<int>[] outputs = new List<int>[3] {new List<int>(), new List<int>(), new List<int>()};
		static void Main(string[] args)
		{
			try
			{
				//Solver("testData.txt", 2, 5);
				Solver("inputData.txt", 17, 61);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception catched: " + ex);
			}
			Console.WriteLine("Done.");
			Console.ReadLine();
		}

		private static void Solver(string inputFile, int answerLow, int answerHigh)
		{
			Bot answerBot = null;
			var botList = new Dictionary<int, Bot>();
			var commandList = new List<Command>();
			foreach (string line in File.ReadLines(inputFile))
			{
				var command = CommandFactory.Create(line);
				commandList.Add(command);
			}
			commandList.Reverse();
			var commandStack = new Stack<Command>(commandList);
			

			while (commandStack.Any())
			{
				var currentCommand = commandStack.Pop();
				if (!botList.ContainsKey(currentCommand.BotId))
				{
					botList[currentCommand.BotId] =  new Bot(currentCommand.BotId);
				}
				var currentBot = botList[currentCommand.BotId];
				
				currentCommand.Execute(currentBot);
				//Console.WriteLine(currentCommand);
				Console.WriteLine(currentBot);
				
				if (currentBot.Values.Count == 2 && currentBot.QueuedCommands.Any())
				{
					answerBot = CheckAnswer(currentBot, answerLow, answerHigh) ?? answerBot;
					var giveCommand = currentBot.QueuedCommands.Pop();
					var data = giveCommand.LowData;
					var value = currentBot.Values.Min();
					ProcessGiveToBotOrOutput(data, value, commandStack, currentBot.BotId);
					data = giveCommand.HighData;
					value = currentBot.Values.Max();
					ProcessGiveToBotOrOutput(data, value, commandStack, currentBot.BotId);
					currentBot.ClearAfterGive();
				}
			}

			Console.WriteLine($"Answer bot is {answerBot.BotId}");
			var result = 1;
			for (int i = 0; i < 3; i++)
			{
				
				foreach (var value in outputs[i])
				{
					result = result * value;
				}
			}
			Console.WriteLine($"For outputs multivalue is {result}");
		}

		private static Bot CheckAnswer(Bot currentBot, int answerLow, int answerHigh)
		{
			if (currentBot.Values.Min() == answerLow && currentBot.Values.Max() == answerHigh)
			{
				return currentBot;
			}
			return null;
		}

		private static void ProcessGiveToBotOrOutput(GiveCommandData data, int value, Stack<Command> commandStack, int botId)
		{
			if (data.GiveCommandOutputType == GiveCommandOutputTypes.Bot)
			{
				var newBotCommand = new SetCommand(data.Id, value);
				commandStack.Push(newBotCommand);
				Console.WriteLine($">>>Bot {botId} gives {value} to bot {data.Id}");
			}
			else
			{
				Console.WriteLine($">>>Bot {botId} gives {value} to output {data.Id}");
				if (data.Id <= 2)
				{
					outputs[data.Id].Add(value);
				}
			}
		}
	}
}
