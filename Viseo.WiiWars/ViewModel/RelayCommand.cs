using System;
using System.Windows.Input;

namespace Viseo.WiiWars
{
	class RelayCommand : ICommand
	{
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
		private readonly Action _executeMethod;
		private readonly Func<bool> _canExecuteMethod;
		public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
		{
			_executeMethod = executeMethod;
			_canExecuteMethod = canExecuteMethod;
		}
		public RelayCommand(Action executeMethod)
			: this(executeMethod, null)
		{
		}
		public bool CanExecute(object parameter)
		{
			return _canExecuteMethod == null || _canExecuteMethod.Invoke();
		}

		public void Execute(object parameter)
		{
			_executeMethod.Invoke();
		}
	}
}
