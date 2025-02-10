using SV_NoteApp.Model;
using SV_NoteApp.Utilities;
using SV_NoteApp.ViewModel;
using System;
using System.Collections.Generic;

namespace SV_NoteApp.Services
{

    public class NoteService
    {
        List<Note> theNoteList = new List<Note>(); //Elemek listája
        public List<NoteItem> NoteItemList = new List<NoteItem>(); //Grafikusan megjelenített elemek listája
        public List<CategoryItem> CategoryItemList = new List<CategoryItem>(); //Grafikusan megjelenített kategória elemek listája
        ViewModelBase viewmodel = null;
        private CategoryService theCategoryService;
        private SQLService theNoteSQLService = new SQLService();

        int filterId = -1;
        public NoteService(ViewModelBase theViewModel)
        {
            viewmodel = theViewModel;
            theCategoryService = new CategoryService(viewmodel);
            theCategoryService.getCategoriesFromSQL();
        }
            public void getNotesFromSQL()
        {
            theNoteList = theNoteSQLService.Query("SELECT Id,Title,Text,CatId FROM Notes") as List<Note>;
            refreshNoteItemList(filterId);
        }

        #region CategoryControl
        public void AddCategory(NoteCategory newCategory)
        {
            theCategoryService.Add(newCategory);
            refreshCategoriesList();
        }
        public void DeleteCategory(int categoryIndex)
        {
            theCategoryService.Delete(categoryIndex);

            foreach (Note item in theNoteList)
            {
                if (item.CategoryId==categoryIndex) //A törölt kategóriában lévő elemek kategróia azonosítójának törlése
                {
                    item.CategoryId = -10;
                }
            }

            refreshCategoriesList();
        }

        public void UpdateCategory(NoteCategory updateCategory)
        {
            theCategoryService.Update(updateCategory);

            refreshCategoriesList();
        }
        #endregion

        public void Add(Note newNote)
        {
            newNote.Id = getIndexForNewNote();

            SQLAdd(newNote);
            theNoteList.Add(newNote);
            
            refreshNoteItemList(filterId);
        }

        public void FilterNotes(int newFilter)
        {
            filterId = newFilter;
            refreshNoteItemList(filterId);
        }

        private Note FindItem(int searchIndex)
        {
            Note result = new Note();
            result = theNoteList.Find(x => x.Id == searchIndex);
            return result;
        }

        private int getIndexForNewNote()
        {
            int maxindex = MaxOfId();
            return maxindex + 1;
        }

        private int MaxOfId()
        {
            int max = 0;
            foreach (Note item in theNoteList)
            {
                if (item.Id>max)
                {
                    max = item.Id;
                }
            }
            return max;
        }

        public bool HasThis(int searchIndex)
        {
            bool hasItem = false;

            Note result = new Note();
            result = FindItem(searchIndex);
            if (result != null)
            {
                hasItem = true;
            }
            return hasItem; 
        }

        public void Delete(Note deleteNote)
        {
            SQLDelete(deleteNote.Id);
            theNoteList.Remove(FindItem(deleteNote.Id));
            
            refreshNoteItemList(filterId);
        }

        public void Update(Note UpdateNote)
        {
            Note oldNote = FindItem(UpdateNote.Id);

            SQLUpdate(UpdateNote);

            oldNote.Title = UpdateNote.Title;
            oldNote.Text = UpdateNote.Text;
            oldNote.CategoryId = UpdateNote.CategoryId;

            refreshNoteItemList(filterId);
        }

        private NoteItem getNoteItem(Note item)
        {
            NoteItem newNoteItem = new NoteItem(item, viewmodel);
            return newNoteItem;
        }


        public List<NoteItem> getNoteItems(int filterId)
        {
            NoteItemList.Clear();
            foreach (Note item in theNoteList)
            {
                if (filterId>0)
                {
                    if (item.CategoryId==filterId)
                    {
                        NoteItemList.Add(getNoteItem(item));
                    }
                }
                else
                {
                    NoteItemList.Add(getNoteItem(item));
                }   
            }

            return NoteItemList;  
        }

        public void refreshNoteItemList(int filterId)
        {
            refreshCategoriesList();
            NoteItemList = getNoteItems(filterId);
        }

        public void refreshCategoriesList()
        {
            CategoryItemList.Clear();
            CategoryItemList = theCategoryService.getCategoryItems();
        }

        public List<CategoryItem> getCategoryItems()
        {
            return CategoryItemList;
        }

        public List<NoteCategory> getCategories()
        {
            return theCategoryService.getAll();
        }


        private void SQLAdd(Note NewNote)
        {
            String commandtxt =String.Format ("INSERT INTO Notes VALUES({0},'{1}','{2}','{3}')", NewNote.Id.ToString(), NewNote.Title, NewNote.Text, NewNote.CategoryId.ToString());
            theNoteSQLService.Execute(commandtxt);
        }

        private void SQLUpdate(Note UpdatedNote)
        {
            String commandtxt = String.Format("UPDATE Notes SET Title='{0}', Text='{1}', CatId={2} WHERE Id={3}",UpdatedNote.Title,UpdatedNote.Text, UpdatedNote.CategoryId.ToString(), UpdatedNote.Id.ToString());
            theNoteSQLService.Execute(commandtxt);
        }

        private void SQLDelete(int NoteId)
        {
            String commandtxt = String.Format("DELETE FROM Notes WHERE Id={0}",NoteId);
            theNoteSQLService.Execute(commandtxt);
        }
    }
}
