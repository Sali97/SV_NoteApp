using System.Windows.Controls;
using SV_NoteApp.Model;
using System.ComponentModel;
using System.Windows.Input;
using SV_NoteApp.ViewModel;

namespace SV_NoteApp.Utilities
{
    /// <summary>
    /// Interaction logic for NoteItem.xaml
    /// </summary>
    public partial class NoteItem : UserControl, INotifyPropertyChanged
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

        private ViewModelBase viewmodel;

        private Note myNote;
        public Note MyNote
        {
            get { return myNote; }
            set { myNote = value; OnPropertyChanged("MyNote"); }
        }


        public NoteItem(Note theNote, ViewModelBase theViewModel)
        {
            InitializeComponent();

            viewmodel = theViewModel;
            MyNote = new Note();
            MyNote = theNote;

            this.DataContext = this;

            EditNoteCommand = new RelayCommand(EditNote);
            DeleteNoteCommand = new RelayCommand(DeleteNote);
            SelectCommand = new RelayCommand(SelectNote);
        }

        public ICommand SelectNoteCommand { get; set; }
        public ICommand UpdateViewCommand { get; set; }
        public RelayCommand EditNoteCommand { get; set; }
        public RelayCommand DeleteNoteCommand { get; set; }
        public RelayCommand SelectCommand { get; set; }


        private void SelectNote()
        {
            SelectNoteCommand = new SelectNoteCommand(viewmodel);
            SelectNoteCommand.Execute(MyNote);

            UpdateViewCommand = new UpdateViewCommand(viewmodel);
            UpdateViewCommand.Execute("SelectedView");
        }

        private void EditNote()
        {
            SelectNoteCommand = new SelectNoteCommand(viewmodel);
            SelectNoteCommand.Execute(MyNote);

            UpdateViewCommand = new UpdateViewCommand(viewmodel);
            UpdateViewCommand.Execute("NoteModifyViewE");
        }

        private void DeleteNote()
        {
            SelectNoteCommand = new SelectNoteCommand(viewmodel);
            SelectNoteCommand.Execute(MyNote);

            viewmodel.DeleteNoteCommand.Execute(MyNote);
        }


    }
}
