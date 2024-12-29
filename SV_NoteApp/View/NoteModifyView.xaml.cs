using SV_NoteApp.Model;
using SV_NoteApp.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SV_NoteApp.View
{
    /// <summary>
    /// Interaction logic for NoteModifyView.xaml
    /// </summary>
    public partial class NoteModifyView : UserControl
    {

        #region INotifyPropertyChanged_Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion

        private List<NoteCategory> myList;

        public List<NoteCategory> MyList
        {
            get { return myList; }
            set { myList = value; OnPropertyChanged("MyList"); }
        }

        private ViewModelBase viewmodel;
        private Note SelectedNote;
        private NoteCategory SelectedCategory;
      

        public NoteModifyView(bool isNewNote, ViewModelBase theViewModel)
        {
            InitializeComponent();

            viewmodel = theViewModel;
            MyList = viewmodel.NoteCategoryList;
            SelectedNote = (theViewModel.SelectedNote as Note);
            SelectedCategory = new NoteCategory { Id=0, Name=""};
        }

        private void getCategoryOfSelectedNote()
        {
            int SelCatIndex = -10;

            if (myComboBox.Items.Count>0)
            {
                foreach (var item in myComboBox.Items)
                {
                    if ((item as NoteCategory).Id == SelectedNote.CategoryId)
                    {
                        SelectedCategory = (item as NoteCategory);
                        SelCatIndex = myComboBox.Items.IndexOf(item);
                        myComboBox.SelectedIndex = SelCatIndex;
                    }
                }
            }
        
        }


        private void myComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            getCategoryOfSelectedNote();
        }

        private void myComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                viewmodel.ChangeCategoryId((myComboBox.Items[myComboBox.SelectedIndex] as NoteCategory).Id);
            }
            catch (Exception)
            {

            }
           
        }
    }
}
