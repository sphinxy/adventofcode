using System;
using System.Windows.Input;

namespace day10
{
	public abstract class Command : ICommand
	{
		public int BotId;
		protected Bot commandBot = null;
		public bool CanExecute(object parameter)
		{
			return false;
		}

		public virtual void Execute(object parameter)
		{
			commandBot = (Bot) parameter;
		}

		public event EventHandler CanExecuteChanged;
	}
}