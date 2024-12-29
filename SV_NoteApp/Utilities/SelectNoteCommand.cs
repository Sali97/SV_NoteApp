using SV_NoteApp.Model;
using SV_NoteApp.ViewModel;
using System;
using System.Windows.Input;

namespace SV_NoteApp.Utilities
{
    public class SelectNoteCommand: ICommand
    {
        private ViewModelBase viewModel;


        public SelectNoteCommand(ViewModelBase viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.SelectedNote = parameter;
        }
    }
}
