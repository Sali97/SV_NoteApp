using System;
using System.Windows.Input;
using SV_NoteApp.ViewModel;
using SV_NoteApp.View;
using SV_NoteApp.Model;

namespace SV_NoteApp.Utilities
{
    public class UpdateViewCommand : ICommand
    {
        public ViewModelBase viewModel;

        public UpdateViewCommand(ViewModelBase viewModel)
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
            if (parameter.ToString() == "HomeView")
            {
                viewModel.SelectedView = new HomeView();
            }
            if (parameter.ToString() == "SelectedView")
            {
                viewModel.SelectedView = new SelectedView();
            }
            if (parameter.ToString() == "NoteModifyViewN")
            {
                viewModel.SelectedView = new NoteModifyView(true, viewModel);
                viewModel.SelectedNote = new Note();
            }
            if (parameter.ToString() == "NoteModifyViewE")
            {
                viewModel.SelectedView = new NoteModifyView(false, viewModel);
            }
        }
    }
}
