using SV_NoteApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
