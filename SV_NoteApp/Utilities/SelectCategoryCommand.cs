using SV_NoteApp.Model;
using SV_NoteApp.ViewModel;
using System;
using System.Windows.Input;

namespace SV_NoteApp.Utilities
{

    
    public class SelectCategoryCommand: ICommand
    {
        private ViewModelBase viewModel;
        private bool isFilter;

        public SelectCategoryCommand(ViewModelBase viewModel, bool isFilter)
        {
            this.viewModel = viewModel;
            this.isFilter = isFilter;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (isFilter)
            {
                if (viewModel.FilterCategory.Id == (parameter as NoteCategory).Id)
                {
                    viewModel.FilterCategory.Id = -1;
                    viewModel.FilterCategory.Name = "-";
                }
                else
                {
                    viewModel.FilterCategory.Id = (parameter as NoteCategory).Id;
                    viewModel.FilterCategory.Name = (parameter as NoteCategory).Name;
                }
                viewModel.FilterNote();
            }else
            {
                viewModel.SelectedCategory.Id = (parameter as NoteCategory).Id;
                viewModel.SelectedCategory.Name = (parameter as NoteCategory).Name;
            }

            foreach (CategoryItem item in viewModel.CategoryList)
            {
                if (item.MyNoteCategory.Id != viewModel.FilterCategory.Id)
                {
                    item.IsActive = false;
                }
                else { item.IsActive = true; }
            }

        }
    }
}
