using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace AutoCompleteBox
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Item> Roles { get; set; }
        public ObservableCollection<Item> Classes { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            myCombo.EnterPressed += () =>
            {
                myCombo2.FocusSearchBox();
            };

            myCombo2.EnterPressed += () =>
            {
                btnShow.Focus();
            };

            Roles = new ObservableCollection<Item>
            {
                new Item { Name = "Admin" },
                new Item { Name = "Editor" },
                new Item { Name = "Viewer" },
                new Item { Name = "Moderator" },
                new Item { Name = "F" },
                new Item { Name = "G" },
                new Item { Name = "H" },
            };

            Classes = new ObservableCollection<Item>
            {
                new Item { Name = "Class A" },
                new Item { Name = "Class B" }
            };

            DataContext = this;
        }
        public void MoveFocusToNext(UIElement current)
        {
            current.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var roles = myCombo.GetSelectedItems()
                .Select(x => x.Name)
                .ToList();

            var classes = myCombo2.GetSelectedItems()
                .Select(x => x.Name)
                .ToList();

            string json = JsonSerializer.Serialize(new { roles, classes });
            MessageBox.Show(json);

            myCombo.FocusSearchBox();
        }
    }
}