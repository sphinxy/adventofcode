using System;
using System.Windows.Input;

namespace day10
{
	public static class CommandFactory
	{
		
		public static Command Create(string commandLine)
		{
			Command cmd = null;
			var commandParts = commandLine.Split(' ');
			switch (commandParts[0])
			{
				case "value":
					var botId = int.Parse(commandParts[5]);
					var value = int.Parse(commandParts[1]);
					cmd = new SetCommand(botId, value);
					break;
				case "bot":
					botId = int.Parse(commandParts[1]);
					var lowGiveData = new GiveCommandData(int.Parse(commandParts[6]), commandParts[5]);
					var highGiveData = new GiveCommandData(int.Parse(commandParts[11]),commandParts[10]);
					cmd = new GiveCommand(botId, lowGiveData, highGiveData);
					break;
				default:
					throw new ArgumentOutOfRangeException($"Unknown command in line {commandLine}");
			}
			return cmd;
		}
	}
}