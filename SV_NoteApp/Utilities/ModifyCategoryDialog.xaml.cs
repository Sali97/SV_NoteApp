using SV_NoteApp.Model;
using System.Windows;

namespace SV_NoteApp.Utilities
{
    /// <summary>
    /// Interaction logic for ModifyCategoryDialog.xaml
    /// </summary>
    public partial class ModifyCategoryDialog : Window
    {
        public ModifyCategoryDialog(NoteCategory theNoteCategory)
        {
            InitializeComponent();
            ResponseText = theNoteCategory.Name;
        }

        public string ResponseText
        {
            get { return ResponseTextBox.Text; }
            set { ResponseTextBox.Text = value; }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResponseText.Length<=0||ResponseText.Trim(' ').Length<=0)
            {
                MessageBox.Show("Nem lehet üres a kategória neve!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DialogResult = true;
            }
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
