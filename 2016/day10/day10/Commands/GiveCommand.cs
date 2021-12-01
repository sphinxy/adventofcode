using System;

namespace day10
{
	public class GiveCommand : Command
	{
		public GiveCommandData LowData;
		public GiveCommandData HighData;
		public GiveCommand(int botId, GiveCommandData lowData, GiveCommandData highData)
		{
			BotId = botId;
			LowData = lowData;
			HighData = highData;

		}

		public override void Execute(object parameter)
		{
			base.Execute(parameter);
			commandBot.QueueCommand(this);
		}

		public override string ToString()
		{
			return $"Command {nameof(GiveCommand)} with BotId {BotId} and {LowData}/{HighData}";
		}

		public event EventHandler CanExecuteChanged;
	}
}