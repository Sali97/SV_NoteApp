using System.ComponentModel;
using System.Runtime.CompilerServices;
using SV_NoteApp.View;
using SV_NoteApp.Utilities;
using System.Windows.Input;
using System.Collections.Generic;
using SV_NoteApp.Model;
using SV_NoteApp.Services;
using System.Windows;
using System.Xml.Linq;

namespace SV_NoteApp.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged_Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion

        public List<NoteItem> NoteList { get; set; }

        private List<CategoryItem> categoryList;

        public List<CategoryItem> CategoryList
        {
            get { return categoryList; }
            set { categoryList = value; getNoteCategoryList(); }
        }

        private int SelectedCategoryId =0;


        private List<NoteCategory> noteCategoryList;
        public List<NoteCategory> NoteCategoryList
        {
            get { return noteCategoryList; }
            set { noteCategoryList = value; OnPropertyChanged("NoteCategoryList"); }
        }

        private void getNoteCategoryList()
        {
            NoteCategoryList.Clear();
            foreach (CategoryItem item in CategoryList)
            {
                NoteCategoryList.Add(item.MyNoteCategory);
            }
        }
        
        private NoteCategory filterCategory;
        public NoteCategory FilterCategory
        {
            get { return filterCategory; }
            set { filterCategory = value; OnPropertyChanged("FilterCategory"); }
        }


        private NoteCategory selectedCategory;
        public NoteCategory SelectedCategory
        {
            get { return selectedCategory; }
            set { selectedCategory = value; OnPropertyChanged("SelectedCategory"); }
        }

        private NoteService theNoteService;

        private object selectedNote;
        public object SelectedNote
        {
            get { return selectedNote; }
            set { selectedNote = value; OnPropertyChanged("SelectedNote"); }
        }

        private object selectedView;
        public object SelectedView
        {
            get { return selectedView; }
            set { selectedView = value; OnPropertyChanged("SelectedView"); }
        }


        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ViewModelBase()
        {
            theNoteService = new NoteService(this);
            SelectedView = new HomeView();
            UpdateViewCommand = new UpdateViewCommand(this);
            SelectedNote = new Note();
            FilterCategory = new NoteCategory{ Id = -1, Name = "-"};
            SelectedCategory = new NoteCategory();

            

            #region NoteCommands
            addNoteCommand = new RelayCommand(AddNote);
            updateNoteCommand = new RelayCommand(UpdateNote);
            deleteNoteCommand = new RelayCommand(DeleteNote);
            filterNoteCommand = new RelayCommand(FilterNote);
            #endregion
            #region CategoryCommands
            addCategoryCommand = new RelayCommand(AddCategory);
            #endregion

            NoteCategoryList = new List<NoteCategory> { };

            NoteList = new List<NoteItem>
            {  };

            theNoteService.test();
            NoteList = theNoteService.NoteItemList;

            CategoryList = new List<CategoryItem>
            { };

            CategoryList = theNoteService.CategoryItemList;
        }
        public void ChangeCategoryId(int catId)
        {
            SelectedCategoryId = catId;
  
        }


        public ICommand UpdateViewCommand { get; set; }

        #region AddNoteOperation
        private RelayCommand addNoteCommand;
        public RelayCommand AddNoteCommand
        {
            get { return addNoteCommand; }
        }
        public void AddNote()
        {
            Note theNote = new Note();
            theNote.Id = (SelectedNote as Note).Id;
            theNote.Title = (SelectedNote as Note).Title;
            theNote.Text = (SelectedNote as Note).Text;
            theNote.CategoryId = SelectedCategoryId;

            if (theNoteService.HasThis(theNote.Id))
            {
                UpdateNote();
            }else
            {
                theNoteService.Add(theNote);
            }

            UpdateViewCommand.Execute("HomeView");
            RefreshView();
        }
        #endregion
        #region UpateNoteOperation
        private RelayCommand updateNoteCommand;
        public RelayCommand UdateNoteCommand
        {
            get { return updateNoteCommand; }
        }
        public void UpdateNote()
        {
            Note theNote = new Note();
            theNote.Id = (SelectedNote as Note).Id;
            theNote.Title = (SelectedNote as Note).Title;
            theNote.Text = (SelectedNote as Note).Text;
            theNote.CategoryId = SelectedCategoryId;

            theNoteService.Update(theNote);
            RefreshView();
        }
        #endregion
        #region DeleteNoteOperation
        private RelayCommand deleteNoteCommand;
        public RelayCommand DeleteNoteCommand
        {
            get { return deleteNoteCommand; }
        }
        public void DeleteNote()
        {
            Note theNote = new Note();
            theNote.Id = (SelectedNote as Note).Id;
            theNote.Title = (SelectedNote as Note).Title;
            theNote.Text = (SelectedNote as Note).Text;

            theNoteService.Delete(theNote);
            RefreshView();
        }
        #endregion
        #region FilterNoteOperation
        private RelayCommand filterNoteCommand;
        public RelayCommand FilterNoteCommand
        {
            get { return filterNoteCommand; }
        }
        public void FilterNote()
        {
            theNoteService.FilterNotes(FilterCategory.Id);
            RefreshView();
        }
        #endregion

        #region AddCategoryOperation
        private RelayCommand addCategoryCommand;
        public RelayCommand AddCategoryCommand
        {
            get { return addCategoryCommand; }
        }
        public void AddCategory()
        {
            var dialog = new ModifyCategoryDialog(new NoteCategory());
            if (dialog.ShowDialog()==true)
            {
                theNoteService.AddCategory(new NoteCategory { Name = dialog.ResponseText });
            }
            RefreshView();
        }
        #endregion
        #region UpateCategoryOperation
        private RelayCommand updateCategoryCommand;
        public RelayCommand UpdateCategoryCommand
        {
            get { return updateCategoryCommand; }
        }
        public void UpdateCategory(NoteCategory theNoteCategory)
        {
            var dialog = new ModifyCategoryDialog(theNoteCategory);
            if (dialog.ShowDialog() == true)
            {
                if (theNoteCategory.Name != dialog.ResponseText)
                {
                    theNoteCategory.Name = dialog.ResponseText;
                    theNoteService.UpdateCategory(theNoteCategory);
                    if (FilterCategory.Id==theNoteCategory.Id && FilterCategory.Name!=theNoteCategory.Name)
                    {
                        FilterCategory.Name = theNoteCategory.Name;
                    }
                }
            }
            RefreshView();
        }
        #endregion
        #region DeleteCategoryOperation
        public void DeleteCategory(int categoryIndex)
        {
            theNoteService.DeleteCategory(categoryIndex);
            if (FilterCategory.Id == categoryIndex)
            {
                FilterCategory.Id = -1;
                FilterCategory.Name = "-";
            }
            FilterNote();
        }
        #endregion
        public void RefreshView()
        {
            CategoryList = theNoteService.CategoryItemList;
            NoteList = theNoteService.NoteItemList;

            UpdateViewCommand.Execute("HomeView");
        }
    }
}
