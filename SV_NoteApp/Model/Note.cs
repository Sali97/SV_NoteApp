using System;
using System.ComponentModel;

namespace SV_NoteApp.Model
{
    public class Note
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

        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged("Title"); }
        }

        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged("Text"); }
        }

        private int categoryId;
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; OnPropertyChanged("CategoryId"); }
        }

        private String createDate;

        public String CreateDate
        {
            get { return createDate; }
            set { createDate = value; OnPropertyChanged("CreateDate"); }
        }

        private String modifyDate;

        public String ModifyDate
        {
            get { return modifyDate; }
            set { modifyDate = value; OnPropertyChanged("ModifyDate"); }
        }

        private bool isPrio;

        public bool IsPrio
        {
            get { return isPrio; }
            set { isPrio = value; OnPropertyChanged("IsPrio"); }
        }


    }
}
