using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AutoCompleteBox
{
    public class AutoCompleteTextBox : TextBox
    {
        private Popup popup;
        private ListBox listBox;

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(AutoCompleteTextBox));

        public AutoCompleteTextBox()
        {
            popup = new Popup();
            listBox = new ListBox();

            popup.Child = listBox;
            popup.PlacementTarget = this;
            popup.StaysOpen = false;

            this.TextChanged += AutoCompleteTextBox_TextChanged;

            listBox.MouseDoubleClick += (s, e) =>
            {
                if (listBox.SelectedItem != null)
                {
                    Text = listBox.SelectedItem.ToString();
                    popup.IsOpen = false;
                }
            };
        }

        private void AutoCompleteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ItemsSource == null) return;

            listBox.Items.Clear();

            foreach (var item in ItemsSource)
            {
                if (item.ToString().ToLower().Contains(Text.ToLower()))
                {
                    listBox.Items.Add(item);
                }
            }

            popup.IsOpen = listBox.Items.Count > 0;
        }
    }
}