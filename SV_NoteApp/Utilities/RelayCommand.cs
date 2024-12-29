using System;
using System.Windows.Input;

namespace SV_NoteApp.Utilities
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action doWork;

        public RelayCommand(Action work)
        {
            doWork = work;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            doWork();
        }

    }
}
