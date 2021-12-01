using System;

namespace day10
{
	public class SetCommand: Command
	{
		public int Value;

		public SetCommand(int botId, int value)
		{
			BotId = botId;
			Value = value;
			
		}

		public override void Execute(object parameter)
		{
			base.Execute(parameter);
			commandBot.Values.Add(Value);
		}

		public override string ToString()
		{
			return $"Command {nameof(SetCommand)} with BotId={BotId} and value={Value}";
		}

		public event EventHandler CanExecuteChanged;
	}
}