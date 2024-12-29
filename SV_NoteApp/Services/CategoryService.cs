using SV_NoteApp.Model;
using SV_NoteApp.Utilities;
using SV_NoteApp.ViewModel;
using System.Collections.Generic;

namespace SV_NoteApp.Services
{
    public class CategoryService
    {
        List<NoteCategory> theCategoryList = new List<NoteCategory>(); //Kategória elemek listája
        ViewModelBase viewmodel = null;

        public CategoryService(ViewModelBase theViewModel)
        {
            viewmodel = theViewModel;
        }

        public void test()
        {
            theCategoryList.Add(new NoteCategory { Id = 8, Name = "Angol jegyzetek" });
            theCategoryList.Add(new NoteCategory { Id = 1, Name = "2024-12-05" });
            theCategoryList.Add(new NoteCategory { Id = 2, Name = "Mikulás viccek" });
            theCategoryList.Add(new NoteCategory { Id = 3, Name = "Családi" });
            theCategoryList.Add(new NoteCategory { Id = 4, Name = "---Titkos---" });
            theCategoryList.Add(new NoteCategory { Id = 5, Name = "Bevásárlólista" });
            theCategoryList.Add(new NoteCategory { Id = 6, Name = "Teendők" });
            theCategoryList.Add(new NoteCategory { Id = 7, Name = "Egyéb" });
        }

        public void Add(NoteCategory newCategory)
        {
            newCategory.Id = getIndexForNewNote();
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
            foreach (NoteCategory item in theCategoryList)
            {
                if (item.Id > max)
                {
                    max = item.Id;
                }
            }
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
            theCategoryList.Remove(FindItem(deleteIndex));

        }

        public void Update(NoteCategory UpdateCategory)
        {
            NoteCategory oldCategory = FindItem(UpdateCategory.Id);

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

            foreach (NoteCategory item in theCategoryList)
            {
                theCategoryItemsList.Add(getNoteItem(item));
            }

            return theCategoryItemsList;
        }

        public List<NoteCategory> getAll()
        {
            return theCategoryList;
        }
    }
}
