using System.Windows.Controls;
using SV_NoteApp.Model;
using System.ComponentModel;
using SV_NoteApp.ViewModel;
using System.Windows.Input;
using System.Windows;

namespace SV_NoteApp.Utilities
{
    /// <summary>
    /// Interaction logic for CategoryItem.xaml
    /// </summary>
    public partial class CategoryItem : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged_Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private NoteCategory myNoteCategory;
        public NoteCategory MyNoteCategory
        {
            get { return myNoteCategory; }
            set { myNoteCategory = value; OnPropertyChanged("MyNoteCategory"); }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; OnPropertyChanged("IsActive"); }
        }

        private ViewModelBase viewmodel;

        public CategoryItem(NoteCategory theCategory, ViewModelBase theViewModel)
        {
            InitializeComponent();
            SelectCatCommand = new RelayCommand(SelectCategory);
            UpdateCatCommand = new RelayCommand(UpdateCategory);
            DeleteCatCommand = new RelayCommand(DeleteCategory);

            IsActive = false;

            viewmodel = theViewModel;
            MyNoteCategory = new NoteCategory();
            MyNoteCategory = theCategory;

            this.DataContext = this;
        }

        public ICommand SelectCategoryCommand { get; set; }
        public RelayCommand SelectCatCommand { get; set; }
        public RelayCommand UpdateCatCommand { get; set; }
        public RelayCommand DeleteCatCommand { get; set; }

        private void SelectCategory()
        {
            SelectCategoryCommand = new SelectCategoryCommand(viewmodel, true);
            SelectCategoryCommand.Execute(myNoteCategory);
        }

        private void DeleteCategory()
        {
            viewmodel.DeleteCategory(myNoteCategory.Id);
        }

        private void UpdateCategory()
        {
            viewmodel.UpdateCategory(MyNoteCategory);
        }
    }
}