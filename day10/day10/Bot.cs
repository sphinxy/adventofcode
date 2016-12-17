using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace day10
{
	public class Bot
	{
		public int BotId;
		public List<int> Values = new List<int>();
		public Stack<GiveCommand> QueuedCommands = new Stack<GiveCommand>();

		public void QueueCommand(GiveCommand command)
		{
			QueuedCommands.Push(command);
		}

		public Bot(int botId)
		{
			BotId = botId;
		}


		public void ClearAfterGive()
		{
			Values.Clear();
		}

		public void AddValue(int value)
		{	
			Values.Add(value);
		}

		public override string ToString()
		{
			return $"Bot {BotId} with values {string.Join(",",Values.ToArray())} and have #{QueuedCommands.Count} qcommands";
		}
	}
}