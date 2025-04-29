using SV_NoteApp.Model;
using SV_NoteApp.Utilities;
using SV_NoteApp.ViewModel;
using System;
using System.Collections.Generic;

namespace SV_NoteApp.Services
{
    public class CategoryService
    {
        List<NoteCategory> theCategoryList; //Kategória elemek listája
        ViewModelBase viewmodel = null;
        private SQLService theCategorySQLService = new SQLService();
        
        public CategoryService(ViewModelBase theViewModel)
        {
            viewmodel = theViewModel;
        }

        public void getCategoriesFromSQL()
        {
            theCategoryList = theCategorySQLService.Query("SELECT Id,Name FROM Categories") as List<NoteCategory>;
        }

        public void Add(NoteCategory newCategory)
        {
            newCategory.Id = getIndexForNewNote();

            SQLAdd(newCategory);
            theCategoryList.Add(newCategory);
        }
        private NoteCategory FindItem(int searchIndex)
        {
            NoteCategory result = new NoteCategory();
            result = theCategoryList.Find(x => x.Id == searchIndex);
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
            if (theCategoryList !=null)
            {
                  foreach (NoteCategory item in theCategoryList)
            {
                if (item.Id > max)
                {
                    max = item.Id;
                }
            }
            }
            else { theCategoryList = new List<NoteCategory>(); }

                return max;
        }

        public bool HasThis(int searchIndex)
        {
            bool hasItem = false;

            NoteCategory result = new NoteCategory();
            result = FindItem(searchIndex);
            if (result != null)
            {
                hasItem = true;
            }
            return hasItem;
        }

        public void Delete(int deleteIndex)
        {
            SQLDelete(deleteIndex);
            theCategoryList.Remove(FindItem(deleteIndex));
        }

        public void Update(NoteCategory UpdateCategory)
        {
            NoteCategory oldCategory = FindItem(UpdateCategory.Id);

            SQLUpdate(UpdateCategory);
            oldCategory.Name = UpdateCategory.Name;

        }

        private CategoryItem getNoteItem(NoteCategory item)
        {
            CategoryItem newCategoryItem = new CategoryItem(item, viewmodel);
            return newCategoryItem;
        }

        public List<CategoryItem> getCategoryItems()
        {
            List<CategoryItem> theCategoryItemsList = new List<CategoryItem>();

            if (theCategoryList != null)
            {
                foreach (NoteCategory item in theCategoryList)
                {
                    theCategoryItemsList.Add(getNoteItem(item));
                }
            }
            else { }

                return theCategoryItemsList;
        }

        public List<NoteCategory> getAll()
        {
            return theCategoryList;
        }

        private void SQLAdd(NoteCategory NewNoteCategory)
        {
            String commandtxt = String.Format("INSERT INTO Categories VALUES({0},'{1}')", NewNoteCategory.Id.ToString(), NewNoteCategory.Name);
            theCategorySQLService.Execute(commandtxt);
        }

        private void SQLUpdate(NoteCategory UpdatedNoteCategory)
        {
            String commandtxt = String.Format("UPDATE Categories SET Name='{0}' WHERE Id={1}", UpdatedNoteCategory.Name, UpdatedNoteCategory.Id);
            theCategorySQLService.Execute(commandtxt);
        }

        private void SQLDelete(int CategoryId)
        {
            String commandtxt = String.Format("DELETE FROM Categories WHERE Id={0}", CategoryId);
            theCategorySQLService.Execute(commandtxt);
        }
    }
}
