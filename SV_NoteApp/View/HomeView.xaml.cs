using SV_NoteApp.ViewModel;
using System.Windows.Controls;

namespace SV_NoteApp.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        private ViewModelBase viewmodel;
        public HomeView(ViewModelBase theViewModel)
        {
            InitializeComponent();
            viewmodel = theViewModel;
        }
    }
}
