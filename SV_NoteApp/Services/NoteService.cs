using SV_NoteApp.Model;
using SV_NoteApp.Utilities;
using SV_NoteApp.ViewModel;
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

        int filterId = -1;
        public NoteService(ViewModelBase theViewModel)
        {
            viewmodel = theViewModel;
            theCategoryService = new CategoryService(viewmodel);
            theCategoryService.test();
        }
            public void test()
        {
            Add((new Note {Title = "Title1", Text = "The place of your text. Here will be your messages ad every informations what you want to collect.", CategoryId=1 }));
            Add((new Note {Title = "Title2", Text = "This is the second note.", CategoryId = 1 }));
            Add((new Note {Title = "Title3", Text = "Binding is working perfectly.", CategoryId = 3 }));
            Add((new Note {Title = "Title4", Text = "Jó fasza.", CategoryId = 2 }));
            Add((new Note {Title = "Title5", Text = "Működik végre ez a szartalicska.", CategoryId = 1 }));
            Add((new Note {Title = "Title6", Text = "Nice, it is a beautiful.", CategoryId = 4 }));
            Add((new Note { Title = "Title7", Text = "Es ist sehr gut jetzt.", CategoryId = 5 }));
            Add((new Note {Title = "Title8", Text = "Elképzelhető, hogy ennek műküődése nagymértékben megkérdőjelezhető, de az eredmény magáért beszél.", CategoryId = 2 }));
            Add((new Note { Title = "Title9", Text = "Egyszer csak elkészül ez a fostenger.", CategoryId = 8 }));
            Add((new Note { Title = "Title10", Text = "Ami nem biztos az lehet, hogy nincs. Tehát ami nincs az nem biztos, hogy van. A létezése asszem mindenféleképpen megkérdőjelezhető annak, melynek nem vagyunk biztosak az ismeretáéről, hogy tudjuk-e pontosan, hogy mi az ami az-e ami nem az.", CategoryId = 8 }));
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
            theNoteList.Remove(FindItem(deleteNote.Id));

            refreshNoteItemList(filterId);
        }

        public void Update(Note UpdateNote)
        {
            Note oldNote = FindItem(UpdateNote.Id);

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

    }
}
