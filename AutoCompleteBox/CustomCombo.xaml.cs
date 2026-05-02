using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;

namespace AutoCompleteBox
{

    public partial class CustomCombo : UserControl
    {
        public event Action EnterPressed;
        public CustomCombo()
        {
            InitializeComponent();

            SelectedItems = new ObservableCollection<Item>();
            FilteredItems = new ObservableCollection<Item>();

            Loaded += CustomCombo_Loaded;
        }

        private void CustomCombo_Loaded(object sender, RoutedEventArgs e)
        {
            if (Config?.AutoOpen == true)
            {
                ListBorder.Visibility = Visibility.Visible;

                Dispatcher.BeginInvoke(() =>
                {
                    SearchBox.Focus();
                });
            }
        }

        public ObservableCollection<Item> Items
        {
            get => (ObservableCollection<Item>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(
                nameof(Items),
                typeof(ObservableCollection<Item>),
                typeof(CustomCombo),
                new PropertyMetadata(null, OnItemsChanged));

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CustomCombo)d;

            if (e.NewValue is ObservableCollection<Item> newItems)
            {
                control.FilteredItems.Clear();
                foreach (var item in newItems)
                    control.FilteredItems.Add(item);
            }
        }

        //    public bool IsAutoOpenEnabled
        //    {
        //        get => (bool)GetValue(IsAutoOpenEnabledProperty);
        //        set => SetValue(IsAutoOpenEnabledProperty, value);
        //    }

        //    public static readonly DependencyProperty IsAutoOpenEnabledProperty =
        //DependencyProperty.Register(
        //    nameof(IsAutoOpenEnabled),
        //    typeof(bool),
        //    typeof(CustomCombo),
        //    new PropertyMetadata(false, OnAutoOpenChanged));

        //    private static void OnAutoOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //    {
        //        var control = (CustomCombo)d;

        //        if ((bool)e.NewValue)
        //            control.ListBorder.Visibility = Visibility.Visible;
        //        else
        //            control.ListBorder.Visibility = Visibility.Collapsed;
        //    }

        public ComboConfig Config
        {
            get => (ComboConfig)GetValue(ConfigProperty);
            set => SetValue(ConfigProperty, value);
        }

        public static readonly DependencyProperty ConfigProperty =
            DependencyProperty.Register(
                nameof(Config),
                typeof(ComboConfig),
                typeof(CustomCombo),
                new PropertyMetadata(null));

      

        public ObservableCollection<Item> FilteredItems { get; set; }

       
        public ObservableCollection<Item> SelectedItems { get; set; }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            //if (Config?.AutoOpen == true)
            //{
            //    ListBorder.Visibility = Visibility.Visible;
            //}

            RefreshList();
        }

        private void List_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                var selected = List.SelectedItem as Item;
                if (selected != null)
                {
                    SelectItem(selected);
                }

                Dispatcher.BeginInvoke(() =>
                {
                    var request = new TraversalRequest(FocusNavigationDirection.Next);
                    List.MoveFocus(request);
                });
            }
        }

        private void SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                EnterPressed?.Invoke();
                return;
            }
            if (e.Key == Key.Down && List.Items.Count > 0)
            {
                e.Handled = true;
                ListBorder.Visibility = Visibility.Visible;

                List.SelectedIndex = 0;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var container = List.ItemContainerGenerator.ContainerFromIndex(0) as ListBoxItem;
                    container?.Focus();
                }), System.Windows.Threading.DispatcherPriority.Input);
            }

            if (e.Key == Key.Space )
            {
                e.Handled = true; 

                DropDownToggle_Click(this, new RoutedEventArgs());
            }

        }



        //private void DropDownToggle_Click(object sender, RoutedEventArgs e)
        //{
        //    if (ListBorder.Visibility == Visibility.Visible)
        //    {
        //        ListBorder.Visibility = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        ListBorder.Visibility = Visibility.Visible;

        //        Dispatcher.BeginInvoke(new Action(() =>
        //        {
        //            SearchBox.Focus();
        //            Keyboard.Focus(SearchBox);
        //        }), System.Windows.Threading.DispatcherPriority.Input);
        //    }
        //}

        //private void DropDownToggle_Click(object sender, RoutedEventArgs e)
        //{
        //    if (IsAutoOpenEnabled)
        //    {
        //        // always open if enabled
        //        ListBorder.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        // normal toggle behavior
        //        if (ListBorder.Visibility == Visibility.Visible)
        //            ListBorder.Visibility = Visibility.Collapsed;
        //        else
        //            ListBorder.Visibility = Visibility.Visible;
        //    }

        //    Dispatcher.BeginInvoke(new Action(() =>
        //    {
        //        SearchBox.Focus();
        //        Keyboard.Focus(SearchBox);
        //    }), System.Windows.Threading.DispatcherPriority.Input);
        //}

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            ToggleDropdown();
        }
        private void DropDownToggle_Click(object sender, RoutedEventArgs e)
        {
            //if (Config?.AutoOpen == true)
            //{
            //    ListBorder.Visibility = Visibility.Visible;
            //    Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        SearchBox.Focus();
            //        Keyboard.Focus(SearchBox);
            //    }), System.Windows.Threading.DispatcherPriority.Input);
            //    return;
            //}

            //if (ListBorder.Visibility == Visibility.Visible)
            //{
            //    ListBorder.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    ListBorder.Visibility = Visibility.Visible;

            //    Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        SearchBox.Focus();
            //        Keyboard.Focus(SearchBox);
            //    }), System.Windows.Threading.DispatcherPriority.Input);
            //}

            ToggleDropdown();
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (Item)((Button)sender).CommandParameter;

            SelectedItems.Remove(item);

            if (List.SelectedItem == item)
                List.SelectedItem = null;

            RefreshList();
        }

        private void List_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
            var item = (sender as ListBox).SelectedItem as Item;
            if (item != null)
            {
                SelectItem(item);
            }
        }
   
        private void SelectItem(Item selected)
        {
            if (selected != null && !SelectedItems.Contains(selected))
            {
                SelectedItems.Add(selected);
            }
           
            List.SelectedItem = null;
            SearchBox.Text = string.Empty;
            SearchBox.Clear();

            RefreshList();

            Dispatcher.BeginInvoke(new Action(() =>
            {
                SearchBox.Focus();
                Keyboard.Focus(SearchBox);
            }), System.Windows.Threading.DispatcherPriority.Input);
        }
        //feature branch pull request
        private void RefreshList()
        {
            if (Items == null) return;

            string text = SearchBox.Text?.ToLower() ?? "";

            FilteredItems.Clear();

            var result = Items
                .Where(x =>
                    !SelectedItems.Any(s => s.Name == x.Name) &&
                    x.Name.ToLower().Contains(text));

            foreach (var item in result)
                FilteredItems.Add(item);
        }
        private void ToggleDropdown()
        {
            if (ListBorder.Visibility == Visibility.Visible)
            {
                ListBorder.Visibility = Visibility.Collapsed;
                Keyboard.ClearFocus();
            }
            else
            {
                ListBorder.Visibility = Visibility.Visible;

                Dispatcher.BeginInvoke(() =>
                {
                    SearchBox.Focus();
                });
            }
        }

        public void FocusSearchBox()
        {
            SearchBox.Focus();
        }
        //hello
        public List<Item> GetSelectedItems()
        {
            return SelectedItems.ToList();
        }
    }

    public class Item
    {
        public string Name { get; set; }
    }

    public class ComboConfig
    {
        public bool AutoOpen { get; set; } = false;
       
    }
}

